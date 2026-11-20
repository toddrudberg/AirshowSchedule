namespace AirshowSchedules;

internal static class ScreenLayout
{
    internal static void FitOnLoad(Form form)
    {
        form.Load += (_, _) => Fit(form);
        form.DpiChanged += (_, _) => Fit(form);
    }

    internal static void Fit(Form form)
    {
        var area = Screen.FromControl(form.Owner ?? form).WorkingArea;
        form.MinimumSize = new Size(Math.Min(form.MinimumSize.Width, area.Width), Math.Min(form.MinimumSize.Height, area.Height));
        form.Size = new Size(Math.Min(form.Width, area.Width), Math.Min(form.Height, area.Height));
        form.Location = new Point(Math.Clamp(form.Left, area.Left, area.Right - form.Width),
            Math.Clamp(form.Top, area.Top, area.Bottom - form.Height));
    }

    internal static void ConfigureScrollable(Form form)
    {
        // Preserve the existing editor's content extent when the viewport is smaller.
        form.AutoScroll = true;
        form.AutoScrollMinSize = form.ClientSize;
        FitOnLoad(form);
    }
}
