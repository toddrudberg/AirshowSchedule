using AirshowSchedules;
using Newtonsoft.Json;
using System.Xml.Serialization;

internal static class UpdateChecks
{
    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    private static Airshow Show(int id, string name, string start = "2026-06-01") => new()
    { ID = id, name_airshow = name, date_start = start, date_finish = start, date_added = "2025-01-01",
        location = new Airshow.cLocation { city = "Town", state = "CO" } };
    internal static void Run(List<Airshow> source, List<cContact> sourceContacts)
    {
        var show = Show(10, "Town Air Show");
        show.Status = Airshow.eStatus.contract; show.UndauntedNotes.Add("Keep note"); show.AirshowLinks.Add("Keep link");
        show.Performers.performer.Add("Old performer");
        var other = Show(20, "Other Show");
        var missing = Show(30, "Missing Show");
        var previousYear = Show(40, "Town Air Show", "2025-06-01");
        var db = new AirshowGroup { AirshowYearOfInterest = 2025 };
        db.GetAirshows().AddRange(new[] { show, other, missing, previousYear });
        var contacts = new List<cContact> { new() { ID = 5, name = "Shared Contact", phone = "111", address = "Keep address", showIDs = new() { 10, 20 } } };
        var changed = Show(0, "Town Airshow (CANCELLED)", "2026-06-08");
        changed.Performers.performer.Add("New performer");
        var newShow = Show(1, "New Show"); newShow.location.city = "New Town";
        var incoming = new List<Airshow> { changed, newShow };
        var newContacts = new List<cContact> { new() { ID = 0, name = "Shared Contact", phone = "222", address = "", showIDs = new() { 0 } } };
        var plan = new IcasUpdatePlan(db, contacts, incoming, newContacts, 2026);
        Check(plan.Changes.Count(c => c.Group == "New shows") == 1, "Date change created a duplicate show.");
        Check(!plan.Changes.Single(c => c.Group == "Contact changes").Selected, "Contact conflict selected by default.");
        Check(plan.Changes.Count(c => c.Group == "Missing from download") == 2, "Missing events not flagged.");
        foreach (var c in plan.Changes.Where(c => c.Apply != null)) c.Selected = true;
        int count = plan.ApplySelected();
        Check(count == 5, "Unexpected change count.");
        var result = plan.Database.GetAirshows().Single(s => s.ID == 10);
        Check(result.Status == Airshow.eStatus.contract && result.UndauntedNotes.Single() == "Keep note" && result.AirshowLinks.Single() == "Keep link"
            && result.date_added == "2025-01-01", "Custom fields lost.");
        Check(plan.Contacts.Single(c => c.ID == 5).phone == "111" && plan.Contacts.Single(c => c.ID == 5).showIDs.SequenceEqual(new[] { 20 }), "Shared contact was overwritten.");
        Check(plan.Database.GetAirshows().Count == 5 && db.GetAirshows().Count == 4 && contacts[0].showIDs.Count == 2, "Live state mutated before commit.");
        var repeat = new IcasUpdatePlan(plan.Database, plan.Contacts, incoming, newContacts, 2026);
        Check(!repeat.Changes.Any(c => c.Apply != null), "Repeated import proposed changes.");
        var ambiguousDb = new AirshowGroup();
        ambiguousDb.GetAirshows().AddRange(new[] { Show(1, "Same", "2026-04-01"), Show(2, "Same", "2026-05-01") });
        var ambiguous = new IcasUpdatePlan(ambiguousDb, new(), new() { Show(0, "Same", "2026-06-01") }, new(), 2026);
        Check(ambiguous.Changes.Any(c => c.Group == "Needs review") && !ambiguous.Changes.Any(c => c.Apply != null), "Ambiguous match was applied.");
        var empty = new IcasUpdatePlan(new AirshowGroup(), new(), source, sourceContacts, 2026);
        empty.ApplySelected();
        Check(empty.Database.GetAirshows().Count == source.Count && empty.Contacts.All(c => c.showIDs.All(id => empty.Database.GetAirshows().Any(s => s.ID == id))), "Empty database merge failed.");
        Check(!new IcasUpdatePlan(empty.Database, empty.Contacts, source, sourceContacts, 2026).Changes.Any(c => c.Apply != null), "Real document import is not idempotent.");
        string realDatabase = Path.GetFullPath("DataFile/2025/ActiveDB.asg.xml");
        string realContacts = Path.GetFullPath("DataFile/2025/ActiveDB.contacts.json");
        if (File.Exists(realDatabase) && File.Exists(realContacts))
        {
            string dbHash = IcasUpdateStore.Hash(realDatabase), contactHash = IcasUpdateStore.Hash(realContacts);
            AirshowGroup active;
            using (var reader = File.OpenRead(realDatabase)) active = (AirshowGroup)new XmlSerializer(typeof(AirshowGroup)).Deserialize(reader);
            var activeContacts = JsonConvert.DeserializeObject<List<cContact>>(File.ReadAllText(realContacts));
            var actual = new IcasUpdatePlan(active, activeContacts, source, sourceContacts, 2026);
            System.Console.WriteLine("Real database preview: " + string.Join(", ", actual.Changes.GroupBy(c => c.Group).Select(g => $"{g.Key}: {g.Count()}")));
            using (var preview = new FormIcasUpdate(actual))
            using (var bitmap = new System.Drawing.Bitmap(preview.Width, preview.Height))
            {
                preview.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                preview.Location = new System.Drawing.Point(-10000, -10000);
                preview.ShowInTaskbar = false;
                preview.Show();
                System.Windows.Forms.Application.DoEvents();
                preview.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, preview.Width, preview.Height));
                Directory.CreateDirectory("artifacts");
                bitmap.Save("artifacts/icas-review.png");
            }
            foreach (var c in actual.Changes.Where(c => c.Apply != null)) c.Selected = true;
            actual.ApplySelected();
            Check(!new IcasUpdatePlan(actual.Database, actual.Contacts, source, sourceContacts, 2026).Changes.Any(c => c.Apply != null), "Real database repeat import proposed applied changes.");
            Check(IcasUpdateStore.Hash(realDatabase) == dbHash && IcasUpdateStore.Hash(realContacts) == contactHash, "Active database files changed in test.");
        }
        using (var review = new FormIcasUpdate(repeat))
        {
            review.CreateControl();
            Check(review.Controls.OfType<System.Windows.Forms.DataGridView>().Single().Rows.Count == repeat.Changes.Count, "Review rows missing.");
        }
        string folder = Path.Combine(Path.GetTempPath(), "IcasUpdateChecks-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        try
        {
            string dbPath = Path.Combine(folder, "active.asg.xml"), contactsPath = Path.Combine(folder, "active.contacts.json"), docPath = Path.Combine(folder, "source.doc");
            using (var writer = File.Create(dbPath)) new XmlSerializer(typeof(AirshowGroup)).Serialize(writer, db);
            File.WriteAllText(contactsPath, JsonConvert.SerializeObject(contacts)); File.WriteAllText(docPath, "test source");
            string beforeDb = IcasUpdateStore.Hash(dbPath), beforeContacts = IcasUpdateStore.Hash(contactsPath);
            IcasUpdateStore.Commit(dbPath, contactsPath, docPath, plan, beforeDb, beforeContacts, IcasUpdateStore.Hash(docPath));
            Check(IcasUpdateStore.History(dbPath, contactsPath).Single().Status == "Applied", "History missing.");
            string afterDb = File.ReadAllText(dbPath);
            File.AppendAllText(dbPath, " ");
            try { IcasUpdateStore.Undo(dbPath, contactsPath); throw new Exception("Undo erased later edits."); } catch (IOException) { }
            File.WriteAllText(dbPath, afterDb);
            IcasUpdateStore.Undo(dbPath, contactsPath);
            Check(IcasUpdateStore.Hash(dbPath) == beforeDb && IcasUpdateStore.Hash(contactsPath) == beforeContacts, "Undo did not restore both files exactly.");
            try { IcasUpdateStore.Commit(dbPath, contactsPath, docPath, plan, "stale", beforeContacts, IcasUpdateStore.Hash(docPath)); throw new Exception("Stale review committed."); } catch (IOException) { }
            Check(IcasUpdateStore.History(dbPath, contactsPath).Single().Status == "Undone", "Undo history incorrect.");
            IcasUpdateStore.Commit(dbPath, contactsPath, docPath, plan, beforeDb, beforeContacts, IcasUpdateStore.Hash(docPath));
            var interrupted = IcasUpdateStore.History(dbPath, contactsPath).First();
            interrupted.Status = "Pending";
            File.WriteAllText(Path.Combine(interrupted.Folder, "update.json"), JsonConvert.SerializeObject(interrupted));
            File.Copy(Path.Combine(interrupted.Folder, "before.asg.xml"), dbPath, true);
            IcasUpdateStore.Undo(dbPath, contactsPath);
            Check(IcasUpdateStore.Hash(dbPath) == beforeDb && IcasUpdateStore.Hash(contactsPath) == beforeContacts, "Interrupted update recovery failed.");
        }
        finally
        {
            Check(Path.GetFullPath(folder).StartsWith(Path.GetFullPath(Path.GetTempPath()), StringComparison.OrdinalIgnoreCase), "Unexpected test cleanup path.");
            Directory.Delete(folder, true);
        }
        System.Console.WriteLine("PASS: date-change matching, field selections, shared contacts, custom fields, missing shows, ambiguity, repeat imports, empty database, review form, history, paired undo and stale-write protection.");
    }
}
