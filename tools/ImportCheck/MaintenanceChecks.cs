using AirshowSchedules;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml.Serialization;

internal static class MaintenanceChecks
{
    private static Airshow Show(int id, string name, string date, string city = "Town") => new()
    {
        ID = id, name_airshow = name, date_start = date, date_finish = date, date_added = date,
        location = new Airshow.cLocation { city = city, state = "CO" }
    };
    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    internal static void Run()
    {
        var active = new AirshowGroup { AirshowYearOfInterest = 2026 };
        active.GetAirshows().AddRange(new[] { Show(10, "Keep", "2026-06-06"), Show(11, "Possible duplicate", "2026-06-13"),
            Show(20, "Booked", "2026-07-04", "Booked town"), Show(30, "Missing", "2026-08-01", "Missing town"),
            Show(40, "Prior year", "2025-06-07"), Show(50, "Ambiguous", "2026-09-05", "Ambiguous town") });
        active.GetAirshows().Single(s => s.ID == 20).Status = Airshow.eStatus.contract;
        var source = new AirshowGroup { AirshowYearOfInterest = 2026 };
        source.GetAirshows().AddRange(new[] { Show(1, "Keep", "2026-06-06"), Show(2, "Booked (CANCELLED)", "2026-07-04", "Booked town"),
            Show(3, "Ambiguous A", "2026-09-05", "Ambiguous town"), Show(4, "Ambiguous B", "2026-09-05", "Ambiguous town") });
        source.GetAirshows()[0].UndauntedNotes.Add("Merge note");
        var contacts = new List<cContact> { new() { ID = 99, name = "Existing", showIDs = new() { 10, 11, 20, 30, 40, 50 } } };
        var sourceContacts = new List<cContact> { new() { ID = 0, name = "New", showIDs = new() { 1 } } };
        string original = JsonConvert.SerializeObject(new { active, contacts, source, sourceContacts });
        var session = new DatabaseMaintenance(active, contacts, source, sourceContacts);
        var cleanup = session.CleanupChoices();
        Check(cleanup.Select(c => c.Show.ID).SequenceEqual(new[] { 10, 11, 30 }), "Cleanup candidates or chronological order incorrect");
        Check(cleanup.All(c => !c.Selected && !c.Suggested), "Deletion was preselected");
        Check(cleanup.Single(c => c.Show.ID == 10).Reason.Contains("Name and dates match latest calendar"), "Retained entry match evidence missing");
        Check(cleanup.Single(c => c.Show.ID == 11).Reason.Contains("Dates differ from latest calendar") && cleanup.Single(c => c.Show.ID == 11).ComparisonShows.Single().ID == 1,
            "Incorrect-date candidate lacks latest calendar evidence");
        Check(cleanup.Single(c => c.Show.ID == 30).ComparisonShows.Count == 0, "Missing-location candidate shows unrelated comparison");
        Check(session.RelatedActiveShows(cleanup[0].Show).Select(s => s.ID).SequenceEqual(new[] { 10, 11 }), "Related active entries include another year or location");
        using (var review = new FormDatabaseMaintenance(session))
        {
            var grid = (System.Windows.Forms.DataGridView)typeof(FormDatabaseMaintenance).GetField("grid", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(review)!;
            Check(grid.Columns[3].Visible && grid.Rows[1].Cells[3].Value.ToString().Contains("2026-06-06"), "Cleanup hides source dates");
            var details = (System.Windows.Forms.TextBox)typeof(FormDatabaseMaintenance).GetField("details", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(review)!;
            Check(details.Text.Contains("Possible duplicate") && details.Text.Contains("Latest calendar entries"), "Initial details omit related active or latest entries");
        }
        var legitimateSource = active.DeepCopy();
        var legitimate = new DatabaseMaintenance(active, contacts, legitimateSource, contacts).CleanupChoices();
        Check(legitimate.Where(c => c.Show.ID is 10 or 11).All(c => c.Reason.Contains("Name and dates match latest calendar") && c.ComparisonShows.Count == 2 && !c.Suggested),
            "Two legitimate location-sharing events incorrectly favor one entry");
        var renamedSource = source.DeepCopy();
        renamedSource.GetAirshows()[0].name_airshow = "Renamed event";
        Check(new DatabaseMaintenance(active, contacts, renamedSource, sourceContacts).CleanupChoices().Single(c => c.Show.ID == 10).Reason.Contains("Dates match latest calendar; name differs"),
            "Date-only match mislabeled as name match");
        foreach (var choice in cleanup) choice.Selected = choice.Show.ID is 11 or 30;
        session.RemoveSelected(cleanup);
        Check(session.Contacts[0].showIDs.SequenceEqual(new[] { 10, 20, 40, 50 }), "Deleted show links remain");
        var cancellations = session.CancellationChoices();
        Check(cancellations.Single().Show.ID == 20 && !cancellations.Single().Selected, "Incoming cancellation not surfaced safely");
        session.RemoveSelected(cancellations);
        Check(session.Database.GetAirshows().Single(s => s.ID == 20).Status == Airshow.eStatus.contract, "Cancellation changed booking status");
        var cancellationSession = new DatabaseMaintenance(active, contacts, source, sourceContacts);
        var removal = cancellationSession.CancellationChoices();
        removal.Single().Selected = true;
        cancellationSession.RemoveSelected(removal);
        Check(!cancellationSession.Database.GetAirshows().Any(s => s.ID == 20) && !cancellationSession.Contacts[0].showIDs.Contains(20), "Cancellation left a show or contact link");
        var merges = session.MergeChoices();
        Check(merges.Count(c => c.Show.ID == 50) == 2 && merges.Where(c => c.Show.ID == 50).All(c => !c.Suggested), "Ambiguous merge preselected");
        foreach (var choice in merges) choice.Selected = true;
        string beforeAmbiguous = JsonConvert.SerializeObject(session.Database);
        try { session.MergeSelected(merges); throw new Exception("Multiple merge choices accepted"); }
        catch (InvalidOperationException) { }
        Check(beforeAmbiguous == JsonConvert.SerializeObject(session.Database), "Invalid merge partially modified session");
        foreach (var choice in merges) choice.Selected = choice.Suggested;
        session.MergeSelected(merges);
        Check(session.Database.GetAirshows().Single(s => s.ID == 10).UndauntedNotes.Contains("Merge note"), "Additional fields not merged");
        Check(original == JsonConvert.SerializeObject(new { active, contacts, source, sourceContacts }), "Staging changed source or active data");

        // All three steps still advance when no cleanup or cancellation candidates exist.
        var cleanActive = source.DeepCopy();
        cleanActive.GetAirshows().RemoveAll(s => s.ID != 1);
        var clean = new DatabaseMaintenance(cleanActive, sourceContacts, cleanActive, sourceContacts);
        Check(clean.CleanupChoices().Count == 0 && clean.CancellationChoices().Count == 0, "Empty-stage fixture invalid");
        using (var form = new FormDatabaseMaintenance(clean))
        {
            var advance = typeof(FormDatabaseMaintenance).GetMethod("Advance", BindingFlags.NonPublic | BindingFlags.Instance)!;
            for (int i = 0; i < 3; i++) advance.Invoke(form, new object[] { true });
            Check((int)typeof(FormDatabaseMaintenance).GetField("step", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(form)! == 3, "Empty step stopped workflow");
            using var bitmap = new System.Drawing.Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }

        string folder = Path.Combine(Path.GetTempPath(), "AirshowMaintenanceCheck-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        try
        {
            string db = Path.Combine(folder, "active.asg.xml"), json = Path.Combine(folder, "active.contacts.json");
            using (var writer = File.Create(db)) new XmlSerializer(typeof(AirshowGroup)).Serialize(writer, active);
            File.WriteAllText(json, JsonConvert.SerializeObject(contacts));
            string dbBefore = File.ReadAllText(db), contactsBefore = File.ReadAllText(json);
            string dbHash = IcasUpdateStore.Hash(db), contactHash = IcasUpdateStore.Hash(json);
            string backup = DatabaseMaintenanceStore.Commit(db, json, session, dbHash, contactHash);
            Check(File.ReadAllText(db) != dbBefore && File.ReadAllText(json) != contactsBefore, "Paired cleanup not saved");
            string edited = File.ReadAllText(json) + " ";
            File.WriteAllText(json, edited);
            try { DatabaseMaintenanceStore.Undo(db, json); throw new Exception("Undo overwrote later edits"); } catch (IOException) { }
            File.WriteAllText(json, edited[..^1]);
            DatabaseMaintenanceStore.Undo(db, json);
            Check(File.ReadAllText(db) == dbBefore && File.ReadAllText(json) == contactsBefore, "Undo did not restore exact originals");
            backup = DatabaseMaintenanceStore.Commit(db, json, session, dbHash, contactHash);
            // Simulate an interrupted paired write: show file still old, contacts already new.
            File.WriteAllText(db, dbBefore);
            string manifest = Path.Combine(backup, "cleanup.json");
            var pending = JsonConvert.DeserializeObject<MaintenanceRecord>(File.ReadAllText(manifest))!;
            pending.Status = "Pending";
            File.WriteAllText(manifest, JsonConvert.SerializeObject(pending));
            DatabaseMaintenanceStore.Undo(db, json);
            Check(File.ReadAllText(db) == dbBefore && File.ReadAllText(json) == contactsBefore, "Interrupted cleanup recovery failed");
            File.WriteAllText(json, contactsBefore + " ");
            try { DatabaseMaintenanceStore.Commit(db, json, session, dbHash, contactHash); throw new Exception("Stale cleanup accepted"); } catch (IOException) { }
            Check(File.ReadAllText(db) == dbBefore, "Stale commit modified database");
        }
        finally { Directory.Delete(folder, true); }
        System.Console.WriteLine("PASS: cleanup comparison dates/names, related entries, legitimate shared locations, staged sequence, unchecked removals, year scope, ambiguity, empty steps, paired backups/undo and stale-write protection.");
    }
}
