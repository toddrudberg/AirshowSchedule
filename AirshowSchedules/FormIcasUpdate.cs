namespace AirshowSchedules;

internal sealed class FormIcasUpdate : Form
{
    internal FormIcasUpdate(IcasUpdatePlan plan)
    {
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        Text = $"Review ICAS update — {plan.Year}";
        Width = 1200; Height = 800; MinimumSize = new Size(850, 550);
        StartPosition = FormStartPosition.CenterParent;
        var table = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
        table.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Apply", HeaderText = "Apply", FillWeight = 25 });
        foreach (string column in new[] { "Group", "Show", "Field", "Existing", "ICAS" })
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = column, HeaderText = column, ReadOnly = true });
        table.Columns["Show"].FillWeight = 160;
        var details = new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Bottom, Height = 180 };
        var summary = new Label { Dock = DockStyle.Top, Height = 65, Padding = new Padding(10), Text =
            $"{plan.Changes.Count(c => c.Group == "New shows")} new shows · {plan.Changes.Count(c => c.Apply != null && c.Group != "New shows")} field updates · {plan.Unchanged} unchanged · {plan.CancellationCount} cancellation warnings\n" +
            "Notes, booking statuses and links are preserved. Contact replacements need your selection. Missing shows are kept." };
        ScreenLayout.FitOnLoad(this);
        var buttons = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(6) };
        var apply = new Button { Text = "Apply selected changes", AutoSize = true };
        var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true };
        buttons.Controls.Add(apply); buttons.Controls.Add(cancel);
        var filterBar = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Height = 38 };
        var filter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 240 };
        filter.Items.Add("All changes");
        foreach (string group in plan.Changes.Select(c => c.Group).Distinct()) filter.Items.Add(group);
        filter.SelectedIndex = 0;
        filterBar.Controls.Add(new Label { Text = "Show:", AutoSize = true, Padding = new Padding(6) }); filterBar.Controls.Add(filter);
        var selectVisible = new Button { Text = "Select visible changes", AutoSize = true };
        var clearVisible = new Button { Text = "Clear visible selections", AutoSize = true };
        filterBar.Controls.Add(selectVisible); filterBar.Controls.Add(clearVisible);
        void SelectVisible(bool selected)
        {
            table.EndEdit();
            foreach (DataGridViewRow row in table.Rows)
                if (row.Visible && ((IcasChange)row.Tag).Apply != null) row.Cells[0].Value = selected;
        }
        selectVisible.Click += (_, _) => SelectVisible(true);
        clearVisible.Click += (_, _) => SelectVisible(false);
        foreach (var change in plan.Changes)
        {
            int index = table.Rows.Add(change.Selected, change.Group, change.Show, change.Field, change.Before.Replace("\n", "; "), change.After.Replace("\n", "; "));
            var row = table.Rows[index]; row.Tag = change;
            if (change.Apply == null) { row.Cells[0].ReadOnly = true; row.DefaultCellStyle.ForeColor = Color.DimGray; }
            if (change.Show.Contains("CANCELLED", StringComparison.OrdinalIgnoreCase)) row.DefaultCellStyle.BackColor = Color.LightYellow;
        }
        void RefreshApply()
        {
            int count = plan.Changes.Count(c => c.Selected && c.Apply != null);
            apply.Enabled = count > 0;
            apply.Text = $"Apply {count} selected changes";
        }
        table.CurrentCellDirtyStateChanged += (_, _) => { if (table.IsCurrentCellDirty) table.CommitEdit(DataGridViewDataErrorContexts.Commit); };
        table.CellValueChanged += (_, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0 && table.Rows[e.RowIndex].Tag is IcasChange c)
            { c.Selected = c.Apply != null && Convert.ToBoolean(table.Rows[e.RowIndex].Cells[0].Value); RefreshApply(); }
        };
        table.SelectionChanged += (_, _) =>
        {
            if (table.CurrentRow?.Tag is IcasChange c)
                details.Text = $"{c.Show}\r\n{c.Field}\r\n\r\nExisting:\r\n{c.Before.Replace("\n", "\r\n")}\r\n\r\nICAS:\r\n{c.After.Replace("\n", "\r\n")}";
        };
        filter.SelectedIndexChanged += (_, _) =>
        {
            table.CurrentCell = null;
            foreach (DataGridViewRow row in table.Rows) row.Visible = filter.SelectedIndex == 0 || ((IcasChange)row.Tag).Group == (string)filter.SelectedItem;
        };
        apply.Click += (_, _) => { table.EndEdit(); DialogResult = DialogResult.OK; Close(); };
        Controls.Add(table); Controls.Add(details); Controls.Add(filterBar); Controls.Add(summary); Controls.Add(buttons);
        CancelButton = cancel;
        RefreshApply();
    }
}
