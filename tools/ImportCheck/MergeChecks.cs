using AirshowSchedules;
using Newtonsoft.Json;

internal static class MergeChecks
{
    internal static void Run()
    {
        var master = new Airshow { ID = 388, name_airshow = "Truckee", Notes_AirshowStuff = "Existing notes", Status = Airshow.eStatus.contract };
        var incoming = new Airshow { ID = 7, name_airshow = "Truckee", Notes_AirshowStuff = "Incoming notes" };
        master.date_start = incoming.date_start = "2026-06-20";
        master.date_finish = incoming.date_finish = "2026-06-21";
        incoming.Performers.performer.Add("Performer");
        incoming.UndauntedNotes.Add("Follow up");
        incoming.AirshowLinks.Add("https://example.com");
        var activeContacts = new List<cContact> { new() { ID = 231, name = "Mark", phone = "111", showIDs = new() { 100 } } };
        var importedContacts = new List<cContact>
        {
            new() { ID = 233, name = "Mark", phone = "222", showIDs = new() { 7, 8 } },
            new() { ID = 236, name = "Carol", phone = "333", showIDs = new() { 7 } }
        };
        string sourceBefore = JsonConvert.SerializeObject(new { incoming, importedContacts });
        // Exercise the old call shape as well: the destination must be explicit.
        incoming.mergeAdditionalInformation(master, incoming, activeContacts, importedContacts);
        Check(cContact.getContacts(activeContacts, master).Count == 2, "Multiple contacts were not merged");
        Check(activeContacts.Single(c => c.name == "Mark").showIDs.SequenceEqual(new[] { 100, 388 }), "Imported IDs leaked into shared contact");
        Check(activeContacts.Single(c => c.name == "Carol").showIDs.SequenceEqual(new[] { 388 }), "New contact has wrong show link");
        Check(activeContacts.Select(c => c.ID).Distinct().Count() == 2, "Contact IDs collided");
        Check(master.Notes_AirshowStuff == "Existing notes" + Environment.NewLine + "Incoming notes", "Notes did not reach destination");
        Check(master.Status == Airshow.eStatus.contract && master.ID == 388, "Booking or identity changed");
        string merged = JsonConvert.SerializeObject(new { master, activeContacts });
        master.mergeAdditionalInformation(master, incoming, activeContacts, importedContacts);
        Check(merged == JsonConvert.SerializeObject(new { master, activeContacts }), "Repeated merge duplicated fields");
        Check(sourceBefore == JsonConvert.SerializeObject(new { incoming, importedContacts }), "Merge changed source data");
        System.Console.WriteLine("PASS: multiple-contact cleanup merge, shared contact links, destination notes, repeated merge and unchanged source data.");
    }

    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
}
