using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AirshowSchedules;

public sealed class SilentScheduleReceipt
{
    public string ChangeId { get; set; } = "";
    public int LocalId { get; set; }
}

internal sealed record SilentShow(string Id, string Name, string Start, string Finish, string City, string State,
    string Region, string Status, string Notes, List<string> TeamNotes, List<string> Performers, List<string> Links);
internal sealed record SilentContact(string Id, string Name, string Phone, string Address, List<string> Emails, List<string> ShowIds);
internal sealed record SilentWeekend(int Month, string Start, string Finish, List<string> ShowIds, List<string> WeekdayIds);
internal sealed record SilentSnapshot(int SchemaVersion, int Year, string PublicationId, List<SilentShow> Shows,
    List<SilentContact> Contacts, List<SilentWeekend> Weekends, Dictionary<string, string> Regions,
    Dictionary<string, string> StatusColors, List<SilentScheduleReceipt> Receipts);
internal sealed record SilentPublishState(int Year, int Revision, DateTimeOffset? PublishedAt);
internal sealed record SilentContactDetails(string Name, string Phone, string Address, List<string> Emails)
{
    public override string ToString() => $"Name: {Name}\nPhone: {Phone}\nAddress: {Address}\nEmail: {string.Join(", ", Emails)}";
}
internal sealed record SilentChange(string Id, int Year, string Kind, string User, DateTimeOffset CreatedAt,
    string? ShowId, string? BaseStatus, string? Status, string? Name, string? Start, string? Finish, string? City,
    string? State, string? Phone, string? Address, List<string>? Emails, string? ContactId, SilentContactDetails? BaseContact,
    bool WaitForContact);
internal sealed record SilentReview(string Subject, string SnapshotValue, string LocalValue, string ProposedValue, bool Conflict,
    string? Blocked, SilentScheduleReceipt? Saved);

internal static class SilentSchedule
{
    internal static string Id(int id) => id.ToString(CultureInfo.InvariantCulture);
    private static string Date(DateTime date) => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    internal static SilentSnapshot Build(AirshowGroup database, List<cContact> contacts, Regions regions, int year,
        IReadOnlyList<SilentScheduleReceipt>? contactReceipts = null)
    {
        if (database.GetAirshows().Select(s => s.ID).Distinct().Count() != database.GetAirshows().Count ||
            contacts.Select(c => c.ID).Distinct().Count() != contacts.Count)
            throw new FormatException("Resolve duplicate show or contact IDs before publishing.");
        var local = CalendarPlanning.ForYear(database.GetAirshows(), year);
        var ids = local.Select(s => s.ID).ToHashSet();
        var shows = local.Select(s => new SilentShow(Id(s.ID), s.name_airshow ?? "", s.date_start, s.date_finish,
            s.location.city ?? "", s.location.state ?? "",
            regions.ForState(s.location.state),
            s.Status.ToString(), s.Notes_AirshowStuff ?? "", s.UndauntedNotes, s.Performers.performer, s.AirshowLinks)).ToList();
        var linked = contacts.Where(c => c.showIDs.Any(ids.Contains)).Select(c => new SilentContact(Id(c.ID), c.name ?? "",
            c.phone ?? "", c.address ?? "", c.emailAddresses, c.showIDs.Where(ids.Contains).Distinct().Select(Id).ToList())).ToList();
        var weekends = Enumerable.Range(1, 12).SelectMany(month => CalendarPlanning.WeekendStarts(year, month).Select(start =>
            new SilentWeekend(month, Date(start), Date(start.AddDays(1)),
                CalendarPlanning.Weekend(local, start).Select(s => Id(s.ID)).ToList(),
                CalendarPlanning.NearbyWeekdays(local, start).Select(s => Id(s.ID)).ToList()))).ToList();
        var colors = Enum.GetValues<Airshow.eStatus>().ToDictionary(status => status.ToString(), status =>
        {
            Color color = CalendarPlanning.StatusColor(status);
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        });
        var receipts = database.SilentScheduleReceipts.Concat(contacts.Where(c => c.SilentScheduleChangeId != null)
            .Select(c => new SilentScheduleReceipt { ChangeId = c.SilentScheduleChangeId!, LocalId = c.ID }))
            .Concat(contactReceipts ?? Array.Empty<SilentScheduleReceipt>()).DistinctBy(r => r.ChangeId).ToList();
        return new SilentSnapshot(1, year, Guid.NewGuid().ToString(), shows, linked, weekends, regions.myRegions, colors, receipts);
    }

    private static Airshow? FindShow(AirshowGroup database, string? showId)
    {
        if (showId == null) return null;
        int id;
        if (showId.StartsWith("web:", StringComparison.Ordinal))
        {
            var receipt = database.SilentScheduleReceipts.FirstOrDefault(r => r.ChangeId == showId[4..]);
            if (receipt == null) return null;
            id = receipt.LocalId;
        }
        else if (!int.TryParse(showId, NumberStyles.Integer, CultureInfo.InvariantCulture, out id)) return null;
        return database.GetAirshows().SingleOrDefault(s => s.ID == id);
    }

    private static int? ContactId(List<cContact> contacts, IReadOnlyList<SilentScheduleReceipt> receipts, string? identifier)
    {
        if (identifier != null && identifier.StartsWith("web:", StringComparison.Ordinal))
        {
            string creation = identifier[4..];
            return contacts.SingleOrDefault(contact => contact.SilentScheduleChangeId == creation)?.ID
                ?? receipts.FirstOrDefault(receipt => receipt.ChangeId == creation)?.LocalId;
        }
        return int.TryParse(identifier, NumberStyles.None, CultureInfo.InvariantCulture, out int id) ? id : null;
    }

    private static (SilentContactDetails Details, bool Conflict) MergeContact(SilentContactDetails baseline,
        SilentContactDetails proposal, SilentContactDetails local)
    {
        bool conflict = false;
        T Merge<T>(T before, T next, T current, Func<T, T, bool> equals)
        {
            if (equals(before, next)) return current;
            conflict |= !equals(before, current) && !equals(next, current);
            return next;
        }
        var merged = new SilentContactDetails(Merge(baseline.Name, proposal.Name, local.Name, string.Equals),
            Merge(baseline.Phone, proposal.Phone, local.Phone, string.Equals),
            Merge(baseline.Address, proposal.Address, local.Address, string.Equals),
            Merge(baseline.Emails, proposal.Emails, local.Emails, Enumerable.SequenceEqual));
        return (merged, conflict);
    }

    private static SilentContactDetails ContactDetails(cContact contact) =>
        new(contact.name ?? "", contact.phone ?? "", contact.address ?? "", contact.emailAddresses);

    private static SilentContactDetails ProposedContact(SilentChange change) =>
        new(change.Name ?? "", change.Phone ?? "", change.Address ?? "", change.Emails ?? new());

    internal static SilentReview Inspect(AirshowGroup database, List<cContact> contacts, SilentChange change,
        IReadOnlyList<SilentScheduleReceipt>? contactReceipts = null)
    {
        contactReceipts ??= Array.Empty<SilentScheduleReceipt>();
        var saved = database.SilentScheduleReceipts.FirstOrDefault(r => r.ChangeId == change.Id)
            ?? contactReceipts.FirstOrDefault(r => r.ChangeId == change.Id);
        var savedContact = contacts.SingleOrDefault(c => c.SilentScheduleChangeId == change.Id);
        if (savedContact != null)
            saved = new SilentScheduleReceipt { ChangeId = change.Id, LocalId = savedContact.ID };
        if (saved != null)
            return new SilentReview(change.Name ?? change.ShowId ?? change.Kind, "", "Saved on this PC", change.Status ?? "",
                false, null, saved);

        if (change.Kind is "editContact" or "deleteContact")
        {
            bool deleting = change.Kind == "deleteContact";
            var baseline = change.BaseContact;
            if (change.WaitForContact)
                return new(baseline?.Name ?? "Contact", "", "", "", false,
                    "Import or acknowledge the earlier web change for this contact first.", null);
            int? id = ContactId(contacts, contactReceipts, change.ContactId);
            var contact = contacts.SingleOrDefault(c => c.ID == id);
            if (baseline == null || id == null || (!deleting && contact == null))
                return new(baseline?.Name ?? "Contact", "", "", "", false,
                    "Import the contact creation first, or restore the contact if it was removed.", null);
            if (contact == null)
                return new(baseline.Name, baseline.ToString(), "Already removed", "Delete contact", false, null, null);
            var local = ContactDetails(contact);
            if (deleting)
            {
                bool changed = local.Name != baseline.Name || local.Phone != baseline.Phone || local.Address != baseline.Address ||
                    !local.Emails.SequenceEqual(baseline.Emails);
                string removal = $"Delete contact and remove all {contact.showIDs.Distinct().Count()} show links.";
                return new(baseline.Name, baseline.ToString(), local.ToString(), removal, changed, null, null);
            }
            var (merged, conflict) = MergeContact(baseline, ProposedContact(change), local);
            return new(baseline.Name, baseline.ToString(), local.ToString(), merged.ToString(), conflict, null, null);
        }

        var target = FindShow(database, change.ShowId);
        bool needsShow = change.Kind != "addShow";
        string? blocked = needsShow && target == null ? "Import the linked show first, or restore it if it was removed." : null;
        if (target != null && CalendarPlanning.ForYear(new[] { target }, change.Year).Count == 0)
            blocked = "The linked show is outside the published year.";
        string localStatus = target?.Status.ToString() ?? "";
        return change.Kind switch
        {
            "status" => new(target?.name_airshow ?? change.ShowId ?? "Show", change.BaseStatus ?? "", localStatus, change.Status ?? "",
                target != null && localStatus != change.BaseStatus && localStatus != change.Status, blocked, null),
            "addShow" => new(change.Name ?? "", "", "", $"{change.Start} to {change.Finish}\n{change.City}, {change.State}\n{change.Status}",
                false, null, null),
            "addContact" => new($"Contact for {target?.name_airshow ?? change.ShowId}", "", "",
                $"{change.Name}\n{change.Phone}\n{change.Address}\n{string.Join(", ", change.Emails ?? new())}", false, blocked, null),
            _ => new("", "", "", "", false, "This change requires a newer version of the Windows app.", null)
        };
    }

    internal static SilentScheduleReceipt Apply(string databasePath, string contactsPath, AirshowGroup database,
        List<cContact> contacts, string databaseHash, string contactsHash, SilentChange change)
    {
        var contactReceipts = SilentContactSaves.Read(contactsPath, contactsHash);
        var review = Inspect(database, contacts, change, contactReceipts);
        if (review.Saved != null) return review.Saved;
        if (review.Blocked != null) throw new InvalidOperationException(review.Blocked);
        if (IcasUpdateStore.Hash(databasePath) != databaseHash || IcasUpdateStore.Hash(contactsPath) != contactsHash)
            throw new IOException("The local files changed during review. Refresh and review again.");
        var receipt = new SilentScheduleReceipt { ChangeId = change.Id };
        var shows = database.GetAirshows();
        switch (change.Kind)
        {
            case "addShow":
                if (string.IsNullOrWhiteSpace(change.Name)) throw new FormatException("The proposed show needs a name.");
                var show = new Airshow
                {
                    ID = checked(shows.Select(s => s.ID).Concat(contacts.SelectMany(c => c.showIDs))
                        .Concat(database.SilentScheduleReceipts.Select(r => r.LocalId)).DefaultIfEmpty(-1).Max() + 1),
                    name_airshow = change.Name, date_start = change.Start ?? "", date_finish = change.Finish ?? "",
                    date_added = Date(DateTime.Today), Status = ParseStatus(change.Status),
                    location = new Airshow.cLocation { city = change.City ?? "", state = change.State ?? "" }
                };
                if (!CalendarPlanning.Dates(show, out var start, out var finish) || start.Year > change.Year || finish.Year < change.Year)
                    throw new FormatException("The proposed show dates are invalid for the published year.");
                shows.Add(show);
                receipt.LocalId = show.ID;
                break;
            case "status":
                var target = FindShow(database, change.ShowId)!;
                target.Status = ParseStatus(change.Status);
                receipt.LocalId = target.ID;
                break;
            case "addContact":
                if (string.IsNullOrWhiteSpace(change.Name)) throw new FormatException("The proposed contact needs a name.");
                var contact = new cContact
                {
                    ID = checked(contacts.Select(c => c.ID).Concat(contactReceipts.Select(r => r.LocalId)).DefaultIfEmpty(-1).Max() + 1),
                    name = change.Name, phone = change.Phone ?? "", address = change.Address ?? "",
                    emailAddresses = change.Emails ?? new(), showIDs = new() { FindShow(database, change.ShowId)!.ID },
                    SilentScheduleChangeId = change.Id
                };
                contacts.Add(contact);
                cContact.SaveMe(contacts, contactsPath);
                receipt.LocalId = contact.ID;
                return receipt;
            case "editContact":
            case "deleteContact":
                receipt.LocalId = ContactId(contacts, contactReceipts, change.ContactId)!.Value;
                int index = contacts.FindIndex(c => c.ID == receipt.LocalId);
                string? originalCreateId = null;
                if (change.Kind == "editContact")
                {
                    var existing = contacts[index];
                    var merged = MergeContact(change.BaseContact!, ProposedContact(change), ContactDetails(existing)).Details;
                    existing.name = merged.Name;
                    existing.phone = merged.Phone;
                    existing.address = merged.Address;
                    existing.emailAddresses = merged.Emails;
                }
                else if (index >= 0)
                {
                    originalCreateId = contacts[index].SilentScheduleChangeId;
                    contacts.RemoveAt(index);
                }
                SilentContactSaves.Commit(contactsPath, contactsHash, contacts, receipt, originalCreateId);
                return receipt;
        }
        database.SilentScheduleReceipts.Add(receipt);
        SaveShows(database, databasePath);
        return receipt;
    }

    private static Airshow.eStatus ParseStatus(string? value) =>
        Enum.TryParse<Airshow.eStatus>(value, out var status) && status.ToString() == value
            ? status : throw new FormatException("The proposed show status is invalid.");

    private static void SaveShows(AirshowGroup database, string path)
    {
        string temp = path + "." + Guid.NewGuid().ToString("N") + ".asg.xml";
        try { AirshowGroup.SaveMe(database, temp); File.Move(temp, path, overwrite: true); }
        finally { File.Delete(temp); }
    }
}

internal sealed class SilentScheduleClient : IDisposable
{
    internal const string DefaultUrl = "https://schedule.undauntedairshows.com/";
    private readonly HttpClient http;
    internal Uri Site { get; }

    internal SilentScheduleClient(string? url = null, string? token = null)
    {
        Site = new Uri(url ?? Environment.GetEnvironmentVariable("SILENT_SCHEDULE_URL") ?? DefaultUrl);
        if ((Site.Scheme != Uri.UriSchemeHttps && !(Site.IsLoopback && Site.Scheme == Uri.UriSchemeHttp)) ||
            Site.AbsolutePath != "/" || Site.UserInfo.Length != 0 || Site.Query.Length != 0 || Site.Fragment.Length != 0)
            throw new InvalidOperationException("Configure a schedule site root over HTTPS. HTTP is available for local sample testing.");
        token ??= Environment.GetEnvironmentVariable("SILENT_SCHEDULE_PC_TOKEN");
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("Set SILENT_SCHEDULE_PC_TOKEN in your Windows user environment and reopen the app.");
        http = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false, UseCookies = false })
        { BaseAddress = Site, Timeout = TimeSpan.FromSeconds(30) };
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    internal Task<SilentPublishState> State(int year) => Send<SilentPublishState>(HttpMethod.Get, $"api/v1/state/{year}");
    internal Task<SilentPublishState> Publish(SilentSnapshot snapshot) => Send<SilentPublishState>(HttpMethod.Put, "api/v1/snapshots", snapshot);
    internal Task<List<SilentChange>> Changes(int year) => Send<List<SilentChange>>(HttpMethod.Get, $"api/v1/changes/{year}");
    internal Task<SilentScheduleReceipt> Ack(SilentScheduleReceipt receipt) =>
        Send<SilentScheduleReceipt>(HttpMethod.Post, $"api/v1/changes/{Uri.EscapeDataString(receipt.ChangeId)}/ack", new { receipt.LocalId });

    private async Task<T> Send<T>(HttpMethod method, string path, object? payload = null)
    {
        using var request = new HttpRequestMessage(method, path);
        if (payload != null) request.Content = JsonContent.Create(payload);
        using var response = await http.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            string? message = null;
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var data = await response.Content.ReadFromJsonAsync<JsonElement>();
                if (data.TryGetProperty("error", out var error)) message = error.GetString();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized) message = "The PC publishing token was rejected.";
            throw new IOException(message ?? $"The schedule service returned HTTP {(int)response.StatusCode}.");
        }
        return await response.Content.ReadFromJsonAsync<T>() ?? throw new IOException("The schedule service returned no data.");
    }
    public void Dispose() => http.Dispose();
}
