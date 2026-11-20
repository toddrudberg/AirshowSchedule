namespace AirshowSchedules;

internal sealed class FormImportSetup : Form
{
    internal FormImportSetup(formMain.cAirshowFileParserSetupTool settings)
    {
        Text = "Import calendar";
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(620, 260);
        StartPosition = FormStartPosition.CenterParent;
        ScreenLayout.FitOnLoad(this);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        var cancel = new Button { Text = "Cancel", AutoSize = true, DialogResult = DialogResult.Cancel };
        var import = new Button { Text = "Import", AutoSize = true };
        buttons.Controls.AddRange(new Control[] { cancel, import });
        CancelButton = cancel;
        AcceptButton = import;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, ColumnCount = 1, Padding = new Padding(12) };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        var fileRow = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
        fileRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        fileRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        var path = new TextBox { Dock = DockStyle.Fill, Text = settings.sFileName };
        var browse = new Button { Text = "Browse…", AutoSize = true };
        fileRow.Controls.Add(path, 0, 0);
        fileRow.Controls.Add(browse, 1, 0);
        browse.Click += (_, _) =>
        {
            using var picker = new OpenFileDialog { Filter = "ICAS Calendar (*.doc;*.html;*.htm;*.txt)|*.doc;*.html;*.htm;*.txt", CheckFileExists = true };
            if (picker.ShowDialog(this) == DialogResult.OK) path.Text = picker.FileName;
        };
        var year = new NumericUpDown { Minimum = 1, Maximum = 9999, Value = Math.Clamp(settings.AirshowYear, 1, 9999), Dock = DockStyle.Top };
        var source = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top, DataSource = Enum.GetValues<formMain.cAirshowFileParserSetupTool.efilesource>() };
        source.SelectedItem = settings.eFileSource;
        foreach (var control in new Control[] { new Label { Text = "Calendar file", AutoSize = true }, fileRow,
            new Label { Text = "Airshow year", AutoSize = true }, year, new Label { Text = "File source", AutoSize = true }, source })
            layout.Controls.Add(control);
        import.Click += (_, _) =>
        {
            if (!File.Exists(path.Text.Trim())) { MessageBox.Show(this, "Choose an existing calendar file.", Text); path.Focus(); return; }
            settings.sFileName = path.Text.Trim();
            settings.AirshowYear = (int)year.Value;
            settings.eFileSource = (formMain.cAirshowFileParserSetupTool.efilesource)source.SelectedItem!;
            DialogResult = DialogResult.OK;
        };
        Controls.Add(layout);
        Controls.Add(buttons);
    }
}
