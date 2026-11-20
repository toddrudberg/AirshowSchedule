using System.Text.RegularExpressions;

namespace AirshowSchedules;

public partial class formMain
{
    private void InstallIcasUpdateMenu()
    {
        var update = new ToolStripMenuItem("Update from ICAS…", null, UpdateFromIcas);
        menuStrip1.Items.Insert(0, update);
        var undo = new ToolStripMenuItem("Undo last ICAS update…", null, UndoIcasUpdate);
        var history = new ToolStripMenuItem("ICAS update history…", null, ShowIcasHistory);
        fileToolStripMenuItem.DropDownItems.Insert(0, history);
        fileToolStripMenuItem.DropDownItems.Insert(0, undo);
        var advanced = new ToolStripMenuItem("Advanced import / export");
        fileToolStripMenuItem.DropDownItems.Remove(pareDataFileToolStripMenuItem);
        fileToolStripMenuItem.DropDownItems.Remove(fileSaveParsedDataFile);
        advanced.DropDownItems.Add(pareDataFileToolStripMenuItem);
        advanced.DropDownItems.Add(fileSaveParsedDataFile);
        dataBaseManagementToolStripMenuItem.DropDownItems.Remove(compareToActiveDBToolStripMenuItem);
        advanced.DropDownItems.Add(compareToActiveDBToolStripMenuItem);
        fileToolStripMenuItem.DropDownItems.Add(advanced);
    }

    private void RequireActiveFiles()
    {
        if (!File.Exists(myFormState.fnCurrentXMLDataBase) || !File.Exists(myFormState.fnContactDataBase))
            throw new IOException("Choose an active show database and contact database from the File menu first.");
    }

    private void UpdateFromIcas(object? sender, EventArgs e)
    {
        try
        {
            RequireActiveFiles();
            using var picker = new OpenFileDialog { Title = "Choose the downloaded ICAS calendar", Filter = "ICAS calendars (*.doc;*.html;*.htm)|*.doc;*.html;*.htm" };
            if (picker.ShowDialog(this) != DialogResult.OK) return;
            string sourceHash = IcasUpdateStore.Hash(picker.FileName);
            string content = File.ReadAllText(picker.FileName);
            // Date cells are authoritative, rather than the file name or the current calendar year.
            var years = Regex.Matches(content, @"\b(20\d{2})-\d{2}-\d{2}\b").Cast<Match>()
                .Select(m => int.Parse(m.Groups[1].Value)).Distinct().ToArray();
            if (years.Length != 1) throw new FormatException("Expected a calendar containing exactly one year of event dates.");
            var imported = IcasHtmlImporter.Parse(content, years[0]);
            string dbHash = IcasUpdateStore.Hash(myFormState.fnCurrentXMLDataBase);
            string contactsHash = IcasUpdateStore.Hash(myFormState.fnContactDataBase);
            // Read the active files afresh so an external edit cannot be lost to a stale loaded copy.
            AirshowGroup active;
            using (var reader = File.OpenRead(myFormState.fnCurrentXMLDataBase))
                active = (AirshowGroup)new System.Xml.Serialization.XmlSerializer(typeof(AirshowGroup)).Deserialize(reader)!;
            var activeContacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cContact>>(File.ReadAllText(myFormState.fnContactDataBase))
                ?? throw new FormatException("The active contacts file contains no contact list.");
            var plan = new IcasUpdatePlan(active, activeContacts, imported.Shows, imported.Contacts, years[0]);
            using var review = new FormIcasUpdate(plan);
            if (review.ShowDialog(this) != DialogResult.OK) return;
            int count = plan.ApplySelected();
            if (count == 0) return;
            IcasUpdateStore.Commit(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase, picker.FileName, plan, dbHash, contactsHash, sourceHash);
            myAirshowGroup = plan.Database;
            myContacts = plan.Contacts;
            RefreshAfterIcasUpdate();
            MessageBox.Show(this, $"Applied {count} changes. Both databases were backed up and the original download was archived.\nUse File → Undo last ICAS update if needed.", "ICAS update complete");
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "ICAS update could not complete"); }
    }

    private void RefreshAfterIcasUpdate()
    {
        linkLabelDB.Text = $"Airshow Year of Interest: {myAirshowGroup.AirshowYearOfInterest} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
        LoadGrid(myAirshowGroup.AirshowYearOfInterest);
        lstBoxShows.Items.Clear();
        btnFilterShows_Click(this, EventArgs.Empty);
        ColorGrid(myAirshowGroup.GetAirshowsForYear());
        ColorGrid(myFilteredAirshows);
    }

    private void UndoIcasUpdate(object? sender, EventArgs e)
    {
        try
        {
            RequireActiveFiles();
            if (MessageBox.Show(this, "Restore both databases to their state before the last ICAS update?", "Undo ICAS update", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            IcasUpdateStore.Undo(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase);
            myAirshowGroup = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase, out bool showsOk);
            myContacts = cContact.LoadMe(myFormState.fnContactDataBase, out bool contactsOk);
            if (!showsOk || !contactsOk) throw new IOException("Restored files could not be loaded.");
            RefreshAfterIcasUpdate();
            MessageBox.Show(this, "The last ICAS update was undone.", "ICAS update");
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Unable to undo ICAS update"); }
    }

    private void ShowIcasHistory(object? sender, EventArgs e)
    {
        try
        {
            RequireActiveFiles();
            var history = IcasUpdateStore.History(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase);
            using var window = new Form { Text = "ICAS update history", Width = 950, Height = 650, StartPosition = FormStartPosition.CenterParent };
            var list = new ListBox { Dock = DockStyle.Top, Height = 160 };
            var details = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Both };
            foreach (var record in history) list.Items.Add($"{record.Created:g} — {record.Year} — {record.Changes} changes — {record.Status} — {record.SourceName}");
            list.SelectedIndexChanged += (_, _) =>
            {
                if (list.SelectedIndex >= 0)
                {
                    var record = history[list.SelectedIndex];
                    details.Text = $"Backup and source folder:\r\n{record.Folder}\r\n\r\n" + string.Join("\r\n\r\n", record.Details).Replace("\n", "\r\n");
                }
            };
            window.Controls.Add(details); window.Controls.Add(list);
            if (list.Items.Count > 0) list.SelectedIndex = 0;
            else details.Text = "No ICAS updates have been applied to these databases yet.";
            window.ShowDialog(this);
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Unable to read ICAS history"); }
    }
}
