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

namespace AirshowSchedules
{
    public partial class frmAirshowScheduleTool : Form
    {

        FormState myFormState = new FormState();
        //This is the master list
        List<Airshow> myAirshows = new List<Airshow>();
        //This is the filter list and used for display. It is a subset of myAirshows, so watch out for that.
        List<Airshow> myFilteredAirshows = new List<Airshow>();
        List<Airshow> myMergedShows = new List<Airshow>();
        cAirshowFileParserSetupTool WorkingFileParserClass = new cAirshowFileParserSetupTool();
        cAirshowScheduleCompare myASGCompare = new cAirshowScheduleCompare();
        AirshowSchedules.Regions myRegions = new AirshowSchedules.Regions();
        private System.Windows.Forms.TextBox TextBox1;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        public frmAirshowScheduleTool()
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
                //AirshowGroup asg = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(FileName);
                bool success;
                AirshowGroup asg = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase, out success);
                if (!success)
                {
                    MessageBox.Show("Error loading form: " + "Unable to load the active database");
                    return;
                }

                

                myAirshows = asg.Airshows.myShows;
                //myFilteredAirshows = myAirshows.ToList();
                myFormState.AirshowYearofInterest = asg.AirshowYearOfInterest;
                lblYearOfInterest.Text = $"Airshow Year of Interest: {asg.AirshowYearOfInterest.ToString()} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
                LoadGrid(myFormState.AirshowYearofInterest);
                //GridTools.LoadShowGrid(dataGridViewShows, myFormState.AirshowYearofInterest);
                myFilteredAirshows = myAirshows.ToList();
                ColorGrid(myFilteredAirshows);

                myRegions = Regions.LoadMe(myFormState.fnRegions);
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

        public class cAirshowScheduleCompare
        {
            [Display(DisplayName = "Choose an Input File Left (eg working database):")]
            [FileBrowseDialog(OpenFileDialogFilter = "Airshow Group (*.asg.xml)|*.asg.xml")]
            public string sFileNameLeft = "";


            [Display(DisplayName = "Choose an Input File Right (eg freshly downloaded data):")]
            [FileBrowseDialog(OpenFileDialogFilter = "Airshow Group (*.asg.xml)|*.asg.xml")]
            public string sFileNameRight = "";

            public static cAirshowScheduleCompare LoadMe()
            {
                string fng = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowCompare");

                cAirshowScheduleCompare fs = new cAirshowScheduleCompare();

                try
                {
                    if (System.IO.File.Exists(fng))
                        fs = Electroimpact.XmlSerialization.Serializer.Load<cAirshowScheduleCompare>(fng);

                    return fs;
                }
                catch { return fs; }
            }

            public static void SaveMe(cAirshowScheduleCompare fs)
            {
                string fng = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowCompare");

                Electroimpact.XmlSerialization.Serializer.Save(fs, fng);
            }

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
            SaveAirshowSchedule(DoFileDialogue, myAirshows);
            //update myFilteredAirshows
            btnFilterShows_Click(null, null);
        }

        private void SaveAirshowSchedule(bool DoFileDialogue, List<Airshow> airshows)
        {
            AirshowGroup asg = new AirshowGroup();
            asg.Airshows.myShows = airshows;
            asg.AirshowYearOfInterest = myFormState.AirshowYearofInterest;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.asg.XML|*.asg.xml";
            sfd.Title = "Save an Airshow Group";

            string fnCurrentWorkingAirshow = myFormState.fnCurrentXMLDataBase;

            if (DoFileDialogue || fnCurrentWorkingAirshow == "" || !File.Exists(fnCurrentWorkingAirshow))
            {
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Electroimpact.XmlSerialization.Serializer.Save(asg, sfd.FileName);
                }
            }
            else
            {
                Electroimpact.XmlSerialization.Serializer.Save(asg, fnCurrentWorkingAirshow);
            }
        }
        #endregion



        #region Form Event Callbacks


        private DialogResult SelectFilesToCompare(string fnActvDB = null)
        {
            myASGCompare = cAirshowScheduleCompare.LoadMe();

            if (!String.IsNullOrEmpty(fnActvDB))
                myASGCompare.sFileNameLeft = fnActvDB;
            Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(myASGCompare);

            DialogResult dr = sb.showDialog();
            cAirshowScheduleCompare.SaveMe(myASGCompare);
            return dr;
        }

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

                        List<Airshow> airshowsthisweek = myAirshows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
                        airshowsthisweek = myFilteredAirshows.Where(x => x.WeekNumber == asw.weekofyear).ToList();

                        List<Airshow> mergedAirshowsThisweek = myMergedShows.Where(x => x.WeekNumber == asw.weekofyear).ToList();

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
                Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(theAirshow);
                DialogResult dr = sb.showDialog();
                if (dr == DialogResult.OK)
                {
                    SaveAirshowSchedule(false);
                    ColorGrid(myFilteredAirshows);
                }
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
            Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(ashow);
            DialogResult dr = sb.showDialog();
            if (dr == DialogResult.OK)
            {
                myAirshows.Add(ashow);
                myFilteredAirshows.Add(ashow);
                SaveAirshowSchedule(false);
                LoadGrid(myFormState.AirshowYearofInterest);
                ColorGrid(myFilteredAirshows);
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

            foreach (Airshow airshow in myAirshows)
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
            List<Airshow> CallReport = myAirshows.Where(x => x.Status == eStatus.pursue || x.Status == eStatus.maybe).ToList();

            CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

            List<string> calltheseguys = new List<string>();
            calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");
            foreach (Airshow ashow in CallReport)
            {
                string gettowork = $"{ashow.date_start.ToString()}\t{ashow.Status.ToString()}\t{ashow.name_airshow}\t{ashow.location.ToString()}\t{ashow.Notes_AirshowStuff}";
                foreach (cContact contact in ashow.Contacts.contact)
                {
                    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
                }

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
            List<Airshow> CallReport = myAirshows.Where(x => x.Status == eStatus.contract).ToList();

            CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

            List<string> calltheseguys = new List<string>();
            calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");

            foreach (Airshow ashow in CallReport)
            {
                string gettowork = $"{ashow.date_start.ToString()}\t{ashow.Status.ToString()}\t{ashow.name_airshow}\t{ashow.location.ToString()}\t{ashow.Notes_AirshowStuff}";
                foreach (cContact contact in ashow.Contacts.contact)
                {
                    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
                }

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
            List<Airshow> CallReport = myAirshows.Where(x => x.Status == eStatus.pursue || x.Status == eStatus.verbal || x.Status == eStatus.NO || x.Status == eStatus.contract || x.Status == eStatus.maybe).ToList();

            CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

            List<string> calltheseguys = new List<string>();
            foreach (Airshow ashow in CallReport)
            {
                string gettowork = $"{ashow.date_start.ToString()}\t{ashow.Status.ToString()}\t{ashow.name_airshow}\t{ashow.Notes_AirshowStuff}";
                foreach (cContact contact in ashow.Contacts.contact)
                {
                    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
                }

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
                foreach (cContact contact in ashow.Contacts.contact)
                {
                    gettowork = $"{gettowork}\t{contact.name}\t{contact.phone}\t{contact.address}";
                }

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
                    myAirshows.Remove(ashow);
                    myFilteredAirshows.Remove(ashow);
                    SaveAirshowSchedule(false);
                    LoadGrid(myFormState.AirshowYearofInterest);
                    ColorGrid(myFilteredAirshows);
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
                                if (ashow.Contacts.contact.Count > 0)
                                {
                                    foreach (cContact contact in ashow.Contacts.contact)
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

            //System.Windows.Forms.Button cancelButton = new System.Windows.Forms.Button();
            //cancelButton.Text = "Cancel";
            //cancelButton.DialogResult = DialogResult.Cancel;
            //cancelButton.Location = new Point(250, 1200);
            //cancelButton.Height = 50;
            //Controls.Add(cancelButton);
        }
    }

    #endregion
}