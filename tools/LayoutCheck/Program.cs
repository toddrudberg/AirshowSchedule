using AirshowSchedules;
using System.Reflection;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var main = new formMain();
        main.CreateControl();
        foreach (var size in new[] { new Size(1450, 850), new Size(1024, 600), new Size(800, 600), new Size(600, 800), new Size(480, 420), new Size(1450, 850) })
        {
            main.ClientSize = size;
            Layout(main);
            var layout = (TableLayoutPanel)Field(main, "planningLayout");
            var calendar = (Control)Field(main, "dgvCalendar");
            var sidebar = layout.Controls.Cast<Control>().Single(c => c != calendar);
            Check(!calendar.Bounds.IntersectsWith(sidebar.Bounds), $"Calendar/sidebar overlap at {size}");
            foreach (string name in new[] { "dgvCalendar", "lstBoxShows", "chklstRegions", "btnAddShow", "btnDeleteShow", "btnFilterSetAll", "btnFilterClearAll", "btnFilterShows" })
            {
                var control = (Control)Field(main, name);
                var origin = Point.Empty;
                for (Control current = control; current != main; current = current.Parent!)
                    origin.Offset(current.Location);
                var bounds = new Rectangle(origin, control.Size);
                Check(bounds.Width > 0 && bounds.Height > 0 && main.ClientRectangle.Contains(bounds), $"{name} outside viewport at {size}: {bounds}");
                Check(control.Parent.ClientRectangle.Contains(control.Bounds), $"{name} clipped by parent at {size}: {control.Bounds}");
            }
        }
        using var editor = new AirshowEditForm(new Airshow(), new());
        editor.ClientSize = new Size(300, 250);
        Layout(editor);
        Check(editor.AutoScroll && editor.AutoScrollMinSize.Height > editor.ClientSize.Height, "Small editor lost scrollable content");
        var settings = new formMain.cAirshowFileParserSetupTool { sFileName = "unchanged", AirshowYear = 2026 };
        var type = typeof(formMain).Assembly.GetType("AirshowSchedules.FormImportSetup")!;
        using var import = (Form)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { settings }, null)!;
        import.ClientSize = new Size(400, 250);
        Layout(import);
        Check(settings.sFileName == "unchanged" && settings.AirshowYear == 2026, "Opening import changed settings");
        Check(!typeof(formMain).Assembly.GetReferencedAssemblies().Any(a => a.Name == "Electroimpact.SettingsFormBuilder"), "Legacy generated UI still referenced");
        System.Console.WriteLine("PASS: wide, narrow, portrait, small window and resize-back layouts; editor scrolling; import settings; legacy UI removal.");
    }

    private static object Field(object instance, string name) => instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(instance)!;
    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    private static void Layout(Control control) { control.PerformLayout(); foreach (Control child in control.Controls) Layout(child); control.PerformLayout(); }
}
