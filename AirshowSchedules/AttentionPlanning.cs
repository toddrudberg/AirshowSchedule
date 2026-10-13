using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AirshowSchedules;

internal sealed class AttentionItem
{
    internal Airshow Show = null!;
    internal string Category = "";
    internal string Details = "";
    internal int Priority;
    internal bool Reviewed;
    internal string Key => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"{Show.ID}|{Category}|{Details}")));
}

internal sealed class AttentionImport
{
    private static string cachedHistory = "";
    private static Dictionary<int, string> cachedDates = new();
    internal List<IcasChange> Proposed = new();
    internal Dictionary<int, string> AppliedDateChanges = new();
    internal string Description = "";
    internal static AttentionImport? Load(string database, string contacts, AirshowGroup active, List<cContact> activeContacts, int year)
    {
        var history = IcasUpdateStore.History(database, contacts).Where(r => r.Status == "Applied" && r.Year == year).ToList();
        var record = history.FirstOrDefault();
        if (record == null) return null;
        string source = Directory.GetFiles(record.Folder, "source.*").Single();
        var imported = IcasHtmlImporter.Parse(File.ReadAllText(source), year);
        var result = new AttentionImport
        {
            Proposed = new IcasUpdatePlan(active, activeContacts, imported.Shows, imported.Contacts, year).Changes,
            Description = $"Latest applied ICAS download: {record.SourceName} ({record.Created:g})."
        };
        AirshowGroup Read(IcasUpdateRecord update, string name)
        {
            using var reader = File.OpenRead(Path.Combine(update.Folder, name));
            return (AirshowGroup)new XmlSerializer(typeof(AirshowGroup)).Deserialize(reader)!;
        }
        string historyKey = string.Join("|", history.Select(r => r.Folder));
        if (historyKey == cachedHistory)
        {
            result.AppliedDateChanges = new Dictionary<int, string>(cachedDates);
            return result;
        }
        foreach (var update in history)
        {
            var before = Read(update, "before.asg.xml").GetAirshows().ToDictionary(s => s.ID);
            foreach (var after in Read(update, "after.asg.xml").GetAirshows())
                if (!result.AppliedDateChanges.ContainsKey(after.ID) && before.TryGetValue(after.ID, out var old) &&
                    (old.date_start != after.date_start || old.date_finish != after.date_finish))
                    result.AppliedDateChanges[after.ID] = $"{old.date_start}–{old.date_finish} → {after.date_start}–{after.date_finish} (update {update.Created:g})";
        }
        cachedHistory = historyKey;
        cachedDates = new Dictionary<int, string>(result.AppliedDateChanges);
        return result;
    }
}

internal static class AttentionPlanning
{
    internal static List<AttentionItem> Build(AirshowGroup database, List<cContact> contacts, int year, DateTime today,
        bool includePast, AttentionImport? import, ISet<string> reviewed)
    {
        var result = new List<AttentionItem>();
        var shows = CalendarPlanning.ForYear(database.GetAirshows(), year)
            .Where(s => includePast || (CalendarPlanning.Dates(s, out _, out var finish) && finish >= today.Date)).ToList();
        void Add(Airshow show, string category, string details, int priority)
        {
            var item = new AttentionItem { Show = show, Category = category, Details = details, Priority = priority };
            item.Reviewed = reviewed.Contains(item.Key);
            if (!result.Any(i => i.Key == item.Key)) result.Add(item);
        }
        foreach (var show in shows)
        {
            bool sourceCancelled = import?.Proposed.Any(c => c.TargetId == show.ID && c.Group == "Cancellation warnings") == true;
            if (CalendarPlanning.Cancelled(show) || sourceCancelled)
                Add(show, "Cancellation warning", $"ICAS labels this event cancelled. Your status is {CalendarPlanning.Status(show)}. Confirm with the organizer before changing your plans.", CalendarPlanning.Booked(show) ? 0 : 1);
            if (CalendarPlanning.Booked(show))
            {
                var proposedDate = import?.Proposed.FirstOrDefault(c => c.TargetId == show.ID && c.Field == "Dates");
                if (proposedDate != null)
                    Add(show, "Booked dates changed", $"Your database: {proposedDate.Before}\nLatest ICAS download: {proposedDate.After}\nThese dates have not been accepted. Confirm your booking dates.", 0);
                else if (import?.AppliedDateChanges.TryGetValue(show.ID, out var dates) == true)
                    Add(show, "Booked dates changed", $"An ICAS update changed these dates:\n{dates}\nCurrent dates: {show.date_start}–{show.date_finish}. Confirm that your booking matches.", 0);
                var overlaps = database.GetAirshows().Where(other => other.ID != show.ID && CalendarPlanning.Booked(other) && CalendarPlanning.Overlap(show, other)).ToList();
                if (overlaps.Count > 0)
                    Add(show, "Overlapping commitments", "This booking overlaps actual event dates with:\n" + string.Join("\n", overlaps.Select(s => $"{s.name_airshow}: {s.date_start}–{s.date_finish} ({CalendarPlanning.Status(s)})").OrderBy(s => s)) + "\nReview whether you can cover both events.", 0);
            }
            if (import?.Proposed.Any(c => c.TargetId == show.ID && c.Group == "Missing from download") == true)
                Add(show, "Missing from ICAS", "This show is absent from the latest applied ICAS download. It remains in your database; absence does not establish cancellation.\n" + import.Description, CalendarPlanning.Booked(show) ? 0 : 2);
            if (show.Status != Airshow.eStatus.NO && !CalendarPlanning.Cancelled(show) && !sourceCancelled)
            {
                var linked = contacts.Where(c => c.showIDs.Contains(show.ID)).ToList();
                if (!linked.Any(c => !string.IsNullOrWhiteSpace(c.phone) || c.emailAddresses.Any(e => !string.IsNullOrWhiteSpace(e)) || (c.name ?? "").Contains('@')))
                    Add(show, "Missing contact details", linked.Count == 0 ? "No contact is linked to this show. Add an organizer's phone or email." : "Linked contacts have no phone or email. Add a way to reach the organizer.", CalendarPlanning.Booked(show) ? 1 : 3);
                if (show.Status == Airshow.eStatus.pursue)
                    Add(show, "Pursue next step", "This show is marked Pursue. Review the next action and record any follow-up in your notes.", 2);
            }
        }
        return result.OrderBy(i => i.Priority).ThenBy(i => i.Show.date_start).ThenBy(i => i.Show.name_airshow).ThenBy(i => i.Category).ToList();
    }
}

internal static class AttentionReviewStore
{
    private static string PathFor(string database, string contacts) => Path.GetFullPath(database) + ".attention-" +
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Path.GetFullPath(contacts).ToUpperInvariant())))[..12] + ".json";
    internal static HashSet<string> Load(string database, string contacts)
    {
        string path = PathFor(database, contacts);
        return File.Exists(path) ? JsonConvert.DeserializeObject<HashSet<string>>(File.ReadAllText(path)) ?? new() : new();
    }
    internal static void Save(string database, string contacts, HashSet<string> reviewed)
    {
        string path = PathFor(database, contacts);
        string temporary = path + ".tmp";
        File.WriteAllText(temporary, JsonConvert.SerializeObject(reviewed.OrderBy(s => s), Formatting.Indented));
        File.Move(temporary, path, true);
    }
}
