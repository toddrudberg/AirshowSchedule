using Electroimpact.SettingsFormBuilderV2.Attributes;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using static AirshowSchedules.Airshow;
using static AirshowSchedules.cCalenderYear;
using Pastel;
using System.Diagnostics.Metrics;
using Markdig;
using Newtonsoft.Json;

namespace AirshowSchedules;

public partial class formMain : Form
{
    AirshowGroup myAirshowGroup = new AirshowGroup();
    List<cContact> myContacts = new List<cContact>();
    FormState myFormState = new FormState();

    //This is the filter list and used for display. It is a subset of myAirshowGroup.Airshows.myShows, so watch out for that.
    List<Airshow> myFilteredAirshows = new List<Airshow>();
    cAirshowFileParserSetupTool WorkingFileParserClass = new cAirshowFileParserSetupTool();

    AirshowSchedules.Regions myRegions = new AirshowSchedules.Regions();
    private System.Windows.Forms.TextBox TextBox1;

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    public formMain()
    {
        //if(!DesignMode)
        {
            try
            {
                InitializeComponent();
                this.Text = "Airshow Schedule Tool"; // Set the form title
                this.Shown += new EventHandler(frmAirshowScheduleTool_Shown); // Subscribe to the Shown event
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Designer: " + ex.Message);
            }
        }
    }
    private void frmAirshowScheduleTool_Load(object sender, EventArgs e)
    {
        AllocConsole();
        // Position the form in the top-right corner of the screen
        var screen = Screen.FromControl(this);
        this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Top);

        try
        {

            myFormState = FormState.LoadMe();
            bool success;

            myContacts = cContact.LoadMe(myFormState.fnContactDataBase, out success);
            if (!success)
            {
                MessageBox.Show("Error loading form: " + "Unable to load the contact database");
                return;
            }


            AirshowGroup asg = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase, out success);
            if (!success)
            {
                MessageBox.Show("Error loading form: " + "Unable to load the active database");
                return;
            }
            myAirshowGroup = asg;



            lblYearOfInterest.Text = $"Airshow Year of Interest: {asg.AirshowYearOfInterest.ToString()} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
            LoadGrid(myAirshowGroup.AirshowYearOfInterest);
            myFilteredAirshows = myAirshowGroup.Airshows.myShows.ToList();
            ColorGrid(myFilteredAirshows);

            // Get the execution directory
            string executionDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Combine the path with the filename
            string regionsFilePath = Path.Combine(executionDirectory, "Regions.XML");
            // Load the Regions.XML file
            myRegions = Regions.LoadMe(regionsFilePath);

            chklstRegions.Items.Clear();
            foreach (string rgn in myRegions.GetRegionList())
            {
                chklstRegions.Items.Add(rgn);
            }
            for (int ii = 0; ii < chklstRegions.Items.Count; ii++)
            {
                chklstRegions.SetItemChecked(ii, true);
            }
            arciveActiveDBToolStripMenuItem_Click(this, null);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading form: " + ex.Message);
        }
        toolTip1.Active = false;
    }

    private void frmAirshowScheduleTool_Shown(object sender, EventArgs e)
    {

        // Set the console to be tall and narrow
        SetConsoleSize(75, 75); // Adjust width and height here
                                // Set form size to a percentage of screen resolution
                                // Use a Timer to delay the call to SetConsoleSize

    }

    #region Helper Classes
    [Serializable]
    public class cSearchTerms
    {
        public string szSearchTerm = "";
        public enum eSearchField
        {
            ContactName,
            CityName,
            ShowName
        }

        public eSearchField SearchFiled;
    }


    [Serializable]
    public class cAirshowFileParserSetupTool
    {
        [Display(DisplayName = "Choose an Input File - not the xml file:")]
        [FileBrowseDialog(OpenFileDialogFilter = "Text File (.txt)|*.txt")]
        [XmlElement("FileName")]
        public string sFileName = "";

        [XmlAttribute]
        [Display(DisplayName = "Which Airshow Year")]
        public int AirshowYear = 2023;

        [XmlAttribute]
        [Display(DisplayName = "File Source")]
        public efilesource eFileSource;
        public enum efilesource
        {
            ICAS,
            AirshowStuff
        }


        public static void SaveMe(cAirshowFileParserSetupTool afpst)
        {
            string FileName = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowSchedulerConverterSetup");
            Electroimpact.XmlSerialization.Serializer.Save(afpst, FileName);
        }

        public static cAirshowFileParserSetupTool LoadMe()
        {
            string fng = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowSchedulerConverterSetup");
            cAirshowFileParserSetupTool afpst = new cAirshowFileParserSetupTool();
            if (System.IO.File.Exists(fng))
            {
                try
                {
                    afpst = Electroimpact.XmlSerialization.Serializer.Load<cAirshowFileParserSetupTool>(fng);

                }
                catch { }
            }
            return afpst;
        }
    }



    #endregion

    #region Sub Routines
    private int GetYearOfInterest()
    {
        myFormState = FormState.LoadMe();
        bool success;
        AirshowGroup asg = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase, out success);
        int yearofinterest = asg.AirshowYearOfInterest;
        return yearofinterest;
    }


    private void SaveAirshowSchedule(bool DoFileDialogue)
    {
        SaveAirshowSchedule(DoFileDialogue, myAirshowGroup);
        SaveContacts(DoFileDialogue);
        btnFilterShows_Click(null, null);
        ColorGrid(myAirshowGroup.Airshows.myShows);
        ColorGrid(myFilteredAirshows);
    }

    private void SaveContacts(bool DoFileDialogue)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "*.JSON|*.json";
        sfd.Title = "Save a Contact List";

        string fnCurrentWorkingContacts = myFormState.fnContactDataBase;

        if (DoFileDialogue || fnCurrentWorkingContacts == "" || !File.Exists(fnCurrentWorkingContacts))
        {
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Electroimpact.XmlSerialization.Serializer.Save(myContacts, sfd.FileName);
                cContact.SaveMe(myContacts, sfd.FileName);
            }
        }
        else
        {
            //Electroimpact.XmlSerialization.Serializer.Save(myContacts, fnCurrentWorkingContacts);
            cContact.SaveMe(myContacts, fnCurrentWorkingContacts);
        }
    }

    private void SaveAirshowSchedule(bool DoFileDialogue, AirshowGroup airshowGroup)
    {

        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "*.asg.XML|*.asg.xml";
        sfd.Title = "Save an Airshow Group";

        string fnCurrentWorkingAirshow = myFormState.fnCurrentXMLDataBase;

        if (DoFileDialogue || fnCurrentWorkingAirshow == "" || !File.Exists(fnCurrentWorkingAirshow))
        {
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Electroimpact.XmlSerialization.Serializer.Save(airshowGroup, sfd.FileName);
                AirshowGroup.SaveMe(airshowGroup, sfd.FileName);
            }
        }
        else
        {
            AirshowGroup.SaveMe(airshowGroup, fnCurrentWorkingAirshow);
            //Electroimpact.XmlSerialization.Serializer.Save(asg, fnCurrentWorkingAirshow);
        }
    }
    #endregion



    #region Form Event Callbacks


    private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        CellClickEvent();
    }
    private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        CellClickEvent();
    }

    private void CellClickEvent()
    {
        if (dgvCalendar.SelectedCells.Count > 0)
        {
            object cell = dgvCalendar.SelectedCells[0].Value;

            //if( cell is cAirshowWeekend)
            {
                //cAirshowWeekend weekend = (cAirshowWeekend)cell;
                //int weekofyear = weekend.weekofyear;
                string[] weekendinquestion = cell.ToString().Split(' ');

                lstBoxShows.Items.Clear();
                lstBoxShows.Text = "No airshows";
                dataGridViewShows.Columns.Clear();

                if (weekendinquestion.Length > 2)
                {
                    DateTime dateTime = new DateTime(GetYearOfInterest(), int.Parse(weekendinquestion[0]), int.Parse(weekendinquestion[2]));
                    AirshowWeekend asw = new AirshowWeekend(dateTime);

                    List<Airshow> airshowsthisweek = myAirshowGroup.Airshows.myShows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
                    airshowsthisweek = myFilteredAirshows.Where(x => x.WeekNumber == asw.weekofyear).ToList();

                    List<string> shownames = new List<string>();

                    foreach (Airshow airshow in airshowsthisweek)
                    {
                        shownames.Add(airshow.ToString());
                    }
                    if (shownames.Count > 0)
                    {
                        foreach (Airshow airshow in airshowsthisweek)
                        {
                            lstBoxShows.Items.Add(airshow);
                        }
                        LoadShowGrid(airshowsthisweek, $"Airshows week of {cell.ToString()}:");
                        //GridTools.LoadShowGrid(dataGridViewShows, airshowsthisweek, $"Airshows week of {cell.ToString()}:");
                    }
                }
            }
        }
    }

    private void dataGridView2_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
    {
        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
    }

    private void lstBoxShows_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstBoxShows.SelectedItems.Count > 0)
        {
            Airshow theAirshow = (Airshow)lstBoxShows.Items[lstBoxShows.SelectedIndex];

            using (AirshowEditForm editForm = new AirshowEditForm(theAirshow, myContacts))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // The Airshow object has been updated
                    SaveAirshowSchedule(false); // Save the updated airshow schedule
                }
            }


            // Airshow theAirshow = (Airshow)lstBoxShows.Items[lstBoxShows.SelectedIndex];
            // Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(theAirshow);
            // DialogResult dr = sb.showDialog();
            // if (dr == DialogResult.OK)
            // {
            //     SaveAirshowSchedule(false);
            //     //ColorGrid(myFilteredAirshows);
            // }
        }
    }

    private void dataGridViewShows_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        dataGridViewShows_CellClickHandler(sender, e);
    }

    private void dataGridViewShows_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        dataGridViewShows_CellClickHandler(sender, e);
    }

    private void dataGridViewShows_CellClickHandler(object sender, DataGridViewCellEventArgs e)
    {
        int rowIndex = -1; // Default value in case no cell is selected

        if (dataGridViewShows.SelectedCells.Count > 0)
        {
            rowIndex = dataGridViewShows.SelectedCells[0].RowIndex;
            lstBoxShows.SelectedIndex = rowIndex;
        }
    }

    private void btnAddShow_Click(object sender, EventArgs e)
    {
        Airshow ashow = new Airshow();
        int year = myAirshowGroup.AirshowYearOfInterest;
        //pick a date in year that is a saturday in july
        DateTime firstSaturdayInJuly = new DateTime(year, 7, 1);
        while (firstSaturdayInJuly.DayOfWeek != DayOfWeek.Saturday)
        {
            firstSaturdayInJuly = firstSaturdayInJuly.AddDays(1);
        }
        ashow.date_start = firstSaturdayInJuly.ToString("yyyy-MM-dd");
        ashow.date_finish = firstSaturdayInJuly.AddDays(1).ToString("yyyy-MM-dd");

        using (AirshowEditForm editForm = new AirshowEditForm(ashow, myContacts))
        {
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                // The Airshow object has been updated
                myAirshowGroup.Airshows.myShows.Add(ashow);
                myFilteredAirshows.Add(ashow);
                SaveAirshowSchedule(false); // Save the updated airshow schedule
            }
        }
    }


    private void btnFilterSetAll_Click(object sender, EventArgs e)
    {
        for (int ii = 0; ii < chklstRegions.Items.Count; ii++)
        {
            chklstRegions.SetItemChecked(ii, true);
        }
    }

    private void btnFilterClearAll_Click(object sender, EventArgs e)
    {
        for (int ii = 0; ii < chklstRegions.Items.Count; ii++)
        {
            chklstRegions.SetItemChecked(ii, false);
        }
    }

    private void chklstRegions_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void btnFilterShows_Click(object sender, EventArgs e)
    {
        List<Airshow> FilteredAirshows = new List<Airshow>();
        foreach (Airshow airshow in myAirshowGroup.Airshows.myShows)
        {
            string rgn = "";
            string state = airshow.location.state.ToUpper();
            try
            {
                rgn = myRegions.myRegions[state];
            }
            catch
            {
                continue;
            }

            for (int ii = 0; ii < chklstRegions.CheckedItems.Count; ii++)
            {
                if ((string)chklstRegions.CheckedItems[ii] == rgn)
                {
                    FilteredAirshows.Add(airshow);
                }
            }
        }
        myFilteredAirshows = FilteredAirshows.ToList();
        ColorGrid(myFilteredAirshows);
    }

    private void generateCallListToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<Airshow> CallReport = myAirshowGroup.Airshows.myShows.Where(x => x.Status == eStatus.pursue || x.Status == eStatus.maybe || x.Status == eStatus.verbal).ToList();

        CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

        List<string> calltheseguys = new List<string>();
        calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");
        foreach (Airshow ashow in CallReport)
        {
            string gettowork = $"{ashow.date_start.ToString()}\t{ashow.Status.ToString()}\t{ashow.name_airshow}\t{ashow.location.ToString()}\t{ashow.Notes_AirshowStuff}";
            //contacts
            //foreach (cContact contact in ashow.Contacts.contact)
            //{
            //    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
            //}

            calltheseguys.Add(gettowork);
        }
        showSearchResults(calltheseguys);
    }

    private void ShowDataInForm(string allthedata)
    {
        // Create a new form
        Form dataForm = new Form();
        dataForm.Text = "Call List";
        dataForm.Size = new Size(this.Width, (int)((double)this.Height * .75));
        dataForm.StartPosition = FormStartPosition.CenterParent;

        // Create a Label to inform the user
        Label infoLabel = new Label();
        infoLabel.Text = "The data will be copied to the Clipboard in Tab Delimted for display in Excel.";
        infoLabel.Font = new Font(infoLabel.Font.FontFamily, 12, FontStyle.Bold);
        infoLabel.Dock = DockStyle.Top;
        infoLabel.TextAlign = ContentAlignment.MiddleCenter;
        infoLabel.Height = 30;

        // Create a TextBox to display the data
        TextBox textBox = new TextBox();
        textBox.Multiline = true;
        textBox.ReadOnly = true;
        textBox.ScrollBars = ScrollBars.Vertical;
        textBox.Dock = DockStyle.Fill;
        textBox.Text = allthedata;

        // Create an OK button to close the form
        Button okButton = new Button();
        okButton.Text = "OK";
        okButton.Font = new Font(infoLabel.Font.FontFamily, 12, FontStyle.Bold);
        okButton.Dock = DockStyle.Bottom;
        okButton.Height = 40;
        okButton.Click += (sender, e) => dataForm.Close();

        // Add the Label, TextBox, and OK button to the form
        dataForm.Controls.Add(textBox);
        dataForm.Controls.Add(okButton);
        dataForm.Controls.Add(infoLabel);

        // Show the form as a modal dialog
        dataForm.ShowDialog();
    }

    private void generateBookedListToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<Airshow> CallReport = myAirshowGroup.Airshows.myShows.Where(x => x.Status == eStatus.contract).ToList();

        CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

        List<string> calltheseguys = new List<string>();
        calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");

        foreach (Airshow ashow in CallReport)
        {
            string gettowork = $"{ashow.date_start.ToString()}\t{ashow.Status.ToString()}\t{ashow.name_airshow}\t{ashow.location.ToString()}\t{ashow.Notes_AirshowStuff}";
            //contacts
            //foreach (cContact contact in ashow.Contacts.contact)
            //{
            //    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
            //}

            calltheseguys.Add(gettowork);
        }
        showSearchResults(calltheseguys);
    }

    private void showSearchResults(List<string> calltheseguys)
    {
        string allthedata = "";
        string fortextbox = "";
        foreach (string st in calltheseguys)
        {
            allthedata += st + "\n";
            fortextbox += st.Replace("\t", " ") + "\r\n";
        }
        // show in a message box:
        ShowDataInForm(fortextbox);
        // copy to clipboard:
        Clipboard.SetText(allthedata);
    }

    private void generateICASMailingListToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<Airshow> CallReport = myAirshowGroup.Airshows.myShows.Where(x => x.Status == eStatus.pursue || x.Status == eStatus.verbal || x.Status == eStatus.NO || x.Status == eStatus.contract || x.Status == eStatus.maybe).ToList();

        CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

        List<string> calltheseguys = new List<string>();
        foreach (Airshow ashow in CallReport)
        {
            string gettowork = $"{ashow.date_start.ToString()}\t{ashow.Status.ToString()}\t{ashow.name_airshow}\t{ashow.Notes_AirshowStuff}";
            //contacts
            //foreach (cContact contact in ashow.Contacts.contact)
            //{
            //    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
            //}

            calltheseguys.Add(gettowork);
        }
        showSearchResults(calltheseguys);
    }

    private void generateICASMailingListAllInRegionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<Airshow> CallReport = myFilteredAirshows.ToList();
        CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

        List<string> calltheseguys = new List<string>();
        calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");
        foreach (Airshow ashow in CallReport)
        {
            string gettowork = $"{ashow.date_start.ToString()}\t{ashow.Status.ToString()}\t{ashow.name_airshow}\t{ashow.location.ToString()}\t{ashow.Notes_AirshowStuff}";
            //contacts
            //foreach (cContact contact in ashow.Contacts.contact)
            //{
            //    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
            //}

            calltheseguys.Add(gettowork);
        }
        showSearchResults(calltheseguys);
    }

    private void btnDeleteShow_Click(object sender, EventArgs e)
    {
        int rowIndex = -1; // Default value in case no cell is selected

        if (dataGridViewShows.SelectedCells.Count > 0)
        {
            rowIndex = dataGridViewShows.SelectedCells[0].RowIndex;
            lstBoxShows.SelectedIndex = rowIndex;
            Airshow ashow = lstBoxShows.SelectedItem as Airshow;

            DialogResult dr = MessageBox.Show("Are you sure you want to remomove { " + ashow.ToString() + " } from the database?  There is no UNDO.", "Remove Airshow", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                myAirshowGroup.Airshows.myShows.Remove(ashow);
                myFilteredAirshows.Remove(ashow);
                SaveAirshowSchedule(false);
            }
        }
    }

    private void contactToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //MessageBox.Show("Who?", "Find This Person", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        cSearchTerms SearchTerms = new cSearchTerms();
        Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(SearchTerms);
        DialogResult dr = sb.showDialog();
        if (dr == DialogResult.OK)
        {
            List<Airshow> ret = new List<Airshow>();
            Clipboard.Clear();
            switch (SearchTerms.SearchFiled)
            {
                case cSearchTerms.eSearchField.ContactName:
                    {
                        foreach (Airshow ashow in myFilteredAirshows)
                        {
                            List<cContact> contacts = cContact.getContacts(myContacts, ashow);
                            if (contacts.Count > 0)
                            {
                                foreach (cContact contact in contacts)
                                {
                                    if (contact.name.ToLower().Contains(SearchTerms.szSearchTerm.ToLower()))
                                    {
                                        ret.Add(ashow);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case cSearchTerms.eSearchField.ShowName:
                    {
                        ret = myFilteredAirshows.Where(c => c.name_airshow.ToLower().Contains(SearchTerms.szSearchTerm.ToLower())).ToList();
                        break;
                    }
                case cSearchTerms.eSearchField.CityName:
                    {
                        ret = myFilteredAirshows.Where(c => c.location.city.ToLower().Contains(SearchTerms.szSearchTerm.ToLower())).ToList();
                        break;
                    }
                default:
                    break;
            }
            if (ret.Count > 0)
            {
                Clipboard.Clear();
                ret = ret.OrderBy(c => c.WeekNumber).ToList();
                string lines = Airshow.GetTabOutput(ret);
                Clipboard.SetText(lines);

                {
                    PopupForm puf = new PopupForm();
                    puf.TextBox1.Lines = Airshow.GetLines(ret);
                    puf.Width = 1100;
                    puf.Height = 1200;
                    puf.ShowDialog();
                }
            }
        }
    }

    private void setYearOfInterestToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int likelyYear = myAirshowGroup.AirshowYearOfInterest; // Get the likely year

        using (FormYearSelction yearSelectionForm = new FormYearSelction(likelyYear))
        {
            if (yearSelectionForm.ShowDialog() == DialogResult.OK)
            {
                int selectedYear = yearSelectionForm.SelectedYear;
                myAirshowGroup.AirshowYearOfInterest = selectedYear;
                FormState.SaveMe(myFormState);
                lblYearOfInterest.Text = $"Airshow Year of Interest: {selectedYear} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
                LoadGrid(myAirshowGroup.AirshowYearOfInterest);
                ColorGrid(myFilteredAirshows);
                SaveAirshowSchedule(false);
            }
        }
    }


    public class PopupForm : Form
    {
        public System.Windows.Forms.TextBox TextBox1 { get; private set; }

        public PopupForm()
        {
            ///InitializeComponent();

            // Add a label and textbox to the popup form
            Label label = new Label();
            label.Text = "Results:";
            label.Width = 250;
            label.Location = new Point(10, 10);
            Controls.Add(label);

            TextBox1 = new System.Windows.Forms.TextBox();
            TextBox1.Multiline = true;
            TextBox1.Width = 1000;
            TextBox1.Height = 1000;
            TextBox1.Location = new Point(10, 30);
            Controls.Add(TextBox1);

            // Add an OK and Cancel button
            System.Windows.Forms.Button okButton = new System.Windows.Forms.Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(10, 1050);
            okButton.Height = 50;
            Controls.Add(okButton);
        }
    }

    private void setActiveContactDBToolStripMenuItem_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "*.JSON|*.json";
        ofd.Title = "Open a Contact List";

        DialogResult dr = ofd.ShowDialog();
        if (dr == DialogResult.OK)
        {
            myFormState.fnContactDataBase = ofd.FileName;
            FormState.SaveMe(myFormState);
            myContacts = cContact.LoadMe(myFormState.fnContactDataBase, out bool success);
            if (!success)
            {
                MessageBox.Show("Error loading form: " + "Unable to load the contact database");
                return;
            }
            SaveContacts(false);
        }
    }


    //private void exportContactsToolStripMenuItem_Click(object sender, EventArgs e)
    //{
    //    //let's make a list of all the contacts
    //    List<cContact> allcontacts = new List<cContact>();
    //    int airshowID = 1;
    //    foreach (Airshow ashow in myAirshowGroup.Airshows.myShows)
    //    {
    //        foreach (cContact contact in ashow.Contacts.contact)
    //        {
    //            if ( contact.name == "" )
    //            {
    //                continue;
    //            }
    //            ashow.ID = airshowID;
    //            contact.showIds.Add(airshowID);
    //            allcontacts.Add(contact);
    //        }
    //        airshowID++;
    //    }
    //    //let's check for duplicates
    //    List<cContact> nodups = new List<cContact>();
    //    foreach (cContact contact in allcontacts)
    //    {
    //        bool found = false;
    //        foreach (cContact contact2 in nodups)
    //        {
    //            if (contact.name == contact2.name)
    //            {
    //                found = true;
    //                break;
    //            }
    //        }
    //        if (!found)
    //        {
    //            nodups.Add(contact);
    //        }
    //    }
    //    // for all of the nodups, let's create a unique id
    //    int id = 1;
    //    foreach (cContact contact in nodups)
    //    {
    //        contact.id = id++;
    //    }

    //    Console.WriteLine($"Found {allcontacts.Count} contacts and {nodups.Count} unique contacts".Pastel(Color.LimeGreen));

    //    //now we need to be sure to merge contact data:
    //    List<cContact> merged = new List<cContact>();
    //    foreach (cContact contact in nodups)
    //    {
    //        cContact newcontact = new cContact();
    //        newcontact.id = contact.id;
    //        newcontact.name = contact.name;
    //        newcontact.phone = contact.phone;
    //        newcontact.address = contact.address;
    //        newcontact.emailAddresses = contact.emailAddresses;
    //        newcontact.showIds = newcontact.showIds;
    //        foreach (Airshow ashow in myAirshowGroup.Airshows.myShows)
    //        {
    //            foreach (cContact contact2 in ashow.Contacts.contact)
    //            {
    //                if (contact2.name == contact.name)
    //                {
    //                    newcontact.showIds = newcontact.showIds.Union(contact2.showIds).ToList();
    //                    if (contact2.phone != "" && newcontact.phone == "")
    //                    {
    //                        newcontact.phone = contact2.phone;
    //                    }
    //                    if (contact2.address != "" && newcontact.address == "")
    //                    {
    //                        newcontact.address = contact2.address;
    //                    }
    //                    foreach (string email in contact2.emailAddresses)
    //                    {
    //                        if (!newcontact.emailAddresses.Contains(email))
    //                        {
    //                            newcontact.emailAddresses.Add(email);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        merged.Add(newcontact);
    //    }
    //    //output a json file
    //    string json = JsonConvert.SerializeObject(merged, Formatting.Indented);
    //    string FileName = "";

    //    SaveFileDialog sfd = new SaveFileDialog();
    //    sfd.Filter = "*.JSON|*.json";
    //    sfd.Title = "Save a Contact List";
    //    //open the file dialog
    //    DialogResult dr = sfd.ShowDialog();
    //    if (dr == DialogResult.OK)
    //    {
    //        FileName = sfd.FileName;
    //    }
    //    else
    //    {
    //        return;
    //    }

    //    //make sure the directory exists
    //    string dir = Path.GetDirectoryName(FileName);
    //    if (!Directory.Exists(dir))
    //    {
    //        Directory.CreateDirectory(dir);
    //    }
    //    System.IO.File.WriteAllText(FileName, json);
    //    Console.WriteLine($"Contacts have been exported to {FileName}".Pastel(Color.LimeGreen));


    //    //now let's assign the contacts to the shows
    //    foreach (Airshow ashow in myAirshowGroup.Airshows.myShows)
    //    {
    //        foreach (cContact contact in merged)
    //        {
    //            if (contact.showIds.Contains(ashow.ID))
    //            {
    //                ashow.contactIds.Add(contact.id);
    //            }
    //        }
    //    }

    //    //now let's save the airshow group as
    //    SaveAirshowSchedule(true);


    //    Console.WriteLine();
    //    //now for every contact, let's see if we can find a show they are associated with
    //    foreach (cContact contact in merged)
    //    {
    //        Console.WriteLine();
    //        List<Airshow> shows = myAirshowGroup.Airshows.myShows.Where(x => x.Contacts.contact.Contains(contact)).ToList();
    //        if (shows.Count > 0)
    //        {
    //            Console.WriteLine($"Contact: {contact.name} is associated with {shows.Count} shows".Pastel(Color.LimeGreen));
    //            int count = 0;
    //            foreach (Airshow ashow in shows)
    //            {
    //                Console.WriteLine($"\t\t{++count} - {ashow.name_airshow} in {ashow.location.city}, {ashow.location.state}".Pastel(Color.Yellow));
    //            }
    //        }
    //    }
    //}
}
    #endregion
