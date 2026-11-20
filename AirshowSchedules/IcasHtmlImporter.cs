using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace AirshowSchedules;

// ICAS exports an HTML table with a .doc extension, rather than a binary Word file.
internal static class IcasHtmlImporter
{
    private static MatchCollection Matches(string text, string pattern) =>
        Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline, TimeSpan.FromSeconds(5));

    private static string[] Lines(string html)
    {
        string text = Regex.Replace(html, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, "<[^>]*>", "");
        return WebUtility.HtmlDecode(text).Replace('\u00a0', ' ').Split('\n')
            .Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
    }

    internal static (List<Airshow> Shows, List<cContact> Contacts) Parse(string html, int year)
    {
        var shows = new List<Airshow>();
        var contacts = new List<cContact>();
        bool headerFound = false;
        int row = 0;
        foreach (Match match in Matches(html, @"<tr\b[^>]*>(.*?)</tr\s*>"))
        {
            row++;
            var cells = Matches(match.Groups[1].Value, @"<t[dh]\b[^>]*>(.*?)</t[dh]\s*>")
                .Cast<Match>().Select(m => Lines(m.Groups[1].Value)).ToArray();
            if (!headerFound)
            {
                if (cells.Length == 5 && string.Join(" ", cells[0]) == "Air Show Dates"
                    && string.Join(" ", cells[1]) == "Name" && string.Join(" ", cells[2]) == "Location"
                    && string.Join(" ", cells[3]) == "Performers" && string.Join(" ", cells[4]) == "Contact")
                    headerFound = true;
                continue;
            }
            if (cells.Length != 5 || cells[0].Length is < 1 or > 2)
                throw new FormatException($"ICAS row {row}: expected five columns and one or two dates.");
            var dates = new List<DateTime>();
            foreach (string date in cells[0])
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
                    throw new FormatException($"ICAS row {row}: invalid date '{date}'.");
                dates.Add(parsed);
            }
            if (dates.Any(d => d.Year != year) || dates.Last() < dates[0])
                throw new FormatException($"ICAS row {row}: dates do not match year {year}, or finish precedes start.");
            if (cells[1].Length == 0 || cells[2].Length == 0)
                throw new FormatException($"ICAS row {row}: missing show name or location.");
            var show = new Airshow
            {
                ID = shows.Count,
                date_start = dates[0].ToString("yyyy-MM-dd"),
                date_finish = dates.Last().ToString("yyyy-MM-dd"),
                date_added = DateTime.Now.ToString("yyyy-MM-dd"),
                name_airshow = string.Join(" ", cells[1])
            };
            // Keep the full international location in the serialized state field.
            string location = string.Join(" ", cells[2]);
            int comma = location.IndexOf(',');
            show.location.city = comma < 0 ? location : location[..comma].Trim();
            show.location.state = comma < 0 ? "" : location[(comma + 1)..].Trim();
            show.Performers.performer.AddRange(cells[3].Select(s => Regex.Replace(s, @"^-\s*", "")));
            if (cells[4].Length > 0)
            {
                var contact = new cContact { ID = contacts.Count, name = cells[4][0], phone = string.Join(" / ", cells[4].Skip(1)), address = "" };
                if (contact.name.Contains('@')) contact.emailAddresses.Add(contact.name);
                contact.showIDs.Add(show.ID);
                contacts.Add(contact);
            }
            shows.Add(show);
        }
        if (!headerFound || shows.Count == 0)
            throw new FormatException("No ICAS calendar rows found. Expected Air Show Dates, Name, Location, Performers, Contact.");
        return (shows, contacts);
    }
}
