namespace AirshowSchedules;

internal sealed class FormNeedsAttention : Form
{
    internal FormNeedsAttention(int year, Func<bool, List<AttentionItem>> load, Action<Airshow> open, Action<AttentionItem> toggleReview, string context)
    {
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        Text = $"Needs attention — {year}";
        Width = 1200; Height = 800; MinimumSize = new Size(900, 550); StartPosition = FormStartPosition.CenterParent;
        var intro = new Label { Dock = DockStyle.Top, Height = 65, Padding = new Padding(8), Text =
            "Across all regions. Open a show to edit it, or mark a warning reviewed without changing its booking status.\n" + context };
        var options = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Height = 38 };
        var category = new ComboBox { Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
        category.Items.AddRange(new object[] { "All reasons", "Cancellation warning", "Booked dates changed", "Overlapping commitments", "Missing from ICAS", "Missing contact details", "Pursue next step" }); category.SelectedIndex = 0;
        var past = new CheckBox { Text = "Include past shows", AutoSize = true };
        var reviewed = new CheckBox { Text = "Show reviewed", AutoSize = true };
        options.Controls.AddRange(new Control[] { category, past, reviewed });
        var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
        foreach (string name in new[] { "Reason", "Show", "Dates", "Location", "Status", "Reviewed" }) grid.Columns.Add(name, name);
        grid.Columns["Show"].FillWeight = 180; grid.Columns["Reviewed"].FillWeight = 50;
        var detail = new TextBox { Dock = DockStyle.Bottom, Height = 170, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
        ScreenLayout.FitOnLoad(this);
        var buttons = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Bottom, Height = 45, Padding = new Padding(6), FlowDirection = FlowDirection.RightToLeft };
        var close = new Button { Text = "Close", AutoSize = true, DialogResult = DialogResult.Cancel };
        var edit = new Button { Text = "Open show…", AutoSize = true };
        var review = new Button { Text = "Mark reviewed", AutoSize = true };
        var count = new Label { AutoSize = true, Padding = new Padding(8) };
        buttons.Controls.AddRange(new Control[] { close, edit, review, count });
        AttentionItem? Selected() => grid.CurrentRow?.Tag as AttentionItem;
        void Details()
        {
            var item = Selected(); edit.Enabled = review.Enabled = item != null;
            review.Text = item?.Reviewed == true ? "Reopen warning" : "Mark reviewed";
            detail.Text = item == null ? "No warnings match this view." : $"{item.Show.name_airshow}\r\n{item.Category}\r\n\r\n{item.Details.Replace("\n", "\r\n")}";
        }
        void Reload()
        {
            string? selectedKey = Selected()?.Key;
            var items = load(past.Checked).Where(i => (reviewed.Checked || !i.Reviewed) &&
                (category.SelectedIndex == 0 || i.Category == (string)category.SelectedItem!)).ToList();
            grid.Rows.Clear();
            foreach (var item in items)
            {
                int row = grid.Rows.Add(item.Category, item.Show.name_airshow, $"{item.Show.date_start}–{item.Show.date_finish}",
                    $"{item.Show.location.city}, {item.Show.location.state}", CalendarPlanning.Status(item.Show), item.Reviewed ? "Yes" : "No");
                grid.Rows[row].Tag = item;
                grid.Rows[row].DefaultCellStyle.BackColor = item.Priority == 0 ? Color.MistyRose : item.Priority == 1 ? Color.LightYellow : Color.White;
                if (item.Key == selectedKey) grid.CurrentCell = grid.Rows[row].Cells[0];
            }
            count.Text = $"{items.Count} warnings · {items.Select(i => i.Show.ID).Distinct().Count()} shows";
            Details();
        }
        void Open()
        {
            var item = Selected(); if (item == null) return;
            try { open(item.Show); Reload(); }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Unable to edit show"); }
        }
        edit.Click += (_, _) => Open(); grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Open(); };
        review.Click += (_, _) =>
        {
            var item = Selected(); if (item == null) return;
            try { toggleReview(item); Reload(); } catch (Exception ex) { MessageBox.Show(this, ex.Message, "Unable to save review"); }
        };
        category.SelectedIndexChanged += (_, _) => Reload(); past.CheckedChanged += (_, _) => Reload(); reviewed.CheckedChanged += (_, _) => Reload();
        grid.SelectionChanged += (_, _) => Details();
        Controls.Add(grid); Controls.Add(detail); Controls.Add(options); Controls.Add(intro); Controls.Add(buttons);
        CancelButton = close;
        Reload();
    }
}
