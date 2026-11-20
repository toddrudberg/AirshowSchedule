using AirshowSchedules;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
}
var path = Path.GetFullPath(args[0]);
var shows = new List<Airshow>();
var contacts = new List<cContact>();
Airshow.LoadFile(new formMain.cAirshowFileParserSetupTool { sFileName = path, AirshowYear = 2026 }, shows, contacts);
int sourceCount = Regex.Matches(File.ReadAllText(path), @"<tr><td style=""white-space: nowrap;"">2026-").Count;
Check(shows.Count == sourceCount, "Source row count differs from import.");
Check(shows.First().date_start == "2026-01-24" && shows.First().Days == 1, "Single-day event failed.");
Check(shows.Last().name_airshow == "Vintage Weekend Ocean Reef Air Show", "Last event missing.");
Check(shows.Last().date_finish == "2026-12-05", "Last event finish incorrect.");
Check(shows.Any(s => s.name_airshow.Contains("(CANCELLED)")), "Cancellation label lost.");
Check(shows.Any(s => s.location.state.Contains("Australia")), "International location lost.");
Check(contacts.All(c => shows.Any(s => s.ID == c.showIDs.Single())), "Broken contact link.");
Check(contacts.All(c => !string.IsNullOrWhiteSpace(c.name)), "Empty contact generated.");
Check(shows.Any(s => s.name_airshow.Contains("& AOPA")), "Ampersand lost.");
UpdateChecks.Run(shows, contacts);
PlanningChecks.Run();
MergeChecks.Run();
MaintenanceChecks.Run();
var serializer = new XmlSerializer(typeof(AirshowGroup));
using var stream = new MemoryStream();
serializer.Serialize(stream, new AirshowGroup { AirshowYearOfInterest = 2026, Airshows = new AirshowGroup.cAirshows { myShows = shows } });
stream.Position = 0;
var roundtrip = (AirshowGroup)serializer.Deserialize(stream);
Check(roundtrip.GetAirshows().Count == shows.Count, "XML compatibility failed.");
try
{
    Airshow.LoadFile(new formMain.cAirshowFileParserSetupTool { sFileName = path, AirshowYear = 2025 }, new(), new());
    throw new Exception("Wrong year accepted.");
}
catch (FormatException) { }
string fixture = Path.GetTempFileName();
try
{
    File.WriteAllText(fixture, "2026-01-24\n2026-01-24\nLegacy Show\nTampa, FL\n- Performer\nContact\n12345\n");
    var legacy = new List<Airshow>();
    Airshow.LoadFile(new formMain.cAirshowFileParserSetupTool { sFileName = fixture, AirshowYear = 2026 }, legacy, new());
    Check(legacy.Count == 1, "Legacy final record missing.");
    File.WriteAllText(fixture, "<table><tr><td>Air Show Dates</td><td>Name</td><td>Location</td><td>Performers</td><td>Contact</td></tr><tr><td>2026-99-99</td><td>Show</td><td>City</td><td></td><td></td></tr></table>");
    try
    {
        Airshow.LoadFile(new formMain.cAirshowFileParserSetupTool { sFileName = fixture, AirshowYear = 2026 }, new(), new());
        throw new Exception("Malformed date accepted.");
    }
    catch (FormatException) { }
}
finally { File.Delete(fixture); }
if (args.Length > 1)
{
    string output = Path.GetFullPath(args[1]);
    Directory.CreateDirectory(Path.GetDirectoryName(output));
    Check(!File.Exists(output), "Refusing to overwrite an existing import.");
    string contactOutput = Path.ChangeExtension(Path.ChangeExtension(output, null), ".contacts.json");
    Check(!File.Exists(contactOutput), "Refusing to overwrite existing contacts.");
    using (var writer = File.Create(output))
        serializer.Serialize(writer, new AirshowGroup { AirshowYearOfInterest = 2026, Airshows = new AirshowGroup.cAirshows { myShows = shows } });
    File.WriteAllText(contactOutput, Newtonsoft.Json.JsonConvert.SerializeObject(contacts, Newtonsoft.Json.Formatting.Indented));
    System.Console.WriteLine($"Saved separate import: {output}");
}
System.Console.WriteLine($"PASS: {shows.Count} source rows, {contacts.Count} contacts; single-day, final row, cancellations, international locations, XML roundtrip, wrong year, malformed date and legacy import.");
