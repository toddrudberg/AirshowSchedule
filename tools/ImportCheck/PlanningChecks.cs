using AirshowSchedules;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

internal static class PlanningChecks
{
    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    private static Airshow Show(int id, string name, string start, string finish, Airshow.eStatus status = Airshow.eStatus.none) => new()
    {
        ID = id, name_airshow = name, date_start = start, date_finish = finish, date_added = start, Status = status,
        location = new Airshow.cLocation { city = "Town", state = "CO" }
    };
    internal static void Run()
    {
        var sunday = Show(1, "Sunday", "2026-06-07", "2026-06-07");
        var weekday = Show(2, "Midweek contract", "2026-06-03", "2026-06-05", Airshow.eStatus.contract);
        var spanning = Show(3, "Two weekends", "2026-06-06", "2026-06-14", Airshow.eStatus.verbal);
        var monday = Show(4, "Next Monday", "2026-06-08", "2026-06-08");
        var crossYear = Show(5, "Cross year", "2025-12-31", "2026-01-02");
        var saturday = new DateTime(2026, 6, 6);
        var all = new List<Airshow> { sunday, weekday, spanning, monday, crossYear };
        Check(CalendarPlanning.Weekend(all, saturday).Select(s => s.ID).SequenceEqual(new[] { 1, 3 }), "Weekend matching included a weekday or missed Sunday.");
        Check(CalendarPlanning.Weekend(all, saturday.AddDays(7)).Single().ID == 3, "Multiweek event missed second weekend.");
        Check(CalendarPlanning.NearbyWeekdays(all, saturday).Single().ID == 2, "Weekday classification incorrect.");
        Check(CalendarPlanning.WeekdayOnly(weekday) && !CalendarPlanning.WeekdayOnly(sunday) && !CalendarPlanning.WeekdayOnly(spanning), "Weekday-only detection incorrect.");
        Check(CalendarPlanning.ForYear(all, 2026).Any(s => s.ID == 5), "Cross-year event excluded.");
        Check(!CalendarPlanning.Overlap(weekday, spanning) && CalendarPlanning.Overlap(sunday, spanning), "Overlap test used weeks rather than dates.");
        var cancelled = Show(6, "Cancelled (CANCELLED)", "2026-06-06", "2026-06-07", Airshow.eStatus.contract);
        var pursue = Show(7, "Prospect", "2026-06-20", "2026-06-21", Airshow.eStatus.pursue);
        var past = Show(8, "Past cancelled (CANCELLED)", "2026-01-01", "2026-01-01");
        var db = new AirshowGroup { AirshowYearOfInterest = 2026 };
        db.GetAirshows().AddRange(all.Concat(new[] { cancelled, pursue, past }));
        var contacts = new List<cContact> { new() { ID = 1, name = "Organizer", phone = "555", showIDs = new() { 7 } } };
        var import = new AttentionImport { Description = "Test download", Proposed = new()
        {
            new IcasChange { TargetId = 7, Group = "Missing from download" },
            new IcasChange { TargetId = 3, Field = "Dates", Before = "June 6–14", After = "June 13–21" }
        } };
        var issues = AttentionPlanning.Build(db, contacts, 2026, new DateTime(2026, 6, 1), false, import, new HashSet<string>());
        Check(issues.Any(i => i.Show.ID == 6 && i.Category == "Cancellation warning" && i.Priority == 0), "Booked cancellation missing.");
        Check(issues.Any(i => i.Show.ID == 3 && i.Category == "Booked dates changed"), "Pending booked date change missing.");
        Check(issues.Any(i => i.Show.ID == 3 && i.Category == "Overlapping commitments"), "Booking conflict missing.");
        Check(!issues.Any(i => i.Show.ID == 2 && i.Category == "Overlapping commitments"), "Weekday caused false weekend conflict.");
        Check(issues.Any(i => i.Show.ID == 7 && i.Category == "Pursue next step") && issues.Any(i => i.Show.ID == 7 && i.Category == "Missing from ICAS"), "Prospect or missing show not flagged.");
        Check(!issues.Any(i => i.Show.ID == 7 && i.Category == "Missing contact details"), "Usable contact flagged as missing.");
        Check(!issues.Any(i => i.Show.ID == 8), "Past shows included by default.");
        var key = issues.First(i => i.Show.ID == 6).Key;
        var reviewed = new HashSet<string> { key };
        Check(AttentionPlanning.Build(db, contacts, 2026, new DateTime(2026, 6, 1), false, import, reviewed).Single(i => i.Key == key).Reviewed, "Review acknowledgment lost.");
        cancelled.Status = Airshow.eStatus.verbal;
        Check(!AttentionPlanning.Build(db, contacts, 2026, new DateTime(2026, 6, 1), false, import, reviewed).Single(i => i.Show.ID == 6 && i.Category == "Cancellation warning").Reviewed, "Changed warning remained dismissed.");
        Check(AttentionPlanning.Build(db, contacts, 2026, new DateTime(2026, 6, 1), true, import, reviewed).Any(i => i.Show.ID == 8), "Past-show toggle failed.");
        var yearEnd = Show(9, "Year-end booking", "2026-12-31", "2027-01-01", Airshow.eStatus.contract);
        var nextYear = Show(10, "New-year booking", "2027-01-01", "2027-01-01", Airshow.eStatus.verbal);
        db.GetAirshows().AddRange(new[] { yearEnd, nextYear });
        Check(AttentionPlanning.Build(db, contacts, 2026, new DateTime(2026, 6, 1), false, import, reviewed).Any(i => i.Show.ID == 9 && i.Category == "Overlapping commitments"), "Booking across year boundary not detected.");
        using (var attention = new FormNeedsAttention(2026, includePast => AttentionPlanning.Build(db, contacts, 2026, new DateTime(2026, 6, 1), includePast, import, reviewed), _ => { }, _ => { }, "Test calendar"))
        {
            Capture(attention, "artifacts/attention-preview.png");
            var grid = attention.Controls.OfType<DataGridView>().Single();
            Check(grid.Rows.Count > 0, "Attention grid empty.");
        }
        CheckMainCalendar(db, contacts);
        CheckHistoryAndReviews();
        System.Console.WriteLine("PASS: Sunday, weekday-only, multiweek and cross-year calendar dates; booking conflicts, cancellations, contacts, date changes, past filtering, reviewed warnings and rendered planning forms.");
    }

    private static void CheckHistoryAndReviews()
    {
        string folder = Path.Combine(Path.GetTempPath(), "IcasPlanningChecks-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        try
        {
            string dbPath = Path.Combine(folder, "active.asg.xml"), contactsPath = Path.Combine(folder, "contacts.json"), docPath = Path.Combine(folder, "calendar.doc");
            var db = new AirshowGroup { AirshowYearOfInterest = 2026 };
            db.GetAirshows().Add(Show(60, "Booked Show", "2026-06-06", "2026-06-07", Airshow.eStatus.contract));
            using (var writer = File.Create(dbPath)) new XmlSerializer(typeof(AirshowGroup)).Serialize(writer, db);
            File.WriteAllText(contactsPath, "[]");
            File.WriteAllText(docPath, "<table><tr><td>Air Show Dates</td><td>Name</td><td>Location</td><td>Performers</td><td>Contact</td></tr><tr><td>2026-06-13<br>2026-06-14</td><td>Booked Show</td><td>Town, CO</td><td></td><td></td></tr></table>");
            var parsed = IcasHtmlImporter.Parse(File.ReadAllText(docPath), 2026);
            var plan = new IcasUpdatePlan(db, new(), parsed.Shows, parsed.Contacts, 2026);
            plan.ApplySelected();
            IcasUpdateStore.Commit(dbPath, contactsPath, docPath, plan, IcasUpdateStore.Hash(dbPath), IcasUpdateStore.Hash(contactsPath), IcasUpdateStore.Hash(docPath));
            var firstImport = AttentionImport.Load(dbPath, contactsPath, plan.Database, plan.Contacts, 2026);
            var firstIssue = AttentionPlanning.Build(plan.Database, plan.Contacts, 2026, new DateTime(2026, 6, 1), false, firstImport, new HashSet<string>()).Single(i => i.Category == "Booked dates changed");
            var reviewed = new HashSet<string> { firstIssue.Key };
            AttentionReviewStore.Save(dbPath, contactsPath, reviewed);
            Check(AttentionReviewStore.Load(dbPath, contactsPath).SetEquals(reviewed), "Review status not persisted.");
            Check(AttentionReviewStore.Load(dbPath, Path.Combine(folder, "other-contacts.json")).Count == 0, "Reviews leaked into another contact database.");
            // A later performer-only update must not lose an unresolved date warning or its review key.
            File.WriteAllText(docPath, File.ReadAllText(docPath).Replace("<td></td><td></td>", "<td>- Performer<br></td><td></td>"));
            parsed = IcasHtmlImporter.Parse(File.ReadAllText(docPath), 2026);
            var later = new IcasUpdatePlan(plan.Database, plan.Contacts, parsed.Shows, parsed.Contacts, 2026);
            later.ApplySelected();
            IcasUpdateStore.Commit(dbPath, contactsPath, docPath, later, IcasUpdateStore.Hash(dbPath), IcasUpdateStore.Hash(contactsPath), IcasUpdateStore.Hash(docPath));
            var laterImport = AttentionImport.Load(dbPath, contactsPath, later.Database, later.Contacts, 2026);
            var laterIssue = AttentionPlanning.Build(later.Database, later.Contacts, 2026, new DateTime(2026, 6, 1), false, laterImport, AttentionReviewStore.Load(dbPath, contactsPath)).Single(i => i.Category == "Booked dates changed");
            Check(laterIssue.Key == firstIssue.Key && laterIssue.Reviewed, "Later import lost a prior date warning or its review status.");
            IcasUpdateStore.Undo(dbPath, contactsPath);
            Check(AttentionImport.Load(dbPath, contactsPath, plan.Database, plan.Contacts, 2026).AppliedDateChanges.ContainsKey(60), "Undo lost prior applied date change.");
        }
        finally
        {
            Check(Path.GetFullPath(folder).StartsWith(Path.GetFullPath(Path.GetTempPath()), StringComparison.OrdinalIgnoreCase), "Unexpected test cleanup path.");
            Directory.Delete(folder, true);
        }
    }

    private static void Capture(Form form, string path)
    {
        form.StartPosition = FormStartPosition.Manual; form.Location = new System.Drawing.Point(-10000, -10000); form.ShowInTaskbar = false;
        form.Show(); Application.DoEvents();
        Directory.CreateDirectory("artifacts");
        using var bitmap = new System.Drawing.Bitmap(form.Width, form.Height);
        form.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, form.Width, form.Height)); bitmap.Save(path);
    }
    private static void CheckMainCalendar(AirshowGroup fixture, List<cContact> fixtureContacts)
    {
        using var main = new formMain();
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        object Get(string field) => typeof(formMain).GetField(field, flags)!.GetValue(main)!;
        void Set(string field, object value) => typeof(formMain).GetField(field, flags)!.SetValue(main, value);
        void Invoke(string method, params object[] args) => typeof(formMain).GetMethod(method, flags)!.Invoke(main, args);
        // Rendering must not run startup, which reads user settings and creates an archive.
        main.Load -= (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), main, typeof(formMain).GetMethod("frmAirshowScheduleTool_Load", flags)!);
        main.Shown -= (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), main, typeof(formMain).GetMethod("frmAirshowScheduleTool_Shown", flags)!);
        Set("myAirshowGroup", fixture); Set("myContacts", fixtureContacts); Set("myFilteredAirshows", fixture.GetAirshowsForYear());
        Invoke("LoadGrid", 2026); Invoke("ColorGrid", fixture.GetAirshowsForYear());
        var calendar = (DataGridView)Get("dgvCalendar");
        var june6 = calendar.Rows.Cast<DataGridViewRow>().SelectMany(r => r.Cells.Cast<DataGridViewCell>()).Single(c => c.Tag is DateTime d && d == new DateTime(2026, 6, 6));
        Check(june6.Value.ToString()!.Contains("3 shows"), "Weekend count incorrect.");
        var june20 = calendar.Rows.Cast<DataGridViewRow>().SelectMany(r => r.Cells.Cast<DataGridViewCell>()).Single(c => c.Tag is DateTime d && d == new DateTime(2026, 6, 20));
        Check(june20.Style.BackColor == System.Drawing.Color.LightCoral, "Pursue color missing.");
        var weekdayDb = new AirshowGroup { AirshowYearOfInterest = 2026 };
        weekdayDb.GetAirshows().Add(Show(50, "Weekday contract", "2026-06-03", "2026-06-05", Airshow.eStatus.contract));
        Set("myAirshowGroup", weekdayDb); Invoke("ColorGrid", weekdayDb.GetAirshowsForYear());
        Check(june6.Style.BackColor == System.Drawing.Color.White && june6.Value.ToString()!.Contains("Weekday only: 1"), "Weekday contract blocked weekend.");
        Invoke("LoadGrid", 2023);
        Check(calendar.Rows[0].Cells[0].Tag is DateTime jan1Weekend && jan1Weekend == new DateTime(2022, 12, 31), "January 1 Sunday has no weekend cell.");
        string dbPath = "DataFile/2025/ActiveDB.asg.xml", contactsPath = "DataFile/2025/ActiveDB.contacts.json";
        if (File.Exists(dbPath) && File.Exists(contactsPath))
        {
            string beforeDb = IcasUpdateStore.Hash(dbPath), beforeContacts = IcasUpdateStore.Hash(contactsPath);
            AirshowGroup real;
            using (var reader = File.OpenRead(dbPath)) real = (AirshowGroup)new XmlSerializer(typeof(AirshowGroup)).Deserialize(reader)!;
            real.AirshowYearOfInterest = 2026;
            var contacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cContact>>(File.ReadAllText(contactsPath))!;
            Set("myAirshowGroup", real); Set("myContacts", contacts); Set("myFilteredAirshows", real.GetAirshowsForYear());
            var regions = (CheckedListBox)Get("chklstRegions"); regions.Items.Add("All regions (preview)"); regions.SetItemChecked(0, true);
            Invoke("LoadGrid", 2026); Invoke("ColorGrid", real.GetAirshowsForYear());
            Capture(main, "artifacts/calendar-preview.png");
            Check(IcasUpdateStore.Hash(dbPath) == beforeDb && IcasUpdateStore.Hash(contactsPath) == beforeContacts, "Planning preview changed the active database.");
        }
    }
}
