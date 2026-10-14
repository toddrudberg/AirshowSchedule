namespace AirshowSchedules;

internal sealed record MaintenanceChoice(Airshow Show, string Reason, Airshow? Incoming = null, bool Suggested = false)
{
    internal bool Selected;
    internal List<Airshow> ComparisonShows = new();
}

internal sealed class DatabaseMaintenance
{
    internal AirshowGroup Database;
    internal List<cContact> Contacts;
    internal readonly List<string> Changes = new();
    private readonly AirshowGroup incoming;
    private readonly List<cContact> incomingContacts;
    internal int Year => incoming.AirshowYearOfInterest;

    internal DatabaseMaintenance(AirshowGroup active, List<cContact> contacts, AirshowGroup source, List<cContact> sourceContacts)
    {
        Database = active.DeepCopy();
        Contacts = cContact.DeepCopy(contacts);
        incoming = source.DeepCopy();
        incomingContacts = cContact.DeepCopy(sourceContacts);
    }

    private IEnumerable<Airshow> Current => Database.GetAirshows().Where(s => s.Year == incoming.AirshowYearOfInterest);
    private static string Key(string? value) => (value ?? "").Trim().ToUpperInvariant();
    private static bool SamePlace(Airshow a, Airshow b) => Key(a.location.city) == Key(b.location.city) && Key(a.location.state) == Key(b.location.state);
    private static bool Match(Airshow a, Airshow b) => SamePlace(a, b) && a.Year == b.Year && a.WeekNumber == b.WeekNumber;
    private static bool Cancelled(Airshow s) => s.name_airshow.Contains("cancelled", StringComparison.OrdinalIgnoreCase)
        || s.name_airshow.Contains("canceled", StringComparison.OrdinalIgnoreCase)
        || (s.Notes_AirshowStuff ?? "").Contains("canc", StringComparison.OrdinalIgnoreCase);
    private IEnumerable<Airshow> Ordered => Current.OrderBy(s => s.date_start).ThenBy(s => s.name_airshow).ThenBy(s => s.ID);

    internal List<MaintenanceChoice> CleanupChoices() => Ordered.Select(s =>
    {
        bool duplicate = Current.Any(other => other.ID != s.ID && SamePlace(s, other));
        bool missing = !incoming.GetAirshows().Any(other => Match(s, other));
        var comparison = incoming.GetAirshows().Where(other => SamePlace(s, other) && other.Year == s.Year)
            .OrderBy(other => other.date_start).ThenBy(other => other.name_airshow).ToList();
        var reasons = new List<string>();
        if (duplicate) reasons.Add("Possible duplicate: same location/year");
        if (missing) reasons.Add("Missing from comparison database (may have moved dates)");
        if (reasons.Count == 0) return new MaintenanceChoice(s, "");
        bool sameDates = comparison.Any(other => other.date_start == s.date_start && other.date_finish == s.date_finish);
        bool sameNameAndDates = comparison.Any(other => other.date_start == s.date_start && other.date_finish == s.date_finish && Key(other.name_airshow) == Key(s.name_airshow));
        reasons.Add(sameNameAndDates ? "Name and dates match latest calendar" : sameDates ? "Dates match latest calendar; name differs"
            : comparison.Count > 0 ? "Dates differ from latest calendar — review which entry to keep" : "No latest calendar entry at this location");
        return new MaintenanceChoice(s, string.Join("; ", reasons)) { ComparisonShows = comparison };
    }).Where(c => c.Reason.Length > 0).ToList();

    internal List<Airshow> RelatedActiveShows(Airshow show) => Ordered.Where(other => SamePlace(show, other)).ToList();

    internal List<MaintenanceChoice> CancellationChoices() => Ordered.Where(s => Cancelled(s) || incoming.GetAirshows().Any(other => Match(s, other) && Cancelled(other)))
        .Select(s => new MaintenanceChoice(s, "Cancellation warning — review before removing")).ToList();

    internal List<MaintenanceChoice> MergeChoices() => Ordered.SelectMany(s =>
    {
        var matches = incoming.GetAirshows().Where(other => Match(s, other)).ToList();
        return matches.Select(other => new MaintenanceChoice(s, matches.Count == 1 ? "Merge additional fields" : "Multiple matches: choose at most one source for this show", other, matches.Count == 1));
    }).ToList();

    internal void RemoveSelected(IEnumerable<MaintenanceChoice> choices)
    {
        foreach (var choice in choices.Where(c => c.Selected))
        {
            if (!Database.GetAirshows().Remove(choice.Show)) continue;
            cContact.RemoveAirshowReference(Contacts, choice.Show);
            Changes.Add($"Remove: {choice.Show}");
        }
    }

    internal void MergeSelected(IEnumerable<MaintenanceChoice> choices)
    {
        var selected = choices.Where(c => c.Selected).ToList();
        if (selected.GroupBy(c => c.Show.ID).Any(group => group.Count() > 1))
            throw new InvalidOperationException("Choose at most one comparison show for each active show.");
        foreach (var choice in selected)
        {
            var source = choice.Incoming!;
            var contacts = cContact.getContacts(incomingContacts, source).Where(c => !string.IsNullOrWhiteSpace(c.name));
            foreach (var contact in contacts)
                if (Contacts.Count(c => Key(c.name) == Key(contact.name)) > 1)
                    throw new InvalidOperationException($"Contact '{contact.name}' occurs more than once in the active contacts database. Resolve it before applying this merge.");
        }
        foreach (var choice in selected)
        {
            choice.Show.mergeAdditionalInformation(choice.Show, choice.Incoming!, Contacts, incomingContacts);
            Changes.Add($"Merge: {choice.Show} ← {choice.Incoming}");
        }
    }
}
