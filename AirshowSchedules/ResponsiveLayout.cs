namespace AirshowSchedules;

public partial class formMain
{
    private TableLayoutPanel? planningLayout;
    private void InstallResponsiveLayout()
    {
        SuspendLayout();
        var header = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, Padding = new Padding(8, 2, 8, 2) };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (var link in new[] { linkLabelDB, linkLabelContacts })
        {
            link.AutoSize = false;
            link.Dock = DockStyle.Top;
            link.AutoEllipsis = true;
            link.Height = Font.Height + 8;
            header.Controls.Add(link);
        }
        statusLegend!.AutoSize = true;
        statusLegend.WrapContents = true;
        statusLegend.Dock = DockStyle.Top;
        header.Controls.Add(statusLegend);
        var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        foreach (var button in new[] { btnAddShow, btnDeleteShow }) { button.AutoSize = true; actions.Controls.Add(button); }
        var filters = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        foreach (var button in new[] { btnFilterSetAll, btnFilterClearAll, btnFilterShows }) { button.AutoSize = true; filters.Controls.Add(button); }
        var sidebar = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5, ColumnCount = 1, Padding = new Padding(4) };
        sidebar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        sidebar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        sidebar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
        sidebar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        labelWeekendSelected.AutoSize = true;
        foreach (var control in new Control[] { labelWeekendSelected, lstBoxShows, actions, chklstRegions, filters }) { control.Dock = DockStyle.Fill; sidebar.Controls.Add(control); }
        planningLayout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(4) };
        dgvCalendar.Dock = DockStyle.Fill;
        planningLayout.Controls.Add(dgvCalendar);
        planningLayout.Controls.Add(sidebar);
        Controls.Add(planningLayout);
        Controls.Add(header);
        menuStrip1.Dock = DockStyle.Top;
        menuStrip1.BringToFront();
        var area = Screen.FromControl(this).WorkingArea;
        Size = new Size(Math.Min(1450, area.Width), Math.Min(850, area.Height));
        MinimumSize = new Size(Math.Min(480, area.Width), Math.Min(420, area.Height));
        ScreenLayout.FitOnLoad(this);
        Resize += (_, _) => LayoutPlanningTools();
        dgvCalendar.SizeChanged += (_, _) => { SizeCalendarRows(); if (dgvCalendar.Rows.Count > 0) ColorGrid(myFilteredAirshows); };
        DpiChanged += (_, _) =>
        {
            calendarBoldFont?.Dispose(); calendarBoldFont = null;
            calendarRegularFont?.Dispose(); calendarRegularFont = null;
            LayoutPlanningTools();
        };
        LayoutPlanningTools();
        ResumeLayout(true);
    }

    private void LayoutPlanningTools()
    {
        if (planningLayout == null) return;
        bool narrow = ClientSize.Width < 1000 * DeviceDpi / 96;
        if (planningLayout.ColumnCount == (narrow ? 1 : 2)) return;
        planningLayout.SuspendLayout();
        planningLayout.ColumnStyles.Clear();
        planningLayout.RowStyles.Clear();
        planningLayout.ColumnCount = narrow ? 1 : 2;
        planningLayout.RowCount = narrow ? 2 : 1;
        planningLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, narrow ? 100 : 62));
        if (!narrow) planningLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        planningLayout.RowStyles.Add(new RowStyle(SizeType.Percent, narrow ? 55 : 100));
        if (narrow) planningLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
        var sidebar = planningLayout.Controls.Cast<Control>().Single(c => c != dgvCalendar);
        planningLayout.SetCellPosition(dgvCalendar, new TableLayoutPanelCellPosition(0, 0));
        planningLayout.SetCellPosition(sidebar, new TableLayoutPanelCellPosition(narrow ? 0 : 1, narrow ? 1 : 0));
        planningLayout.ResumeLayout(true);
    }
}
