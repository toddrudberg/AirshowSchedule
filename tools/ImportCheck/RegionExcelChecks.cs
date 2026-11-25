using System.Reflection;
using AirshowSchedules;
using ClosedXML.Excel;

internal static class RegionExcelChecks
{
    internal static void Run(string path)
    {
        var group = new AirshowGroup { AirshowYearOfInterest = 2027 };
        group.GetAirshows().AddRange(new[]
        {
            Show(1, "Late Sample Airshow", "2027-11-01", "WA", Airshow.eStatus.contract),
            Show(2, "Mountain Sample Airshow", "2027-08-21", "CO", Airshow.eStatus.none),
            Show(3, "International Sample Airshow", "2027-07-17", "BC", Airshow.eStatus.NO),
            Show(4, "Harbor Practice", "2027-07-24", "WA", Airshow.eStatus.contract),
            Show(5, "=Early Sample Airshow", "2027-6-9", "WA", Airshow.eStatus.maybe),
            Show(6, "Previous Year Sample Airshow", "2026-08-01", "WA", Airshow.eStatus.contract),
            Show(7, "Ridge Sample Fly-In", "2027-08-22", "CO", Airshow.eStatus.pursue),
            Show(8, "Seaside Sample Airshow", "2027-09-18", "BC", Airshow.eStatus.verbal)
        });
        var regions = new Regions { myRegions = new() { ["WA"] = "NORTHWEST", ["CO"] = "MOUNTAIN", ["NV"] = "SOUTHWEST" } };
        using var search = new formSearch(group, new(), regions);
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        search.GetType().GetMethod("formSearch_Load", flags)!.Invoke(search, new object[] { search, EventArgs.Empty });
        ((TextBox)search.Controls.Find("txtShowName", true).Single()).Text = "Sample";
        var results = ((ListBox)search.Controls.Find("lstAirshows", true).Single()).Items.Cast<Airshow>().ToArray();
        Check(results.Select(s => s.ID).SequenceEqual(new[] { 1, 2, 3, 5, 7, 8 }), "Search results ignored the year, text filter, or international region.");
        Check(search.Controls.Find("btnExportExcel", true).Single().Enabled, "The export button is unavailable.");

        RegionExcelExport.Save(path, results, regions);
        using var workbook = new XLWorkbook(path);
        Check(workbook.Worksheets.Select(sheet => sheet.Name).SequenceEqual(new[] { "Shows with Status", "MOUNTAIN", "NORTHWEST", "SOUTHWEST", "Other - international" }),
            "Unexpected region tabs.");
        var status = workbook.Worksheet(1);
        var statusRows = status.RowsUsed().Skip(1).ToList();
        Check(statusRows.Select(row => row.Cell(3).GetString()).SequenceEqual(
            new[] { "=Early Sample Airshow", "Ridge Sample Fly-In", "Seaside Sample Airshow", "Late Sample Airshow" }) &&
            statusRows.Select(row => row.Cell(6).GetString()).SequenceEqual(new[] { "Maybe", "Pursue", "Verbal", "Contract" }),
            "The first tab should list only shows with qualifying statuses, in date order.");
        var northwest = workbook.Worksheet("NORTHWEST");
        Check(northwest.Cell("A2").GetDateTime() == new DateTime(2027, 6, 9) && northwest.Cell("A3").GetDateTime() == new DateTime(2027, 11, 1),
            "The start dates were not stored as dates in chronological order.");
        Check(northwest.Cell("C2").GetString() == "=Early Sample Airshow" && !northwest.Cell("C2").HasFormula &&
            northwest.Cell("C3").GetString() == "Late Sample Airshow" && northwest.LastRowUsed()!.RowNumber() == 3,
            "The workbook lost or added search results, or treated text as a formula.");
        Check(workbook.Worksheet("MOUNTAIN").Cell("C2").GetString() == "Mountain Sample Airshow" &&
            workbook.Worksheet("Other - international").Cell("C2").GetString() == "International Sample Airshow" &&
            workbook.Worksheet("SOUTHWEST").LastRowUsed()!.RowNumber() == 1, "A show was assigned to the wrong region.");
        Console.WriteLine($"PASS: Advanced Search filters and regional workbook. {Path.GetFullPath(path)}");
    }

    private static Airshow Show(int id, string name, string date, string state, Airshow.eStatus status) =>
        new() { ID = id, name_airshow = name, date_start = date, date_finish = date, date_added = "2026-11-24",
            location = new Airshow.cLocation { city = "Sample City", state = state }, Status = status };

    private static void Check(bool passed, string message)
    {
        if (!passed) throw new Exception(message);
    }
}
