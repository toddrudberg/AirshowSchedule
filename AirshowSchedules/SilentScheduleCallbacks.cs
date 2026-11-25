namespace AirshowSchedules;

public partial class formMain
{
    private void InstallSilentScheduleMenu()
    {
        var menu = new ToolStripMenuItem("Silent Schedule");
        menu.DropDownItems.Add("Publish Silent Schedule…", null, PublishSilentSchedule);
        menu.DropDownItems.Add("Review Web Changes…", null, ReviewSilentChanges);
        menuStrip1.Items.Insert(menuStrip1.Items.IndexOf(searchToolStripMenuItem), menu);
    }

    private (AirshowGroup Database, List<cContact> Contacts, string DatabaseHash, string ContactsHash,
        List<SilentScheduleReceipt> ContactReceipts) ReadSilentLocal()
    {
        RequireActiveFiles();
        string databasePath = myFormState.fnCurrentXMLDataBase, contactsPath = myFormState.fnContactDataBase;
        string databaseHash = IcasUpdateStore.Hash(databasePath), contactsHash = IcasUpdateStore.Hash(contactsPath);
        var database = AirshowGroup.LoadMe(databasePath, out bool loaded);
        var contacts = cContact.LoadMe(contactsPath, out bool contactsLoaded);
        if (!loaded || !contactsLoaded || contacts == null)
            throw new IOException("Could not read the active show and contact files.");
        var contactReceipts = SilentContactSaves.Read(contactsPath, contactsHash);
        if (IcasUpdateStore.Hash(databasePath) != databaseHash || IcasUpdateStore.Hash(contactsPath) != contactsHash)
            throw new IOException("The active files changed while being read. Try again.");
        if (database.GetAirshows().Select(s => s.ID).Distinct().Count() != database.GetAirshows().Count ||
            contacts.Select(c => c.ID).Distinct().Count() != contacts.Count)
            throw new IOException("Resolve duplicate show or contact IDs in the active files first.");
        return (database, contacts, databaseHash, contactsHash, contactReceipts);
    }

    private static Form SilentDialog(string title, Size size, Size minimum)
    {
        var dialog = new Form
        {
            Text = title, ClientSize = size, MinimumSize = minimum, StartPosition = FormStartPosition.CenterParent,
            AutoScaleDimensions = new SizeF(7, 15), AutoScaleMode = AutoScaleMode.Font, Padding = new Padding(12)
        };
        ScreenLayout.FitOnLoad(dialog);
        return dialog;
    }

    private void PublishSilentSchedule(object? sender, EventArgs e)
    {
        int year = myAirshowGroup.AirshowYearOfInterest;
        string address = Environment.GetEnvironmentVariable("SILENT_SCHEDULE_URL") ?? SilentScheduleClient.DefaultUrl;
        using var dialog = SilentDialog($"Publish Silent Schedule · {year}", new Size(620, 245), new Size(440, 230));
        var url = new TextBox { Text = address, ReadOnly = true, Dock = DockStyle.Top, AccessibleName = "Private schedule URL" };
        var timestamp = new Label { Dock = DockStyle.Top, Height = 28, Text = "Last successful publish: checking…" };
        var feedback = new Label { Dock = DockStyle.Fill, AccessibleName = "Publish status" };
        var publish = new Button { Text = $"Publish {year}", AutoSize = true, Enabled = false };
        var copy = new Button { Text = "Copy private URL", AutoSize = true };
        var close = new Button { Text = "Close", AutoSize = true, DialogResult = DialogResult.Cancel };
        var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 44 };
        actions.Controls.AddRange(new Control[] { publish, copy, close });
        dialog.Controls.Add(feedback);
        dialog.Controls.Add(actions);
        dialog.Controls.Add(timestamp);
        dialog.Controls.Add(url);
        dialog.CancelButton = close;
        bool busy = false;
        dialog.FormClosing += (_, args) => args.Cancel = busy;
        copy.Click += (_, _) => Clipboard.SetText(address);

        async Task Run(bool send)
        {
            busy = true;
            publish.Enabled = false;
            feedback.ForeColor = SystemColors.ControlText;
            feedback.Text = send ? "Publishing the private schedule…" : "";
            try
            {
                using var client = new SilentScheduleClient();
                SilentPublishState result;
                if (send)
                {
                    var local = ReadSilentLocal();
                    result = await client.Publish(SilentSchedule.Build(local.Database, local.Contacts, myRegions, year, local.ContactReceipts));
                    feedback.Text = $"Published {year} successfully.";
                }
                else result = await client.State(year);
                timestamp.Text = result.PublishedAt is { } time
                    ? $"Last successful publish: {time.ToLocalTime():g} (version {result.Revision})"
                    : "Last successful publish: none for this year.";
            }
            catch (Exception ex)
            {
                feedback.ForeColor = Color.DarkRed;
                feedback.Text = (send ? "Publish failed. " : "Could not check publish status. ") + ex.Message;
                if (timestamp.Text.EndsWith("checking…", StringComparison.Ordinal))
                    timestamp.Text = "Last successful publish: unavailable.";
            }
            finally { busy = false; publish.Enabled = true; }
        }
        dialog.Shown += async (_, _) => await Run(false);
        publish.Click += async (_, _) => await Run(true);
        dialog.ShowDialog(this);
    }

    private void ReviewSilentChanges(object? sender, EventArgs e)
    {
        int year = myAirshowGroup.AirshowYearOfInterest;
        using var dialog = SilentDialog($"Review Web Changes · {year}", new Size(980, 650), new Size(550, 420));
        var table = new DataGridView
        {
            Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true,
            RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
            AllowUserToAddRows = false, AllowUserToDeleteRows = false
        };
        foreach (string heading in new[] { "Change", "Show / contact", "Submitted by", "State" })
            table.Columns.Add(heading, heading);
        var details = new TextBox
        {
            Multiline = true, ReadOnly = true, Dock = DockStyle.Bottom, Height = 175,
            ScrollBars = ScrollBars.Vertical, AccessibleName = "Selected change details"
        };
        var feedback = new Label { Dock = DockStyle.Top, Height = 52, AccessibleName = "Review status" };
        var apply = new Button { Text = "Import selected change", AutoSize = true, Enabled = false };
        var refresh = new Button { Text = "Refresh", AutoSize = true };
        var close = new Button { Text = "Close", AutoSize = true, DialogResult = DialogResult.Cancel };
        var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 44 };
        actions.Controls.AddRange(new Control[] { apply, refresh, close });
        dialog.Controls.Add(table);
        dialog.Controls.Add(details);
        dialog.Controls.Add(actions);
        dialog.Controls.Add(feedback);
        dialog.CancelButton = close;
        bool busy = false;
        dialog.FormClosing += (_, args) => args.Cancel = busy;

        (SilentChange Change, SilentReview Review)? Selected() =>
            table.CurrentRow?.Tag is ValueTuple<SilentChange, SilentReview> pair ? pair : null;
        void Selection()
        {
            var selected = Selected();
            apply.Enabled = !busy && selected is { Review.Blocked: null };
            apply.Text = selected?.Review.Saved != null ? "Retry acknowledgment" : "Import selected change";
            if (selected is not { } row) { details.Clear(); return; }
            var review = row.Review;
            details.Text = ($"{review.Subject}\nSnapshot / web baseline: {review.SnapshotValue}\n" +
                $"Local: {review.LocalValue}\nProposed: {review.ProposedValue}\n" +
                (review.Blocked ?? (review.Conflict ? "Conflict: the local value changed. Confirm to replace it." : "")))
                .ReplaceLineEndings("\r\n");
        }
        table.SelectionChanged += (_, _) => Selection();

        async Task Load(SilentScheduleClient client)
        {
            var changes = await client.Changes(year);
            var local = ReadSilentLocal();
            table.Rows.Clear();
            foreach (var change in changes)
            {
                var review = SilentSchedule.Inspect(local.Database, local.Contacts, change, local.ContactReceipts);
                string state = review.Saved != null ? "Saved locally" : review.Blocked != null ? "Blocked" :
                    review.Conflict ? "Conflict" : "Pending import";
                string kind = change.Kind switch
                {
                    "status" => "Show status", "addShow" => "New show", "addContact" => "New contact",
                    "editContact" => "Edit contact", "deleteContact" => "Delete contact", _ => change.Kind
                };
                int index = table.Rows.Add(kind, review.Subject, change.User, state);
                table.Rows[index].Tag = (change, review);
                if (review.Conflict) table.Rows[index].DefaultCellStyle.BackColor = Color.LightYellow;
            }
            feedback.Text = changes.Count == 0 ? $"No pending web changes for {year}." : $"{changes.Count} web change(s) ready for review.";
        }

        async Task Run(bool import)
        {
            var selected = Selected();
            busy = true;
            refresh.Enabled = false;
            table.Enabled = false;
            Selection();
            feedback.ForeColor = SystemColors.ControlText;
            feedback.Text = import ? "Importing selected change…" : "Checking for web changes…";
            SilentScheduleReceipt? receipt = null;
            try
            {
                using var client = new SilentScheduleClient();
                if (import && selected is { } row)
                {
                    var local = ReadSilentLocal();
                    var latest = SilentSchedule.Inspect(local.Database, local.Contacts, row.Change, local.ContactReceipts);
                    if (latest.Saved == null && latest != row.Review)
                    {
                        await Load(client);
                        feedback.Text = "The local value changed during review. Review the updated values before importing.";
                        return;
                    }
                    if (latest.Conflict && MessageBox.Show(dialog,
                        $"{latest.Subject}\nCurrent local value: {latest.LocalValue}\nProposed: {latest.ProposedValue}\n\nApply the proposed value?"
                            .ReplaceLineEndings("\r\n"),
                        "Confirm local conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        feedback.Text = "No change applied.";
                        return;
                    }
                    receipt = SilentSchedule.Apply(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase,
                        local.Database, local.Contacts, local.DatabaseHash, local.ContactsHash, row.Change);
                    myAirshowGroup = local.Database;
                    myContacts = local.Contacts;
                    RefreshAfterIcasUpdate();
                    await client.Ack(receipt);
                }
                await Load(client);
                if (import) feedback.Text = "Saved locally and acknowledged. Publish this year to send a fresh snapshot. " + feedback.Text;
            }
            catch (Exception ex)
            {
                if (receipt != null && table.CurrentRow is { } current && selected is { } row &&
                    Selected()?.Change.Id == row.Change.Id)
                {
                    var review = row.Review with { Saved = receipt, LocalValue = "Saved on this PC", Conflict = false, Blocked = null };
                    current.Tag = (row.Change, review);
                    current.Cells[3].Value = "Saved locally";
                }
                feedback.ForeColor = Color.DarkRed;
                feedback.Text = receipt != null ? "Saved locally. Refresh or retry acknowledgment. " + ex.Message : ex.Message;
            }
            finally { busy = false; refresh.Enabled = true; table.Enabled = true; Selection(); }
        }
        dialog.Shown += async (_, _) => await Run(false);
        refresh.Click += async (_, _) => await Run(false);
        apply.Click += async (_, _) => await Run(true);
        dialog.ShowDialog(this);
    }
}
