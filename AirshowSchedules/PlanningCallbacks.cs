namespace AirshowSchedules;

public partial class formMain
{
    private ToolStripMenuItem? attentionMenu;
    private FlowLayoutPanel? statusLegend;
    private void InstallPlanningTools()
    {
        attentionMenu = new ToolStripMenuItem("Needs attention…", null, ShowNeedsAttention);
        menuStrip1.Items.Insert(1, attentionMenu);
        menuStrip1.Items.Insert(2, new ToolStripMenuItem("Weekday shows…", null, ShowWeekdayShows));
        statusLegend = new FlowLayoutPanel { Height = 26, WrapContents = false };
        foreach (var status in new[] { Airshow.eStatus.contract, Airshow.eStatus.verbal, Airshow.eStatus.maybe, Airshow.eStatus.pursue })
            statusLegend.Controls.Add(new Label { AutoSize = true, Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(status.ToString()), BackColor = CalendarPlanning.StatusColor(status), Padding = new Padding(4), Margin = new Padding(0, 0, 8, 0) });
        statusLegend.Controls.Add(new Label { Text = "Weekday events do not color weekends", AutoSize = true, Padding = new Padding(4) });
        Controls.Add(statusLegend);
        var workingArea = Screen.FromControl(this).WorkingArea;
        ClientSize = new Size(Math.Min(1450, Math.Max(1084, workingArea.Width - 40)), Math.Min(850, Math.Max(610, workingArea.Height - 80)));
        MinimumSize = new Size(1100, 650);
        Resize += (_, _) => { LayoutPlanningTools(); if (dgvCalendar.Rows.Count > 0) ColorGrid(myFilteredAirshows); };
        LayoutPlanningTools();
        Disposed += (_, _) => { calendarBoldFont?.Dispose(); calendarRegularFont?.Dispose(); };
    }
    private void LayoutPlanningTools()
    {
        if (statusLegend == null) return;
        linkLabelDB.Location = new Point(8, menuStrip1.Bottom + 2);
        linkLabelContacts.Location = new Point(8, linkLabelDB.Bottom + 2);
        statusLegend.Location = new Point(8, linkLabelContacts.Bottom + 4); statusLegend.Width = ClientSize.Width - 16;
        int top = statusLegend.Bottom + 4;
        int calendarWidth = (int)(ClientSize.Width * .62);
        dgvCalendar.SetBounds(8, top, calendarWidth, ClientSize.Height - top - 8);
        int right = dgvCalendar.Right + 12, width = ClientSize.Width - right - 8;
        labelWeekendSelected.SetBounds(right, top, width, 22);
        lstBoxShows.SetBounds(right, labelWeekendSelected.Bottom + 2, width, Math.Max(180, (ClientSize.Height - top) / 2));
        btnAddShow.SetBounds(right, lstBoxShows.Bottom + 8, 124, 28);
        btnDeleteShow.SetBounds(right + 132, lstBoxShows.Bottom + 8, 124, 28);
        chklstRegions.SetBounds(right, btnAddShow.Bottom + 12, width, Math.Max(100, ClientSize.Height - btnAddShow.Bottom - 65));
        btnFilterSetAll.SetBounds(right, chklstRegions.Bottom + 6, 90, 28);
        btnFilterClearAll.SetBounds(right + 98, chklstRegions.Bottom + 6, 90, 28);
        btnFilterShows.SetBounds(right + 196, chklstRegions.Bottom + 6, Math.Max(100, width - 196), 28);
        SizeCalendarRows();
    }
    private List<AttentionItem> BuildAttention(bool includePast, out string context)
    {
        AttentionImport? imported = null;
        context = "Import an ICAS calendar to also track missing shows and changes to booked dates.";
        HashSet<string> reviewed = new();
        if (File.Exists(myFormState.fnCurrentXMLDataBase) && File.Exists(myFormState.fnContactDataBase))
        {
            imported = AttentionImport.Load(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase, myAirshowGroup, myContacts, myAirshowGroup.AirshowYearOfInterest);
            reviewed = AttentionReviewStore.Load(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase);
            if (imported != null) context = imported.Description;
        }
        return AttentionPlanning.Build(myAirshowGroup, myContacts, myAirshowGroup.AirshowYearOfInterest, DateTime.Today, includePast, imported, reviewed);
    }
    private void RefreshAttentionCount()
    {
        if (attentionMenu == null) return;
        try { attentionMenu.Text = $"Needs attention ({BuildAttention(false, out _).Count(i => !i.Reviewed)})…"; attentionMenu.ToolTipText = "Upcoming shows across all regions"; }
        catch (Exception ex) { attentionMenu.Text = "Needs attention…"; attentionMenu.ToolTipText = ex.Message; }
    }
    private void ShowNeedsAttention(object? sender, EventArgs e)
    {
        try
        {
            RequireActiveFiles();
            BuildAttention(false, out var context);
            using var window = new FormNeedsAttention(myAirshowGroup.AirshowYearOfInterest,
                includePast => BuildAttention(includePast, out _), OpenPlanningShow, item =>
                {
                    var reviewed = AttentionReviewStore.Load(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase);
                    if (!reviewed.Add(item.Key)) reviewed.Remove(item.Key);
                    AttentionReviewStore.Save(myFormState.fnCurrentXMLDataBase, myFormState.fnContactDataBase, reviewed);
                    RefreshAttentionCount();
                }, context);
            window.ShowDialog(this);
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Unable to show attention list"); }
    }
    private void OpenPlanningShow(Airshow show)
    {
        // Edit copies because the legacy editor can change contacts before OK is pressed.
        var edited = Airshow.DeepCopy(new List<Airshow> { show })[0];
        var contacts = cContact.DeepCopy(myContacts);
        using var editor = new AirshowEditForm(edited, contacts);
        if (editor.ShowDialog(this) != DialogResult.OK) return;
        if (!CalendarPlanning.Dates(edited, out _, out _))
            throw new FormatException("Start and finish must be valid yyyy-MM-dd dates, with finish on or after start. The edit was not saved.");
        int index = myAirshowGroup.GetAirshows().FindIndex(s => s.ID == show.ID);
        if (index < 0) throw new InvalidOperationException("This show is no longer in the database.");
        myAirshowGroup.GetAirshows()[index] = edited;
        myContacts = contacts;
        SaveAirshowSchedule(false);
        RefreshSelectedWeekend();
    }
    private void ShowWeekdayShows(object? sender, EventArgs e)
    {
        using var window = new Form { Text = $"Weekday-only shows — {myAirshowGroup.AirshowYearOfInterest}", Width = 1100, Height = 600, StartPosition = FormStartPosition.CenterParent };
        var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
        foreach (string name in new[] { "Show", "Start", "Finish", "Location", "Status" }) grid.Columns.Add(name, name);
        var intro = new Label { Dock = DockStyle.Top, Height = 40, Padding = new Padding(8), Text = "Events with no Saturday or Sunday dates. Uses your current region filter. Double-click to edit." };
        void Reload()
        {
            grid.Rows.Clear();
            foreach (var show in myFilteredAirshows.Where(CalendarPlanning.WeekdayOnly).OrderBy(s => s.date_start))
            {
                int row = grid.Rows.Add(show.name_airshow, show.date_start, show.date_finish, $"{show.location.city}, {show.location.state}", CalendarPlanning.Status(show));
                grid.Rows[row].Tag = show;
            }
        }
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex < 0 || grid.Rows[e.RowIndex].Tag is not Airshow show) return;
            try { OpenPlanningShow(show); Reload(); } catch (Exception ex) { MessageBox.Show(window, ex.Message, "Unable to edit show"); }
        };
        window.Controls.Add(grid); window.Controls.Add(intro); Reload(); window.ShowDialog(this);
    }
    private void RefreshSelectedWeekend()
    {
        lstBoxShows.Items.Clear();
        if (dgvCalendar.CurrentCell?.Tag is not DateTime saturday) return;
        foreach (var show in CalendarPlanning.Weekend(myFilteredAirshows, saturday).OrderBy(s => s.Status).ThenBy(s => s.date_start)) lstBoxShows.Items.Add(show);
        labelWeekendSelected.Text = $"Weekend {saturday:MMM d}–{saturday.AddDays(1):MMM d}: {lstBoxShows.Items.Count} shows";
    }
}
