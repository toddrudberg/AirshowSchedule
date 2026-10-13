using System.Security.Cryptography;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AirshowSchedules;

internal sealed class IcasUpdateRecord
{
    public string DatabasePath = "";
    public string ContactsPath = "";
    public string SourceName = "";
    public string SourceHash = "";
    public string BeforeDatabaseHash = "";
    public string BeforeContactsHash = "";
    public string AfterDatabaseHash = "";
    public string AfterContactsHash = "";
    public DateTime Created;
    public int Year;
    public int Changes;
    public string Status = "Pending";
    public List<string> Details = new();
    [JsonIgnore] public string Folder = "";
}

internal static class IcasUpdateStore
{
    internal static string Hash(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path)));
    private static string Root(string database) => Path.GetFullPath(database) + ".icas-history";
    private static void SaveRecord(IcasUpdateRecord record)
    {
        string path = Path.Combine(record.Folder, "update.json");
        File.WriteAllText(path + ".tmp", JsonConvert.SerializeObject(record, Formatting.Indented));
        File.Move(path + ".tmp", path, true);
    }
    internal static List<IcasUpdateRecord> History(string database, string contacts)
    {
        if (!Directory.Exists(Root(database))) return new();
        return Directory.GetFiles(Root(database), "update.json", SearchOption.AllDirectories).Select(path =>
        {
            var record = JsonConvert.DeserializeObject<IcasUpdateRecord>(File.ReadAllText(path))
                ?? throw new IOException($"Unreadable update history: {path}");
            record.Folder = Path.GetDirectoryName(path)!;
            return record;
        }).Where(r => string.Equals(r.ContactsPath, Path.GetFullPath(contacts), StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.Created).ToList();
    }
    internal static void Commit(string database, string contacts, string source, IcasUpdatePlan plan, string databaseHash, string contactsHash, string sourceHash)
    {
        database = Path.GetFullPath(database);
        contacts = Path.GetFullPath(contacts);
        if (database.Equals(contacts, StringComparison.OrdinalIgnoreCase)) throw new IOException("Show and contact files must be different.");
        if (Hash(database) != databaseHash || Hash(contacts) != contactsHash)
            throw new IOException("The database changed during review. Reload it and review the import again.");
        if (Hash(source) != sourceHash) throw new IOException("The ICAS download changed during review. Choose it again.");
        if (History(database, contacts).Any(r => r.Status == "Pending"))
            throw new IOException("An interrupted update needs recovery. Use Undo last ICAS update first.");
        var record = new IcasUpdateRecord { DatabasePath = database, ContactsPath = contacts,
            SourceName = Path.GetFileName(source), SourceHash = sourceHash, Created = DateTime.Now, Year = plan.Year,
            BeforeDatabaseHash = databaseHash, BeforeContactsHash = contactsHash,
            Changes = plan.Changes.Count(c => c.Selected && c.Apply != null),
            Details = plan.Changes.Where(c => c.Selected && c.Apply != null).Select(c => $"{c.Show}: {c.Field}\n{c.Before}\n→ {c.After}").ToList(),
            Folder = Path.Combine(Root(database), DateTime.Now.ToString("yyyyMMdd-HHmmss-fff") + "-" + Guid.NewGuid().ToString("N")) };
        Directory.CreateDirectory(record.Folder);
        File.Copy(database, Path.Combine(record.Folder, "before.asg.xml"));
        File.Copy(contacts, Path.Combine(record.Folder, "before.contacts.json"));
        File.Copy(source, Path.Combine(record.Folder, "source" + Path.GetExtension(source)));
        if (Hash(Path.Combine(record.Folder, "source" + Path.GetExtension(source))) != sourceHash)
            throw new IOException("The ICAS download changed while being archived. No database updates were made.");
        string nextDatabase = Path.Combine(record.Folder, "after.asg.xml");
        string nextContacts = Path.Combine(record.Folder, "after.contacts.json");
        using (var writer = File.Create(nextDatabase)) new XmlSerializer(typeof(AirshowGroup)).Serialize(writer, plan.Database);
        File.WriteAllText(nextContacts, JsonConvert.SerializeObject(plan.Contacts, Formatting.Indented));
        record.AfterDatabaseHash = Hash(nextDatabase);
        record.AfterContactsHash = Hash(nextContacts);
        SaveRecord(record);
        try
        {
            Replace(nextDatabase, database);
            Replace(nextContacts, contacts);
            record.Status = "Applied";
            SaveRecord(record);
        }
        catch
        {
            Replace(Path.Combine(record.Folder, "before.asg.xml"), database);
            Replace(Path.Combine(record.Folder, "before.contacts.json"), contacts);
            record.Status = "Failed (restored)";
            SaveRecord(record);
            throw;
        }
    }
    private static void Replace(string source, string target)
    {
        string temp = target + ".icas-" + Guid.NewGuid().ToString("N") + ".tmp";
        try { File.Copy(source, temp); File.Move(temp, target, true); }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }
    internal static void Undo(string database, string contacts)
    {
        var record = History(database, contacts).FirstOrDefault()
            ?? throw new IOException("There is no ICAS update to undo.");
        if (record.Status != "Applied" && record.Status != "Pending") throw new IOException("The last update has already been undone or failed.");
        string dbHash = Hash(database), contactHash = Hash(contacts);
        bool pending = record.Status == "Pending";
        if ((dbHash != record.AfterDatabaseHash && !(pending && dbHash == record.BeforeDatabaseHash)) ||
            (contactHash != record.AfterContactsHash && !(pending && contactHash == record.BeforeContactsHash)))
            throw new IOException("Changes were saved after this update. Undo would overwrite them. The backup is available in ICAS update history.");
        if (Hash(Path.Combine(record.Folder, "before.asg.xml")) != record.BeforeDatabaseHash ||
            Hash(Path.Combine(record.Folder, "before.contacts.json")) != record.BeforeContactsHash)
            throw new IOException("Backup integrity check failed.");
        // Mark recovery pending before touching either file, allowing an interrupted undo to resume.
        record.Status = "Pending";
        SaveRecord(record);
        Replace(Path.Combine(record.Folder, "before.asg.xml"), database);
        Replace(Path.Combine(record.Folder, "before.contacts.json"), contacts);
        record.Status = "Undone";
        SaveRecord(record);
    }
}
