using System.Globalization;

namespace AirshowSchedules;

internal static class CalendarPlanning
{
    internal static bool Dates(Airshow show, out DateTime start, out DateTime finish)
    {
        bool validStart = DateTime.TryParseExact(show.date_start, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out start);
        bool validFinish = DateTime.TryParseExact(show.date_finish, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out finish);
        return validStart && validFinish && finish >= start;
    }

    internal static bool Booked(Airshow show) => show.Status is Airshow.eStatus.contract or Airshow.eStatus.verbal;
    internal static bool Cancelled(Airshow show) => (show.name_airshow ?? "").Contains("CANCELLED", StringComparison.OrdinalIgnoreCase);
    internal static List<Airshow> ForYear(IEnumerable<Airshow> shows, int year) => shows.Where(s =>
        Dates(s, out var start, out var finish) && start.Year <= year && finish.Year >= year).ToList();
    internal static bool WeekdayOnly(Airshow show)
    {
        if (!Dates(show, out var start, out var finish)) return false;
        int daysToSaturday = ((int)DayOfWeek.Saturday - (int)start.DayOfWeek + 7) % 7;
        return start.DayOfWeek != DayOfWeek.Sunday && start.AddDays(daysToSaturday) > finish;
    }
    internal static List<Airshow> Weekend(IEnumerable<Airshow> shows, DateTime saturday) => shows.Where(s =>
        Dates(s, out var start, out var finish) && start <= saturday.AddDays(1) && finish >= saturday).ToList();
    internal static List<Airshow> NearbyWeekdays(IEnumerable<Airshow> shows, DateTime saturday) => shows.Where(s =>
        WeekdayOnly(s) && Dates(s, out var start, out _) && start >= saturday.AddDays(-5) && start < saturday).ToList();
    internal static bool Overlap(Airshow a, Airshow b) => Dates(a, out var aStart, out var aFinish) &&
        Dates(b, out var bStart, out var bFinish) && aStart <= bFinish && bStart <= aFinish;
    internal static string Status(Airshow show) => show.Status switch
    {
        Airshow.eStatus.contract => "Contract", Airshow.eStatus.verbal => "Verbal", Airshow.eStatus.maybe => "Maybe",
        Airshow.eStatus.pursue => "Pursue", Airshow.eStatus.NO => "No", _ => "Unassigned"
    };
    internal static Color StatusColor(Airshow.eStatus status) => status switch
    {
        Airshow.eStatus.contract => Color.LightBlue, Airshow.eStatus.verbal => Color.LightGreen,
        Airshow.eStatus.maybe => Color.LightYellow, Airshow.eStatus.pursue => Color.LightCoral,
        Airshow.eStatus.NO => Color.LightGray, _ => Color.White
    };
}
