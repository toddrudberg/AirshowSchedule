using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AirshowSchedules;

public partial class formMain
{
    private void InstallMaintenanceMenu()
    {
        dataBaseManagementToolStripMenuItem.DropDownItems.Insert(0, new ToolStripMenuItem("Clean up database…", null, RunDatabaseMaintenance));
        dataBaseManagementToolStripMenuItem.DropDownItems.Insert(1, new ToolStripMenuItem("Undo last cleanup…", null, UndoDatabaseMaintenance));
        dataBaseManagementToolStripMenuItem.DropDownItems.Insert(2, new ToolStripSeparator());
    }

    private static AirshowGroup ReadShowDatabase(string path)
    {
        using var reader = File.OpenRead(path);
        return (AirshowGroup)new XmlSerializer(typeof(AirshowGroup)).Deserialize(reader)!;
    }

    private static List<cContact> ReadContactDatabase(string path) => JsonConvert.DeserializeObject<List<cContact>>(File.ReadAllText(path))
        ?? throw new FormatException("The contacts file contains no contact list.");

    private void RunDatabaseMaintenance(object? sender, EventArgs e)
    {
        try
        {
            RequireActiveFiles();
            using var picker = new OpenFileDialog { Title = "Choose the latest comparison database for all cleanup steps", Filter = "Airshow databases (*.asg.xml)|*.asg.xml" };
            if (picker.ShowDialog(this) != DialogResult.OK) return;
            string sourceContactsPath = Path.ChangeExtension(Path.ChangeExtension(picker.FileName, null), ".contacts.json");
            if (!File.Exists(sourceContactsPath))
            {
                using var contactPicker = new OpenFileDialog { Title = "Choose the contacts file belonging to the comparison database", Filter = "Contact databases (*.json)|*.json" };
                if (contactPicker.ShowDialog(this) != DialogResult.OK) return;
                sourceContactsPath = contactPicker.FileName;
            }
            string databaseHash = IcasUpdateStore.Hash(myFormState.fnCurrentXMLDataBase);
            string contactsHash = IcasUpdateStore.Hash(myFormState.fnContactDataBase);
            var session = new DatabaseMaintenance(ReadShowDatabase(myFormState.fnCurrentXMLDataBase), ReadContactDatabase(myFormState.fnContactDataBase),
                ReadShowDatabase(picker.FileName), ReadContactDatabase(sourceContactsPath));
            using var review = new FormDatabaseMaintenance(session);
            if (review.ShowDialog(this) != DialogResult.OK) return;
            string backup = DatabaseMaintenanceStore.Commit(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase, session, databaseHash, contactsHash);
            myAirshowGroup = session.Database;
            myContacts = session.Contacts;
            RefreshAfterIcasUpdate();
            MessageBox.Show(this, $"Cleanup complete. Applied {session.Changes.Count} actions.\nBoth original databases are backed up in:\n{backup}\n\nUse Database Management → Undo last cleanup if needed.", "Database cleanup");
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Database cleanup could not complete"); }
    }

    private void UndoDatabaseMaintenance(object? sender, EventArgs e)
    {
        try
        {
            RequireActiveFiles();
            if (MessageBox.Show(this, "Restore both active databases to their state before the last cleanup?", "Undo cleanup", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            DatabaseMaintenanceStore.Undo(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase);
            myAirshowGroup = ReadShowDatabase(myFormState.fnCurrentXMLDataBase);
            myContacts = ReadContactDatabase(myFormState.fnContactDataBase);
            RefreshAfterIcasUpdate();
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Cleanup undo could not complete"); }
    }
}
