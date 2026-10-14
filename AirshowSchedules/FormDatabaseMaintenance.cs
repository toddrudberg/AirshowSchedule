namespace AirshowSchedules;

internal sealed class FormDatabaseMaintenance : Form
{
    private readonly DatabaseMaintenance session;
    private readonly DataGridView grid = new() { Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
    private readonly Label heading = new() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(10), MaximumSize = new Size(800, 0) };
    private readonly TextBox details = new() { Dock = DockStyle.Bottom, Height = 110, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
    private readonly Button next = new() { AutoSize = true };
    private readonly Button skip = new() { AutoSize = true, Text = "Skip this step" };
    private List<MaintenanceChoice> choices = new();
    private int step;

    internal FormDatabaseMaintenance(DatabaseMaintenance session)
    {
        this.session = session;
        Text = $"Clean up database — {session.Year}";
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1050, 700);
        StartPosition = FormStartPosition.CenterParent;
        ScreenLayout.FitOnLoad(this);
        grid.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Choose", HeaderText = "Choose", FillWeight = 18 });
        foreach (string name in new[] { "Active show", "Reason", "Comparison show" })
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = name, HeaderText = name, ReadOnly = true });
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        grid.SelectionChanged += (_, _) =>
        {
            if (grid.CurrentRow?.Tag is MaintenanceChoice choice)
                ShowDetails(choice);
        };
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        var cancel = new Button { AutoSize = true, Text = "Cancel", DialogResult = DialogResult.Cancel };
        buttons.Controls.AddRange(new Control[] { cancel, next, skip });
        CancelButton = cancel;
        next.Click += (_, _) => Advance(false);
        skip.Click += (_, _) => Advance(true);
        Controls.Add(grid);
        Controls.Add(details);
        Controls.Add(buttons);
        Controls.Add(heading);
        Resize += (_, _) => heading.MaximumSize = new Size(Math.Max(100, ClientSize.Width), 0);
        ShowStep();
    }

    private void Advance(bool skipStep)
    {
        try
        {
            grid.EndEdit();
            if (step == 3) { DialogResult = DialogResult.OK; return; }
            if (!skipStep)
            {
                foreach (DataGridViewRow row in grid.Rows)
                    if (row.Tag is MaintenanceChoice choice) choice.Selected = row.Cells[0].Value is true;
                if (step < 2) session.RemoveSelected(choices);
                else session.MergeSelected(choices);
            }
            step++;
            ShowStep();
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, Text); }
    }

    private void ShowStep()
    {
        grid.Rows.Clear();
        details.Clear();
        skip.Visible = step < 3;
        next.Text = step == 3 ? "Apply changes" : "Next";
        heading.Text = step switch
        {
            0 => "1 of 3 · Clean up DB\nCompare active entries with the latest calendar's names and dates. Check only the incorrect copy; leave the entry to keep unchecked. Matching dates are evidence, not an automatic decision.",
            1 => "2 of 3 · Check for cancelled shows\nCheck the shows you want to remove. Cancellation warnings do not automatically delete shows or change booking statuses.",
            2 => "3 of 3 · Update additional fields\nUnique matches start checked. Ambiguous matches need a choice. Existing dates and booking statuses are preserved.",
            _ => $"Review and apply · {session.Changes.Count} actions\nApply saves both active databases and backs up both original files. Cancel discards every staged change."
        };
        grid.Columns[0].Visible = step < 3;
        grid.Columns[0].HeaderText = step < 2 ? "Remove" : "Merge";
        grid.Columns[3].Visible = step == 0 || step == 2;
        grid.Columns[3].HeaderText = step == 0 ? "Latest calendar at this location" : "Comparison show";
        if (step == 3)
        {
            foreach (string change in session.Changes) grid.Rows.Add(false, change, "", "");
            next.Enabled = session.Changes.Count > 0;
            if (session.Changes.Count == 0) details.Text = "No changes selected. Close with Cancel; the active databases are unchanged.";
            return;
        }
        choices = step switch { 0 => session.CleanupChoices(), 1 => session.CancellationChoices(), _ => session.MergeChoices() };
        foreach (var choice in choices)
        {
            string comparison = step == 0 ? (choice.ComparisonShows.Count == 0 ? "No entry at this location/year"
                : string.Join(Environment.NewLine, choice.ComparisonShows.Select(DescribeShow))) : choice.Incoming is null ? "" : DescribeShow(choice.Incoming);
            int index = grid.Rows.Add(choice.Suggested, DescribeShow(choice.Show), choice.Reason, comparison);
            grid.Rows[index].Tag = choice;
        }
        if (choices.Count > 0) ShowDetails(choices[0]);
        if (choices.Count == 0) details.Text = "Nothing to review in this step. Choose Next to continue.";
    }

    private static string DescribeShow(Airshow show) => $"{show.name_airshow} — {show.location.city}, {show.location.state}\r\n{show.date_start} to {show.date_finish} (ID {show.ID})";

    private void ShowDetails(MaintenanceChoice choice)
    {
        details.Text = $"Active: {DescribeShow(choice.Show)}\r\nStatus: {choice.Show.Status}\r\nNotes: {choice.Show.Notes_AirshowStuff}\r\nContacts: {string.Join(", ", cContact.getContacts(session.Contacts, choice.Show).Select(c => c.name))}\r\n{choice.Reason}";
        if (step == 0)
        {
            details.AppendText("\r\n\r\nActive entries at this location:\r\n" + string.Join("\r\n", session.RelatedActiveShows(choice.Show).Select(s => $"{DescribeShow(s)} — Status: {s.Status}")));
            details.AppendText("\r\n\r\nLatest calendar entries:\r\n" + (choice.ComparisonShows.Count == 0 ? "None at this location/year." : string.Join("\r\n", choice.ComparisonShows.Select(DescribeShow))));
        }
        else if (choice.Incoming != null) details.AppendText("\r\nComparison: " + DescribeShow(choice.Incoming));
        details.SelectionStart = 0;
        details.ScrollToCaret();
    }
}
