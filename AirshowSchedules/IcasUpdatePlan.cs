using System.Text.RegularExpressions;

namespace AirshowSchedules;

internal sealed class IcasChange
{
    public string Group = "";
    public string Show = "";
    public string Field = "";
    public string Before = "";
    public string After = "";
    public bool Selected;
    public Action? Apply;
}

internal sealed class IcasUpdatePlan
{
    internal AirshowGroup Database;
    internal List<cContact> Contacts;
    internal List<IcasChange> Changes = new();
    internal int Year;
    internal int Unchanged;
    internal int CancellationCount;
    private static string Key(string? s) => Regex.Replace((s ?? "").ToLowerInvariant()
        .Replace("(cancelled)", "").Replace("air show", "airshow"), @"\s+", " ").Trim();
    private static string Place(Airshow s) => Key(s.location.city) + "|" + Key(s.location.state);
    private static string ContactKey(cContact c) => Key(c.name) + "|" + Key(c.phone) + "|" +
        string.Join("|", c.emailAddresses.Select(Key).OrderBy(x => x));
    private static string ContactText(IEnumerable<cContact> contacts) => string.Join("\n", contacts.Select(c =>
        $"{c.name} — {c.phone} {string.Join(", ", c.emailAddresses)}").OrderBy(x => x));

    internal IcasUpdatePlan(AirshowGroup active, List<cContact> contacts, List<Airshow> incoming, List<cContact> incomingContacts, int year)
    {
        Database = active.DeepCopy();
        Contacts = cContact.DeepCopy(contacts);
        Year = year;
        var existing = Database.GetAirshows().Where(s => s.Year == year).ToList();
        if (Database.GetAirshows().Select(s => s.ID).Distinct().Count() != Database.GetAirshows().Count ||
            Contacts.Select(c => c.ID).Distinct().Count() != Contacts.Count)
            throw new FormatException("Duplicate database IDs must be repaired before an ICAS update.");
        var seen = new HashSet<int>();
        var claimed = new HashSet<int>();
        foreach (var source in incoming)
        {
            if (source.name_airshow.Contains("CANCELLED", StringComparison.OrdinalIgnoreCase))
            {
                CancellationCount++;
                Changes.Add(new IcasChange { Group = "Cancellation warnings", Show = source.name_airshow, Field = "ICAS cancellation",
                    After = $"{source.date_start} – {source.date_finish}\nICAS labels this show cancelled. Your booking status is preserved; review it separately." });
            }
            var byName = existing.Where(s => Key(s.name_airshow) == Key(source.name_airshow)).ToList();
            var candidates = byName.Where(s => Place(s) == Place(source)).ToList();
            if (candidates.Count > 1)
            {
                var exact = candidates.Where(s => s.date_start == source.date_start && s.date_finish == source.date_finish).ToList();
                if (exact.Count > 0) candidates = exact;
            }
            // A unique name permits a location correction. Never guess between multiple events.
            if (candidates.Count == 0 && byName.Count == 1) candidates = byName;
            if (candidates.Count == 0 && byName.Count > 1) candidates = byName;
            if (candidates.Count == 0 && byName.Count == 0)
                candidates = existing.Where(s => Place(s) == Place(source) && s.date_start == source.date_start && s.date_finish == source.date_finish).ToList();
            foreach (var s in candidates) seen.Add(s.ID);
            var sourceContacts = incomingContacts.Where(c => c.showIDs.Contains(source.ID)).ToList();
            if (incoming.Count(s => Key(s.name_airshow) == Key(source.name_airshow) && Place(s) == Place(source)
                && s.date_start == source.date_start && s.date_finish == source.date_finish) > 1)
            {
                Changes.Add(new IcasChange { Group = "Needs review", Show = source.name_airshow, Field = "Duplicate source event",
                    After = "This event occurs more than once in the download. Resolve it manually." });
                continue;
            }
            if (candidates.Count > 1 || (candidates.Count == 1 && !claimed.Add(candidates[0].ID)))
            {
                Changes.Add(new IcasChange { Group = "Needs review", Show = source.name_airshow, Field = "Ambiguous match",
                    Before = string.Join("\n", candidates.Select(s => s.ToString())), After = "Resolve this match manually; no automatic changes." });
                continue;
            }
            if (candidates.Count == 0)
            {
                var added = Airshow.DeepCopy(new List<Airshow> { source })[0];
                Changes.Add(new IcasChange { Group = "New shows", Show = source.name_airshow, Field = "New event", After =
                    $"{source.date_start} – {source.date_finish}\n{source.location.city}, {source.location.state}\nPerformers: {string.Join(", ", source.Performers.performer)}\n{ContactText(sourceContacts)}",
                    Selected = true, Apply = () =>
                    {
                        added.ID = Database.GetAirshows().Select(s => s.ID).DefaultIfEmpty(-1).Max() + 1;
                        Database.GetAirshows().Add(added);
                        ReplaceContacts(added.ID, sourceContacts);
                    } });
                continue;
            }
            var target = candidates[0];
            int beforeCount = Changes.Count;
            void Field(string name, string before, string after, Action apply)
            {
                if (before == after) return;
                Changes.Add(new IcasChange { Group = "Updated shows", Show = source.name_airshow, Field = name,
                    Before = before, After = after, Selected = true, Apply = apply });
            }
            // Cosmetic capitalization differences do not overwrite legacy names.
            if (Key(target.name_airshow) != Key(source.name_airshow) ||
                target.name_airshow.Contains("CANCELLED", StringComparison.OrdinalIgnoreCase) != source.name_airshow.Contains("CANCELLED", StringComparison.OrdinalIgnoreCase))
                Field("Name / cancellation", target.name_airshow, source.name_airshow, () => target.name_airshow = source.name_airshow);
            Field("Dates", $"{target.date_start} – {target.date_finish}", $"{source.date_start} – {source.date_finish}",
                () => { target.date_start = source.date_start; target.date_finish = source.date_finish; });
            if (Place(target) != Place(source))
                Field("Location", $"{target.location.city}, {target.location.state}", $"{source.location.city}, {source.location.state}",
                    () => { target.location.city = source.location.city; target.location.state = source.location.state; });
            if (!target.Performers.performer.Select(Key).OrderBy(x => x).SequenceEqual(source.Performers.performer.Select(Key).OrderBy(x => x)))
                Field("Performers", string.Join("\n", target.Performers.performer), string.Join("\n", source.Performers.performer),
                    () => target.Performers.performer = new List<string>(source.Performers.performer));
            var oldContacts = Contacts.Where(c => c.showIDs.Contains(target.ID)).ToList();
            // A blank ICAS contact cell does not erase existing contacts.
            if (sourceContacts.Count > 0 && !oldContacts.Select(ContactKey).OrderBy(x => x).SequenceEqual(sourceContacts.Select(ContactKey).OrderBy(x => x)))
                Changes.Add(new IcasChange { Group = "Contact changes", Show = source.name_airshow, Field = "Contacts (replace links for this show)",
                    Before = ContactText(oldContacts), After = ContactText(sourceContacts), Selected = oldContacts.Count == 0,
                    Apply = () => ReplaceContacts(target.ID, sourceContacts) });
            if (Changes.Count == beforeCount) Unchanged++;
        }
        foreach (var missing in existing.Where(s => !seen.Contains(s.ID)))
            Changes.Add(new IcasChange { Group = "Missing from download", Show = missing.name_airshow, Field = "Review only",
                Before = $"{missing.date_start} – {missing.date_finish}", After = "Kept in database. Absence does not establish cancellation." });
    }

    private void ReplaceContacts(int showId, List<cContact> incoming)
    {
        foreach (var contact in Contacts) contact.showIDs.RemoveAll(id => id == showId);
        foreach (var source in incoming)
        {
            // Reuse an exact contact; a conflicting name/phone creates a separate record
            // so that contacts shared with other shows and their custom details survive.
            var target = Contacts.FirstOrDefault(c => ContactKey(c) == ContactKey(source));
            if (target == null)
            {
                target = source.DeepCopy();
                target.ID = Contacts.Select(c => c.ID).DefaultIfEmpty(-1).Max() + 1;
                target.showIDs.Clear();
                Contacts.Add(target);
            }
            if (!target.showIDs.Contains(showId)) target.showIDs.Add(showId);
        }
    }

    internal int ApplySelected()
    {
        var selected = Changes.Where(c => c.Selected && c.Apply != null).ToList();
        foreach (var change in selected) change.Apply!();
        if (selected.Count > 0) Database.AirshowYearOfInterest = Year;
        return selected.Count;
    }
}
