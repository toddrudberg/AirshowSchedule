using AirshowSchedules;

internal static class SilentScheduleChecks
{
    private const int Year = 2027;

    internal static async Task Run(string mode, string root, string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var site) || !site.IsLoopback)
            throw new ArgumentException("Sample checks require a loopback server.");
        Directory.CreateDirectory(root);
        string showsPath = Path.Combine(root, "sample.asg.xml"), contactsPath = Path.Combine(root, "sample.contacts.json");
        using var client = new SilentScheduleClient(url);
        var regions = new Regions { myRegions = new() { ["CO"] = "MOUNTAIN", ["WA"] = "NORTHWEST" } };

        (AirshowGroup Database, List<cContact> Contacts) Load()
        {
            var database = AirshowGroup.LoadMe(showsPath, out bool loaded);
            var contacts = cContact.LoadMe(contactsPath, out bool contactsLoaded);
            Check(loaded && contactsLoaded, "Cannot load sample files.");
            return (database, contacts);
        }

        async Task Publish(AirshowGroup database, List<cContact> contacts)
        {
            var snapshot = SilentSchedule.Build(database, contacts, regions, Year,
                SilentContactSaves.Read(contactsPath, IcasUpdateStore.Hash(contactsPath)));
            var result = await client.Publish(snapshot);
            Check(result.PublishedAt != null && result.Year == Year, "Missing publish state.");
            var retry = await client.Publish(snapshot);
            Check(retry == result, "Retrying a publication changed the revision.");
            bool failed = false;
            try { await client.Publish(snapshot with { SchemaVersion = 2, PublicationId = Guid.NewGuid().ToString() }); }
            catch (IOException) { failed = true; }
            Check(failed && await client.State(Year) == result, "Failed publication replaced the active snapshot.");
        }

        if (mode == "publish")
        {
            var database = new AirshowGroup { AirshowYearOfInterest = Year };
            database.GetAirshows().AddRange(new[]
            {
                Show(4, "Harbor Sample Airshow", "2027-06-12", "2027-06-13", "Harbor City", "WA", Airshow.eStatus.maybe),
                Show(19, "Weekday Sample Exhibit", "2027-06-17", "2027-06-17", "Mountain Town", "CO", Airshow.eStatus.contract)
            });
            List<cContact> contacts = new()
            {
                new() { ID = 7, name = "Sample Organizer", phone = "555-0100", address = "", showIDs = new() { 4, 19 },
                    emailAddresses = new() { "sample@example.test", "second@example.test" } },
                new() { ID = 8, name = "Sample Sponsor", phone = "", address = "", showIDs = new() { 4, 19 } },
                new() { ID = 12, name = "Unlinked Sample", phone = "", address = "" }
            };
            AirshowGroup.SaveMe(database, showsPath);
            cContact.SaveMe(contacts, contactsPath);
            await Publish(database, contacts);
        }
        else if (mode == "unrelated")
        {
            var local = Load();
            local.Database.GetAirshows().Single(s => s.ID == 19).Notes_AirshowStuff = "An unrelated PC edit.";
            AirshowGroup.SaveMe(local.Database, showsPath);
            await Publish(local.Database, local.Contacts);
            Check((await client.Changes(Year)).Count == 5, "Unrelated publish lost pending changes.");
        }
        else if (mode is "save-without-ack" or "retry-and-ack")
        {
            var changes = await client.Changes(Year);
            Check(changes.Select(c => c.Kind).SequenceEqual(new[] { "status", "addShow", "addContact", "editContact", "deleteContact" }),
                "Missing or misordered sample changes.");
            if (mode == "save-without-ack")
            {
                var initial = Load();
                initial.Database.GetAirshows().Single(s => s.ID == 4).Status = Airshow.eStatus.verbal;
                AirshowGroup.SaveMe(initial.Database, showsPath);
                var conflict = SilentSchedule.Inspect(initial.Database, initial.Contacts, changes[0]);
                Check(conflict.Conflict && conflict.SnapshotValue == "maybe" && conflict.LocalValue == "verbal" &&
                    conflict.ProposedValue == "contract", "Local status conflict was not flagged.");
                Check(SilentSchedule.Inspect(initial.Database, initial.Contacts, changes[2]).Blocked != null,
                    "A contact can be applied before its draft show.");
                var organizer = initial.Contacts.Single(c => c.ID == 7);
                organizer.phone = "555-0177";
                organizer.address = "PC-only address change";
                cContact.SaveMe(initial.Contacts, contactsPath);
                var edit = SilentSchedule.Inspect(initial.Database, initial.Contacts, changes[3]);
                Check(edit.Conflict && edit.LocalValue.Contains("555-0177") && edit.ProposedValue.Contains("555-0199")
                    && edit.ProposedValue.Contains("PC-only address change"), "Contact conflict or field merge was not shown.");
            }
            else
            {
                // Simulate a process ending after replacing the contact file but before finalizing the delete receipt.
                string deleteReceipt = Path.Combine(contactsPath + ".silent-web-receipts", changes[4].Id + ".receipt.json");
                string receipt = File.ReadAllText(deleteReceipt);
                Check(receipt.Contains("\"Complete\":true"), "Missing completed deletion receipt.");
                File.WriteAllText(deleteReceipt, receipt.Replace("\"Complete\":true", "\"Complete\":false"));
            }
            foreach (var change in changes)
            {
                var local = Load();
                var review = SilentSchedule.Inspect(local.Database, local.Contacts, change,
                    SilentContactSaves.Read(contactsPath, IcasUpdateStore.Hash(contactsPath)));
                if (mode == "retry-and-ack") Check(review.Saved != null, "The original save did not persist a receipt.");
                var receipt = SilentSchedule.Apply(showsPath, contactsPath, local.Database, local.Contacts,
                    IcasUpdateStore.Hash(showsPath), IcasUpdateStore.Hash(contactsPath), change);
                if (mode == "retry-and-ack")
                {
                    await client.Ack(receipt);
                    await client.Ack(receipt);
                }
            }
            var saved = Load();
            var added = saved.Database.GetAirshows().Single(s => s.name_airshow == "Sample Ridge Fly-In");
            var newContact = saved.Contacts.Single(c => c.name == "Sample Ridge Contact");
            Check(saved.Database.GetAirshows().Count == 3 && saved.Contacts.Count == 3 && newContact.showIDs.Single() == added.ID &&
                saved.Database.GetAirshows().Single(s => s.ID == 4).Status == Airshow.eStatus.contract,
                "Import did not save the sample changes exactly once.");
            var updated = saved.Contacts.Single(c => c.ID == 7);
            Check(updated.phone == "555-0199" && updated.address == "PC-only address change" &&
                updated.showIDs.SequenceEqual(new[] { 4, 19 }) && updated.emailAddresses.Count == 2 &&
                !saved.Contacts.Any(c => c.ID == 8), "Contact update lost fields or links, or deletion failed.");
        }
        else if (mode == "republish")
        {
            var saved = Load();
            await Publish(saved.Database, saved.Contacts);
            Check((await client.Changes(Year)).Count == 0, "Applied edits were still pending.");
        }
        else throw new ArgumentException("Unknown sample phase.");
        Console.WriteLine($"PASS: silent schedule {mode}");
    }

    private static Airshow Show(int id, string name, string start, string finish, string city, string state, Airshow.eStatus status) =>
        new() { ID = id, name_airshow = name, date_start = start, date_finish = finish, date_added = "2027-01-01",
            location = new Airshow.cLocation { city = city, state = state }, Status = status };

    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
}
