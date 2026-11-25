using System.Globalization;
using ClosedXML.Excel;

namespace AirshowSchedules;

internal static class RegionExcelExport
{
    private static readonly string[] Headings = { "Start date", "Finish date", "Show", "City", "State / province", "Status" };
    private static readonly int[] Widths = { 14, 14, 52, 23, 23, 15 };
    private static readonly string[] DateFormats = { "yyyy-MM-dd", "yyyy-M-d" };

    internal static void Save(string path, IEnumerable<Airshow> shows, Regions regions)
    {
        var results = shows.ToList();
        var groupedShows = results.ToLookup(show => regions.ForState(show.location.state));
        var tabRegions = regions.GetRegionList().Concat(groupedShows.Select(group => group.Key))
            .Where(region => !string.IsNullOrWhiteSpace(region) && region != Regions.OtherRegion)
            .Distinct().OrderBy(region => region, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(region => region, StringComparer.Ordinal).Append(Regions.OtherRegion);

        using var workbook = new XLWorkbook();
        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "History" };
        AddSheet(workbook, SheetName("Shows with Status", usedNames),
            results.Where(show => show.Status is Airshow.eStatus.contract or Airshow.eStatus.verbal
                or Airshow.eStatus.maybe or Airshow.eStatus.pursue));
        foreach (string region in tabRegions)
            AddSheet(workbook, SheetName(region, usedNames), groupedShows[region]);

        string target = Path.GetFullPath(path);
        string temp = Path.Combine(Path.GetDirectoryName(target)!, $".airshow-export-{Guid.NewGuid():N}.xlsx");
        try
        {
            workbook.SaveAs(temp);
            File.Move(temp, target, overwrite: true);
        }
        finally { File.Delete(temp); }
    }

    private static void AddSheet(XLWorkbook workbook, string name, IEnumerable<Airshow> shows)
    {
        var sheet = workbook.Worksheets.Add(name);
        for (int col = 1; col <= Headings.Length; col++)
        {
            sheet.Cell(1, col).Value = Headings[col - 1];
            sheet.Column(col).Width = Widths[col - 1];
        }
        var header = sheet.Range(1, 1, 1, Headings.Length);
        header.Style.Font.Bold = true;
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#142438");
        sheet.SheetView.FreezeRows(1);

        var rows = shows.Select(show => (Show: show, Start: ParseDate(show.date_start), Finish: ParseDate(show.date_finish)))
            .OrderBy(item => item.Start ?? DateTime.MaxValue).ThenBy(item => item.Finish ?? DateTime.MaxValue)
            .ThenBy(item => item.Show.name_airshow, StringComparer.CurrentCultureIgnoreCase).ThenBy(item => item.Show.ID);
        int row = 2;
        foreach (var item in rows)
        {
            var show = item.Show;
            WriteDate(sheet.Cell(row, 1), item.Start, show.date_start);
            WriteDate(sheet.Cell(row, 2), item.Finish, show.date_finish);
            sheet.Cell(row, 3).Value = show.name_airshow ?? "";
            sheet.Cell(row, 4).Value = show.location.city ?? "";
            sheet.Cell(row, 5).Value = show.location.state ?? "";
            sheet.Cell(row, 6).Value = CalendarPlanning.Status(show);
            row++;
        }
        if (row > 2)
            sheet.Range(1, 1, row - 1, Headings.Length).SetAutoFilter();
    }

    private static DateTime? ParseDate(string? value) =>
        DateTime.TryParseExact(value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;

    private static void WriteDate(IXLCell cell, DateTime? date, string? original)
    {
        if (date is { } parsed)
        {
            cell.Value = parsed;
            cell.Style.DateFormat.Format = "yyyy-mm-dd";
        }
        else cell.Value = original ?? "";
    }

    private static string SheetName(string region, HashSet<string> used)
    {
        string name = new string(region.Select(c => "[]:*?/\\".Contains(c) || char.IsControl(c) ? '-' : c).ToArray()).Trim(' ', '\'');
        if (name.Length == 0) name = "Region";
        for (int index = 1; ; index++)
        {
            string suffix = index == 1 ? "" : $" ({index})";
            int length = Math.Min(name.Length, 31 - suffix.Length);
            if (char.IsHighSurrogate(name[length - 1])) length--;
            string candidate = name[..length].TrimEnd(' ', '\'') + suffix;
            if (used.Add(candidate)) return candidate;
        }
    }
}
