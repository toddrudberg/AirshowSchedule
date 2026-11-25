namespace AirshowSchedules;

internal sealed class FormContacts : Form
{
    private readonly List<cContact> contacts;
    private readonly TextBox search = new() { Dock = DockStyle.Top, PlaceholderText = "Search name, phone or email", AccessibleName = "Search contacts" };
    private readonly DataGridView grid = new()
    {
        Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        MultiSelect = false, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AccessibleName = "Contacts"
    };
    private readonly Button edit = new() { Text = "Edit…", AutoSize = true, Enabled = false };
    private readonly Button delete = new() { Text = "Delete…", AutoSize = true, Enabled = false };
    private readonly Button save = new() { Text = "Save changes", AutoSize = true, Enabled = false };

    internal FormContacts(List<cContact> contacts, Action<List<cContact>> saveContacts)
    {
        this.contacts = contacts;
        Text = "Contacts";
        ClientSize = new Size(760, 480);
        MinimumSize = new Size(460, 300);
        Padding = new Padding(8);
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ScreenLayout.FitOnLoad(this);

        grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", FillWeight = 120 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Phone", FillWeight = 80 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", FillWeight = 150 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Shows", FillWeight = 40 });

        var add = new Button { Text = "New contact…", AutoSize = true };
        var cancel = new Button { Text = "Cancel", AutoSize = true, DialogResult = DialogResult.Cancel };
        var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
        actions.Controls.AddRange(new Control[] { add, edit, delete, save, cancel });
        Controls.Add(grid);
        Controls.Add(search);
        Controls.Add(actions);
        CancelButton = cancel;

        add.Click += (_, _) => EditContact(null);
        edit.Click += (_, _) => EditSelectedContact();
        grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditSelectedContact(); };
        grid.SelectionChanged += (_, _) => edit.Enabled = delete.Enabled = SelectedContact != null;
        search.TextChanged += (_, _) => RefreshContacts();
        delete.Click += (_, _) => DeleteSelectedContact();
        save.Click += (_, _) =>
        {
            try
            {
                saveContacts(contacts);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or Newtonsoft.Json.JsonException)
            {
                MessageBox.Show(this, ex.Message, "Could not save contacts", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };
        RefreshContacts();
    }

    private cContact? SelectedContact => grid.SelectedRows.Count == 0 ? null : grid.SelectedRows[0].Tag as cContact;

    private void RefreshContacts(cContact? selected = null)
    {
        string term = search.Text.Trim();
        bool Matches(string? value) => value?.Contains(term, StringComparison.OrdinalIgnoreCase) == true;
        grid.Rows.Clear();
        foreach (var contact in contacts.OrderBy(c => c.name, StringComparer.CurrentCultureIgnoreCase))
        {
            if (term.Length > 0 && !Matches(contact.name) && !Matches(contact.phone) && !contact.emailAddresses.Any(Matches))
                continue;
            int row = grid.Rows.Add(contact.name, contact.phone, string.Join(", ", contact.emailAddresses), contact.showIDs.Distinct().Count());
            grid.Rows[row].Tag = contact;
            if (ReferenceEquals(contact, selected)) grid.CurrentCell = grid.Rows[row].Cells[0];
        }
        edit.Enabled = delete.Enabled = SelectedContact != null;
    }

    private void EditSelectedContact()
    {
        if (SelectedContact is { } selected) EditContact(selected);
    }

    private void EditContact(cContact? selected)
    {
        var draft = selected?.DeepCopy() ?? new cContact { name = "", phone = "", address = "" };
        using var editor = new ContactEditForm(draft) { Text = selected == null ? "New contact" : "Edit contact" };
        if (editor.ShowDialog(this) != DialogResult.OK) return;

        if (selected == null)
        {
            draft.ID = contacts.Select(c => c.ID).DefaultIfEmpty(-1).Max() + 1;
            contacts.Add(draft);
        }
        else
        {
            contacts[contacts.FindIndex(c => ReferenceEquals(c, selected))] = draft;
        }
        save.Enabled = true;
        search.Clear();
        RefreshContacts(draft);
    }

    private void DeleteSelectedContact()
    {
        if (SelectedContact is not { } selected) return;
        int linkedShows = selected.showIDs.Distinct().Count();
        string prompt = $"Delete {selected.name} from the contact database?";
        if (linkedShows > 0) prompt += $"\nThis will remove the contact from {linkedShows} linked show(s).";
        if (MessageBox.Show(this, prompt, "Delete contact", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

        contacts.RemoveAt(contacts.FindIndex(c => ReferenceEquals(c, selected)));
        save.Enabled = true;
        RefreshContacts();
    }
}
