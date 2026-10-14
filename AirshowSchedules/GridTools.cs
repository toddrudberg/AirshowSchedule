using System.Globalization;

namespace AirshowSchedules;

public partial class formMain
{
    private Font? calendarBoldFont;
    private Font? calendarRegularFont;
    private void LoadGrid(int year)
    {
        dgvCalendar.DataSource = null;
        dgvCalendar.Rows.Clear();
        dgvCalendar.Columns.Clear();
        dgvCalendar.ReadOnly = true;
        dgvCalendar.RowHeadersVisible = true;
        dgvCalendar.RowHeadersWidth = 85 * DeviceDpi / 96;
        dgvCalendar.ShowCellToolTips = true;
        dgvCalendar.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvCalendar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
        for (int i = 0; i < 5; i++) dgvCalendar.Columns.Add($"Weekend{i}", $"Weekend {i + 1}");
        dgvCalendar.Rows.Add(12);
        for (int month = 1; month <= 12; month++)
        {
            var row = dgvCalendar.Rows[month - 1];
            row.HeaderCell.Value = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
            var saturdays = cCalenderYear.GetSaturdaysList(year).Where(w => w.Date.Month == month).Select(w => w.Date).ToList();
            if (month == 1 && new DateTime(year, 1, 1).DayOfWeek == DayOfWeek.Sunday)
                saturdays.Insert(0, new DateTime(year - 1, 12, 31));
            for (int index = 0; index < saturdays.Count; index++) row.Cells[index].Tag = saturdays[index];
            foreach (DataGridViewCell cell in row.Cells)
                if (cell.Tag == null) { cell.Style.BackColor = Color.Gainsboro; cell.Value = ""; }
        }
        SizeCalendarRows();
    }

    private void SizeCalendarRows()
    {
        dgvCalendar.RowHeadersWidth = 85 * DeviceDpi / 96;
        foreach (DataGridViewRow row in dgvCalendar.Rows) row.Height = Math.Max(48 * DeviceDpi / 96, (dgvCalendar.Height - dgvCalendar.ColumnHeadersHeight - 4) / 12);
        foreach (DataGridViewColumn column in dgvCalendar.Columns) column.Width = Math.Max(90 * DeviceDpi / 96, (dgvCalendar.Width - dgvCalendar.RowHeadersWidth - 20) / 5);
    }

    private void ColorGrid(List<Airshow> visibleShows)
    {
        calendarRegularFont ??= new Font(dgvCalendar.Font, FontStyle.Regular);
        calendarBoldFont ??= new Font(dgvCalendar.Font, FontStyle.Bold);
        var all = CalendarPlanning.ForYear(myAirshowGroup.GetAirshows(), myAirshowGroup.AirshowYearOfInterest);
        foreach (DataGridViewRow row in dgvCalendar.Rows)
        foreach (DataGridViewCell cell in row.Cells)
        {
            if (cell.Tag is not DateTime saturday) continue;
            var weekend = CalendarPlanning.Weekend(visibleShows, saturday);
            var weekdays = CalendarPlanning.NearbyWeekdays(visibleShows, saturday);
            var commitments = CalendarPlanning.Weekend(all, saturday).Where(CalendarPlanning.Booked).OrderBy(s => s.Status).ToList();
            var interested = weekend.Where(s => s.Status is Airshow.eStatus.pursue or Airshow.eStatus.maybe).OrderBy(s => s.Status).ToList();
            var primary = commitments.FirstOrDefault() ?? interested.FirstOrDefault();
            cell.Style.BackColor = primary == null ? Color.White : CalendarPlanning.StatusColor(primary.Status);
            cell.Style.Font = weekend.Count > 0 || commitments.Count > 0 ? calendarBoldFont : calendarRegularFont;
            string text = $"{saturday:MMM d}–{saturday.AddDays(1):MMM d}";
            if (weekend.Count > 0)
                text += $"\n{weekend.Count} show{(weekend.Count == 1 ? "" : "s")}" + (weekdays.Count > 0 ? $" · {weekdays.Count} weekday" : "");
            else if (weekdays.Count > 0) text += $"\nWeekday only: {weekdays.Count}";
            if (commitments.Count > 1) text += $"\n{commitments.Count} commitments";
            else if (primary != null)
            {
                string title = $"{CalendarPlanning.Status(primary)}: {primary.name_airshow}";
                string shortened = title;
                while (shortened.Length > 1 && TextRenderer.MeasureText(shortened, cell.Style.Font).Width > cell.Size.Width - 24) shortened = shortened[..^1];
                if (shortened != title) shortened = shortened.TrimEnd() + "…";
                text += "\n" + shortened;
            }
            cell.Value = text;
            cell.ToolTipText = $"Weekend {saturday:MMM d}–{saturday.AddDays(1):MMM d}\n" +
                string.Join("\n", weekend.Select(s => $"{CalendarPlanning.Status(s)}: {s.name_airshow} ({s.date_start}–{s.date_finish})")) +
                (weekdays.Count > 0 ? "\n\nWeekday-only events (do not occupy this weekend):\n" + string.Join("\n", weekdays.Select(s => $"{s.name_airshow}: {s.date_start}–{s.date_finish}")) : "") +
                (commitments.Count > 0 ? "\n\nCommitments across all regions:\n" + string.Join("\n", commitments.Select(s => $"{CalendarPlanning.Status(s)}: {s.name_airshow}")) : "");
        }
        lstBoxShows.Invalidate();
        RefreshAttentionCount();
    }
}
