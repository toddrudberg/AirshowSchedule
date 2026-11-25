using System.Text.Json;

namespace AirshowSchedules;

// Keep contact receipts beside the existing JSON list. A deleted contact cannot carry its own receipt.
internal static class SilentContactSaves
{
    private sealed record SaveRecord(string BeforeHash, string AfterHash, bool Complete, List<SilentScheduleReceipt> Receipts);

    private static string DirectoryFor(string contacts) => Path.GetFullPath(contacts) + ".silent-web-receipts";
    private static FileStream Lock(string directory) =>
        File.Open(Path.Combine(directory, "import.lock"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

    internal static List<SilentScheduleReceipt> Read(string contactsPath, string currentHash)
    {
        string directory = DirectoryFor(contactsPath);
        var receipts = new List<SilentScheduleReceipt>();
        if (!Directory.Exists(directory)) return receipts;
        using var held = Lock(directory);
        if (IcasUpdateStore.Hash(contactsPath) != currentHash)
            throw new IOException("The contact file changed while reading receipts. Refresh and try again.");
        foreach (string path in Directory.EnumerateFiles(directory, "*.receipt.json"))
        {
            var record = JsonSerializer.Deserialize<SaveRecord>(File.ReadAllText(path))
                ?? throw new IOException("Unable to read a contact import receipt.");
            if (!record.Complete)
            {
                if (currentHash == record.AfterHash)
                    Write(path, record with { Complete = true });
                else if (currentHash == record.BeforeHash)
                {
                    File.Delete(path);
                    continue;
                }
                else
                    throw new IOException($"The contact file changed during an interrupted import. Check the contact backups and receipt at {path} before continuing.");
            }
            receipts.AddRange(record.Receipts);
        }
        return receipts;
    }

    internal static void Commit(string contactsPath, string expectedHash, List<cContact> updated,
        SilentScheduleReceipt receipt, string? originalCreateId = null)
    {
        string target = Path.GetFullPath(contactsPath);
        string directory = DirectoryFor(target);
        Directory.CreateDirectory(directory);
        using var held = Lock(directory);
        string path = Path.Combine(directory, Guid.Parse(receipt.ChangeId).ToString() + ".receipt.json");
        string staged = target + "." + Guid.NewGuid().ToString("N") + ".tmp";
        var receipts = new List<SilentScheduleReceipt> { receipt };
        if (originalCreateId != null)
            receipts.Add(new SilentScheduleReceipt { ChangeId = originalCreateId, LocalId = receipt.LocalId });
        try
        {
            cContact.SaveMe(updated, staged);
            if (IcasUpdateStore.Hash(target) != expectedHash)
                throw new IOException("The contact file changed during review. Refresh and review again.");
            var record = new SaveRecord(expectedHash, IcasUpdateStore.Hash(staged), false, receipts);
            Write(path, record);
            try { File.Move(staged, target, overwrite: true); }
            catch
            {
                File.Delete(path);
                throw;
            }
            Write(path, record with { Complete = true });
        }
        finally { File.Delete(staged); }
    }

    private static void Write(string path, SaveRecord record)
    {
        string temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temp, JsonSerializer.Serialize(record));
            File.Move(temp, path, overwrite: true);
        }
        finally { File.Delete(temp); }
    }
}
