using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AirshowSchedules;

internal sealed class MaintenanceRecord
{
    public string ContactsPath = "";
    public string BeforeDatabase = "";
    public string BeforeContacts = "";
    public string AfterDatabase = "";
    public string AfterContacts = "";
    public string Status = "Pending";
    public List<string> Changes = new();
}

internal static class DatabaseMaintenanceStore
{
    private static string Root(string database) => Path.GetFullPath(database) + ".cleanup-history";
    private static IEnumerable<string> Records(string database) => Directory.Exists(Root(database))
        ? Directory.GetFiles(Root(database), "cleanup.json", SearchOption.AllDirectories).OrderByDescending(p => p, StringComparer.Ordinal)
        : Enumerable.Empty<string>();
    private static MaintenanceRecord Read(string path) => JsonConvert.DeserializeObject<MaintenanceRecord>(File.ReadAllText(path))!;
    private static void Write(string path, MaintenanceRecord record)
    {
        File.WriteAllText(path + ".tmp", JsonConvert.SerializeObject(record, Formatting.Indented));
        File.Move(path + ".tmp", path, true);
    }
    private static void Replace(string source, string target)
    {
        string temp = target + ".cleanup-" + Guid.NewGuid().ToString("N") + ".tmp";
        try { File.Copy(source, temp); File.Move(temp, target, true); }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }

    internal static string Commit(string database, string contacts, DatabaseMaintenance session, string databaseHash, string contactsHash)
    {
        contacts = Path.GetFullPath(contacts);
        if (Path.GetFullPath(database).Equals(contacts, StringComparison.OrdinalIgnoreCase)) throw new IOException("Show and contact files must be different.");
        if (IcasUpdateStore.Hash(database) != databaseHash || IcasUpdateStore.Hash(contacts) != contactsHash)
            throw new IOException("An active database changed during review. Start cleanup again to use the latest files.");
        if (Records(database).Any(p => Read(p).Status == "Pending"))
            throw new IOException("An interrupted cleanup needs recovery. Choose Undo last cleanup first.");
        string folder = Path.Combine(Root(database), DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fffffff") + "-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        string beforeDb = Path.Combine(folder, "before.asg.xml"), beforeContacts = Path.Combine(folder, "before.contacts.json");
        string afterDb = Path.Combine(folder, "after.asg.xml"), afterContacts = Path.Combine(folder, "after.contacts.json");
        File.Copy(database, beforeDb);
        File.Copy(contacts, beforeContacts);
        using (var writer = File.Create(afterDb)) new XmlSerializer(typeof(AirshowGroup)).Serialize(writer, session.Database);
        File.WriteAllText(afterContacts, JsonConvert.SerializeObject(session.Contacts, Formatting.Indented));
        if (IcasUpdateStore.Hash(beforeDb) != databaseHash || IcasUpdateStore.Hash(beforeContacts) != contactsHash ||
            IcasUpdateStore.Hash(database) != databaseHash || IcasUpdateStore.Hash(contacts) != contactsHash)
            throw new IOException("An active database changed while preparing the backup. Start cleanup again.");
        var record = new MaintenanceRecord { ContactsPath = contacts, BeforeDatabase = databaseHash, BeforeContacts = contactsHash,
            AfterDatabase = IcasUpdateStore.Hash(afterDb), AfterContacts = IcasUpdateStore.Hash(afterContacts), Changes = session.Changes.ToList() };
        string manifest = Path.Combine(folder, "cleanup.json");
        Write(manifest, record);
        try
        {
            Replace(afterDb, database);
            Replace(afterContacts, contacts);
            record.Status = "Applied";
            Write(manifest, record);
        }
        catch
        {
            Replace(beforeDb, database);
            Replace(beforeContacts, contacts);
            record.Status = "Failed (restored)";
            Write(manifest, record);
            throw;
        }
        return folder;
    }

    internal static void Undo(string database, string contacts)
    {
        string manifest = Records(database).FirstOrDefault(p => Read(p).ContactsPath.Equals(Path.GetFullPath(contacts), StringComparison.OrdinalIgnoreCase))
            ?? throw new IOException("No cleanup backup exists for these active databases.");
        var record = Read(manifest);
        if (record.Status != "Applied" && record.Status != "Pending") throw new IOException("The last cleanup is already restored.");
        string dbHash = IcasUpdateStore.Hash(database), contactHash = IcasUpdateStore.Hash(contacts);
        if ((dbHash != record.AfterDatabase && !(record.Status == "Pending" && dbHash == record.BeforeDatabase)) ||
            (contactHash != record.AfterContacts && !(record.Status == "Pending" && contactHash == record.BeforeContacts)))
            throw new IOException("The databases have changed since cleanup. Undo would overwrite later edits.");
        string folder = Path.GetDirectoryName(manifest)!;
        string dbBackup = Path.Combine(folder, "before.asg.xml"), contactBackup = Path.Combine(folder, "before.contacts.json");
        if (IcasUpdateStore.Hash(dbBackup) != record.BeforeDatabase || IcasUpdateStore.Hash(contactBackup) != record.BeforeContacts)
            throw new IOException("Cleanup backup integrity check failed.");
        record.Status = "Pending";
        Write(manifest, record);
        Replace(dbBackup, database);
        Replace(contactBackup, contacts);
        record.Status = "Restored";
        Write(manifest, record);
    }
}
