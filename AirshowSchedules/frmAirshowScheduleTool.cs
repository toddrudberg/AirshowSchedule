using Electroimpact.SettingsFormBuilderV2.Attributes;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using static AirshowSchedules.Airshow;
using static AirshowSchedules.cCalenderYear;
using Pastel;
using System.Diagnostics.Metrics;

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

       

        public frmAirshowScheduleTool()
        {
            //if(!DesignMode)
            {
                try
                {
                    InitializeComponent();
                    AllocConsole();
                    // Set the console to be tall and narrow
                    SetConsoleSize(75, 75); // Adjust width and height here
                                            // Set form size to a percentage of screen resolution
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Designer: " + ex.Message);
                }
            }
        }
        private void frmAirshowScheduleTool_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {

                using (Graphics g = this.CreateGraphics())
                {
                    float dpiX = g.DpiX;
                    float dpiY = g.DpiY;

                    // Calculate the scale factor directly from the DPI
                    float scaleFactor = dpiX / 96.0f; // 96 DPI is the baseline for 100%
                    double m = (1 - .95) / (96 - 144);
                    double b = 1 - m * 96;
                    double scaleFactorX = m * dpiX + b;
                    m = (1 - 1.1) / (96 - 144);
                    b = 1 - m * 96;
                    double scaleFactorY = m * dpiY + b;

                    //when dpiX = 144 we need scaleFactor * .95, when dpiX = 96, we need scaleFactor * 1:
                    // Apply scaling using the formula above



                    // Apply scaling
                    int formWidth = (int)(1450.0 * scaleFactor * scaleFactorX);
                    int formHeight = (int)(770.0 * scaleFactor * scaleFactorY);

                    this.Size = new Size(formWidth, formHeight);

                    // Position the form in the top-right corner of the screen
                    var screen = Screen.FromControl(this);
                    this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Top);
                }

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
                    txtOutput.Lines = Airshow.GetLines(myAirshows);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading form: " + ex.Message);
                }
            }
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


        private void btnParseAirshowDataFile_Click(object sender, EventArgs e)
        {
            //myAirshows.Clear();

            List<Airshow> airshows = new List<Airshow>();

            //WorkingFileParserClass;
            WorkingFileParserClass = cAirshowFileParserSetupTool.LoadMe();

            Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(WorkingFileParserClass);

            DialogResult dr = DialogResult.OK;
            dr = sb.showDialog();

            if (dr == DialogResult.OK)
            {
                this.Enabled = false;
                this.txtOutput.Clear();
                Airshow.LoadFile(WorkingFileParserClass, airshows);
                //myFormState.AirshowYearofInterest = WorkingFileParserClass.AirshowYear;
                airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();
                txtOutput.Lines = Airshow.GetLines(airshows);
                myFilteredAirshows = airshows.ToList();
                cAirshowFileParserSetupTool.SaveMe(WorkingFileParserClass);
                this.Enabled = true;
            }
        }

        private void btnCopyToTabs_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            myFilteredAirshows = myFilteredAirshows.OrderBy(c => c.WeekNumber).ToList();
            string lines = Airshow.GetTabOutput(myFilteredAirshows);
            Clipboard.SetText(lines);
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {

            AirshowGroup asg = new AirshowGroup();
            List<Airshow> airshows = new List<Airshow>();

            if (System.IO.File.Exists(WorkingFileParserClass.sFileName))
            {
                try
                {
                    Airshow.LoadFile(WorkingFileParserClass, airshows);
                    airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();
                    txtOutput.Lines = Airshow.GetLines(airshows);
                    myFilteredAirshows = airshows.ToList();
                    asg.Airshows.myShows = airshows;
                    asg.AirshowYearOfInterest = WorkingFileParserClass.AirshowYear;
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "*.asg.XML|*.asg.xml";
                    sfd.Title = "Save an Airshow Group";
                    DialogResult dr = sfd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        Electroimpact.XmlSerialization.Serializer.Save(asg, sfd.FileName);
                    }
                    return;
                }
                catch { }
            }
            MessageBox.Show("Something Wrong.  Be first set this up by using the \"Parse Data File Tool\"");
        }

        private void btnReadXML_Click(object sender, EventArgs e)
        {
            myAirshows.Clear();

            System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML|*.xml";
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.Enabled = false;
                bool success;
                AirshowGroup asg = AirshowGroup.LoadMe(ofd.FileName, out success);
                if(!success) { return;  }
                myAirshows = asg.Airshows.myShows;
                myFormState.AirshowYearofInterest = asg.AirshowYearOfInterest;
                myFormState.fnCurrentXMLDataBase = ofd.FileName;
                FormState.SaveMe(myFormState);
                txtOutput.Lines = Airshow.GetLines(myAirshows);
                myFilteredAirshows = myAirshows.ToList();
                this.Enabled = true;
                LoadGrid(myFormState.AirshowYearofInterest);
                ColorGrid(myFilteredAirshows);
                lblYearOfInterest.Text = $"Airshow Year of Interest: {asg.AirshowYearOfInterest.ToString()} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
            }
        }

        private void btnCompareXMLs_Click(object sender, EventArgs e)
        {
            //need to set pointers to each of the xmls
            //What's new in future year?
            //what's not in the other?
            //interface to merge data - eg show merge
            //interface for manual merge

            DialogResult dr = SelectFilesToCompare();

            if (dr == DialogResult.OK)
            {
                this.Enabled = false;
                this.txtOutput.Clear();
                this.chklstbox_diff.Items.Clear();
                this.txtRight.Clear();

                AirshowGroup asgLeft = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameLeft);
                txtOutput.Lines = Airshow.GetLines(asgLeft.Airshows.myShows);
                myFilteredAirshows = asgLeft.Airshows.myShows.ToList();

                AirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameRight);
                txtRight.Lines = Airshow.GetLines(asgRight.Airshows.myShows);

                List<Airshow> newShows = new List<Airshow>();

                foreach (Airshow ashow in asgRight.Airshows.myShows)
                {
                    //List<cPly> plys = Ply.Where(ply => ply.SeqId == SeqId).ToList();
                    //List<cAirshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.CompareYears(ashow)).ToList();
                    List<Airshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.IsEqual(ashow, false)).ToList();
                    if (foundAirshows.Count > 0)
                        continue;
                    newShows.Add(ashow);
                }
                myMergedShows.Clear();
                myMergedShows = newShows;

                //txtMerged.Lines = cAirshow.GetLines(myMergedShows);

                foreach (Airshow ashow in newShows)
                {
                    chklstbox_diff.Items.Add(ashow);
                }
                //chklstbox_diff.Items = myMergedShows.ToList();

                this.Enabled = true;
            }
        }

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

        private void btnNewShows_Click(object sender, EventArgs e)
        {
            DialogResult dr = SelectFilesToCompare();
            if (dr == DialogResult.OK)
            {
                this.Enabled = false;
                this.txtOutput.Clear();
                this.chklstbox_diff.Items.Clear();
                this.txtRight.Clear();

                AirshowGroup asgLeft = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameLeft);
                txtOutput.Lines = Airshow.GetLines(asgLeft.Airshows.myShows);
                myFilteredAirshows = asgLeft.Airshows.myShows.ToList();

                AirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameRight);
                txtRight.Lines = Airshow.GetLines(asgRight.Airshows.myShows);

                List<Airshow> newShows = new List<Airshow>();

                foreach (Airshow ashow in asgRight.Airshows.myShows)
                {
                    //List<cPly> plys = Ply.Where(ply => ply.SeqId == SeqId).ToList();
                    //List<cAirshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.CompareYears(ashow)).ToList();
                    List<Airshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.CompareYears(ashow)).ToList();
                    if (foundAirshows.Count > 0)
                        continue;
                    newShows.Add(ashow);
                }

                foreach (Airshow newAirshow in newShows)
                {
                    chklstbox_diff.Items.Add(newAirshow);
                }
                //txtMerged.Lines = cAirshow.GetLines(newShows);

                myMergedShows.Clear();
                myMergedShows = newShows;

                this.Enabled = true;
            }
        }

        private void btnCopyMiddle_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            string lines = Airshow.GetTabOutput(myMergedShows);
            Clipboard.SetText(lines);
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

                        chklstbox_diff.Items.Clear();
                        if (mergedAirshowsThisweek.Count > 0)
                        {
                            foreach (Airshow airshow in mergedAirshowsThisweek)
                            {
                                chklstbox_diff.Items.Add(airshow);
                            }
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

        private void btnARCVDB_Click(object sender, EventArgs e)
        {
            SaveAirshowSchedule(true);
        }

        private void btnCompareNewToDB_Click(object sender, EventArgs e)
        {
            //need to set pointers to each of the xmls
            //What's new in future year?
            //what's not in the other?
            //interface to merge data - eg show merge
            //interface for manual merge

            DialogResult dr = SelectFilesToCompare(myFormState.fnCurrentXMLDataBase);

            if (dr == DialogResult.OK)
            {
                this.Enabled = false;
                this.txtOutput.Clear();
                this.chklstbox_diff.Items.Clear();
                this.txtRight.Clear();

                AirshowGroup asgLeft = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameLeft);
                txtOutput.Lines = Airshow.GetLines(asgLeft.Airshows.myShows);
                myFilteredAirshows = asgLeft.Airshows.myShows.ToList();

                AirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameRight);
                txtRight.Lines = Airshow.GetLines(asgRight.Airshows.myShows);

                List<Airshow> newShows = new List<Airshow>();

                foreach (Airshow ashow in asgRight.Airshows.myShows)
                {

                    List<Airshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.IsEqual(ashow, false)).ToList();
                    if (foundAirshows.Count > 0)
                        continue;
                    newShows.Add(ashow);
                }
                myMergedShows.Clear();
                myMergedShows = newShows;

                //txtMerged.Lines = cAirshow.GetLines(myMergedShows);



                foreach (Airshow ashow in newShows)
                {
                    chklstbox_diff.Items.Add(ashow);



                }

                this.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Airshow> newairshows = myAirshows.ToList();
            List<Airshow> checkedShows = new List<Airshow>();
            List<int> indicesToNuke = new List<int>();

            for (int ii = 0; ii < chklstbox_diff.Items.Count; ii++)
            {
                if (chklstbox_diff.GetItemChecked(ii) == true)
                {
                    checkedShows.Add((Airshow)chklstbox_diff.Items[ii]);
                    indicesToNuke.Add(ii);
                }
            }

            foreach (Airshow ashow in checkedShows)
            {
                if (!newairshows.Exists(x => x.IsEqual(ashow)))
                {
                    newairshows.Add(ashow);
                    myMergedShows.Remove(ashow);
                }
            }

            for (int ii = indicesToNuke.Count - 1; ii >= 0; ii--)
            {
                chklstbox_diff.Items.RemoveAt(indicesToNuke[ii]);
            }

            DialogResult dr = MessageBox.Show("Do you want to modify the active DB (YES) or save to a new DB (NO)?", "", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                myAirshows = newairshows.ToList();
                SaveAirshowSchedule(false);
            }
            else if (dr == DialogResult.No)
            {
                SaveAirshowSchedule(true, newairshows);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Regions rgn = new Regions();
            string clipboard = Clipboard.GetText();
            rgn.BuildFile(clipboard);
            Regions.SaveMe(rgn, $@"C:\test\Regons.xml");
        }

        private void btnSetRegionFile_Click(object sender, EventArgs e)
        {
            Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(myFormState);

            DialogResult dr = DialogResult.OK;
            dr = sb.showDialog();

            if (dr == DialogResult.OK)
            {
                FormState.SaveMe(myFormState);
                myRegions = Regions.LoadMe(myFormState.fnRegions);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int ii = 0; ii < chklstRegions.Items.Count; ii++)
            {
                chklstRegions.SetItemChecked(ii, true);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int ii = 0; ii < chklstRegions.Items.Count; ii++)
            {
                chklstRegions.SetItemChecked(ii, false);
            }
        }

        private void chklstRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<Airshow> FilteredAirshows = new List<Airshow>();

            foreach (Airshow airshow in myAirshows)
            {
                string rgn = "";
                string state = airshow.location.State.ToUpper();
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
            txtOutput.Clear();
            txtOutput.Lines = Airshow.GetLines(myFilteredAirshows);
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
            string allthedata = "";
            foreach (string st in calltheseguys)
            {
                allthedata += st + "\n";
            }
            Clipboard.SetText(allthedata);
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
            string allthedata = "";
            foreach (string st in calltheseguys)
            {
                allthedata += st + "\n";
            }
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
            string allthedata = "";
            foreach (string st in calltheseguys)
            {
                allthedata += st + "\n";
            }
            Clipboard.SetText(allthedata);
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
            string allthedata = "";
            foreach (string st in calltheseguys)
            {
                allthedata += st + "\n";
            }
            Clipboard.SetText(allthedata);
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

        private void btnCheckForDuplicates_Click(object sender, EventArgs e)
        {

            List<Airshow> new_airshows = new List<Airshow>();
            for (int ii = 0; ii < chklstbox_diff.Items.Count; ii++)
            {
                //if (chklstbox_diff.GetItemChecked(ii) == true)
                new_airshows.Add((Airshow)chklstbox_diff.Items[ii]);
            }
            List<string> Potential_Duplicates = new List<string>();

            foreach (Airshow ashow in new_airshows)
            {
                List<Airshow> lstDuplicates = myAirshows.Where(airshow => airshow.location.Equals(ashow.location)).ToList();

                if (lstDuplicates.Count > 0)
                {
                    foreach (Airshow adup in lstDuplicates)
                    {
                        string possibleDuplicate = string.Concat(adup.ToString(), "\n");
                        Potential_Duplicates.Add(possibleDuplicate);
                    }
                }
            }

            if (Potential_Duplicates.Count == 0)
                return;

            string wholestring = "";
            foreach (string s in Potential_Duplicates)
                wholestring += s;
            MessageBox.Show(wholestring + "\n may be duplicate[s], OK to copy to clipboard.");
            Clipboard.SetText(wholestring);
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
                            ret = myFilteredAirshows.Where(c => c.location.City.ToLower().Contains(SearchTerms.szSearchTerm.ToLower())).ToList();
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

        private void btnCheckForDuplicatesInDB_Click(object sender, EventArgs e)
        {
            //check for duplicates in the database
            List<Airshow> duplicateAirshowsFound = new List<Airshow>();
            List<Airshow> airshowsFoundInNewDB = new List<Airshow>();

            //for every airshow in the database, compare cities to see if they show up more than once
            List<Airshow> copiedList = Airshow.DeepCopy(myAirshows);
            foreach (Airshow ashow in copiedList)
            {
                List<Airshow> duplicatesFound = new List<Airshow>();
                duplicatesFound = copiedList.Where(airshow => airshow.location.Equals(ashow.location)).ToList();

                if (duplicatesFound.Count > 1)
                {
                    foreach (Airshow adup in duplicatesFound)
                    {
                        string possibleDuplicate = string.Concat(adup.ToString(), "\n");
                        duplicateAirshowsFound.Add(adup);
                    }
                }
            }

            //we need an open file dialogue to open up the most recently downloaded asg.xml file
            if (duplicateAirshowsFound.Count == 0)
            {
                MessageBox.Show("No duplicates found.");
                return;
            }
            DialogResult dr = MessageBox.Show($"There are {duplicateAirshowsFound.Count / 2} duplicates.\nDo you want to Open the latest downloaded asg.xml to check which one is valid?", "Open a file?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Airshow Group Files (*.asg.xml)|*.asg.xml";
                openFileDialog.Title = "Open the most recent Airshow List to find the most recent updates";
                openFileDialog.DefaultExt = "asg.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bool success;
                    AirshowGroup asgLatest = AirshowGroup.LoadMe(openFileDialog.FileName, out success );
                    List<Airshow> latestAirshowList = asgLatest.Airshows.myShows.ToList();
                    int count = copiedList.Count;

                    foreach (Airshow ashow in duplicateAirshowsFound)
                    {
                        List<Airshow> airshowsInNewDB = latestAirshowList.Where(airshow => airshow.location.Equals(ashow.location)).ToList();
                        foreach (Airshow adup in airshowsInNewDB)
                        {
                            if (!airshowsFoundInNewDB.Contains(adup))
                            {
                                string airshowInNewDB = string.Concat(adup.ToString(), "\n");
                                airshowsFoundInNewDB.Add(adup);
                            }
                        }
                    }

                    foreach (Airshow dupShow in duplicateAirshowsFound)
                    {
                        List<Airshow> validShows = airshowsFoundInNewDB.Where(airshow => airshow.location.Equals(dupShow.location)).ToList();
                        if (validShows.Count == 1)
                        {
                            if (!validShows[0].IsEqual(dupShow, false))
                            {
                                List<Airshow> airshowToRemove = copiedList.Where(airshow => airshow.IsEqual(dupShow, false)).ToList();
                                foreach (Airshow adup in airshowToRemove)
                                {
                                    validShows[0].AppendCustomFields(adup);
                                    copiedList.Remove(adup);
                                    Console.WriteLine($"Removed {adup.ToString()}".Pastel(Color.Red));
                                }
                            }
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine($"There are {copiedList.Count} airshows left in the list.".Pastel(Color.Green));
                    Console.WriteLine($"There are {myAirshows.Count} airshows in the original database.".Pastel(Color.Green));
                    Console.WriteLine($"There are {duplicateAirshowsFound.Count / 2} duplicates detected.".Pastel(Color.Green));
                    Console.WriteLine($"There are {count - copiedList.Count} duplicates removed.".Pastel(Color.Green));
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue.".Pastel(Color.Green));
                    Console.ReadKey();

                    count = copiedList.Count;
                    List<Airshow> showsToRemove = new List<Airshow>();
                    foreach (Airshow ashow in copiedList)
                    {
                        List<Airshow> showsFound = latestAirshowList.Where(airshow => airshow.IsEqual(ashow, false)).ToList();
                        if (showsFound.Count == 0)
                        {
                            Console.WriteLine($"Airshow {ashow.ToString()} is not in the latest database. Do you want to remove it (Y/N)?".Pastel(Color.Red));
                            string response = Console.ReadLine();
                            if (response.ToLower() == "y")
                            {
                                showsToRemove.Add(ashow);
                            }
                        }
                    }
                    foreach (Airshow airshow in showsToRemove)
                    {
                        copiedList.Remove(airshow);
                    }


                    Console.WriteLine();
                    Console.WriteLine($"There are {copiedList.Count} airshows left in the list.".Pastel(Color.Green));
                    Console.WriteLine($"There are {myAirshows.Count} airshows in the original database.".Pastel(Color.Green));
                    Console.WriteLine($"We removed {count - copiedList.Count} non-existant Airshows.".Pastel(Color.Green));
                    Console.WriteLine();
                    Console.WriteLine("Do you want to make this permenant? (Y/N)".Pastel(Color.Yellow));
                    string response2 = Console.ReadLine();
                    if (response2.ToLower() == "y")
                    {
                        Console.WriteLine("This change is permenant, are you sure?! (Y/N)".Pastel(Color.Red));
                        string confirm = Console.ReadLine();
                        if (confirm.ToLower() == "y")
                        {
                            myAirshows = copiedList;
                            SaveAirshowSchedule(false);
                        }
                    }

                    // {
                    //     AirshowGroup asg = new AirshowGroup();
                    //     List<Airshow> airshows = new List<Airshow>();
                    //     AirshowGroup workingASG = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase);

                    //     try
                    //     {

                    //         copiedList = copiedList.OrderBy(airshow => airshow.WeekNumber).ToList();
                    //         txtOutput.Lines = Airshow.GetLines(airshows);
                    //         asg.Airshows.myShows = copiedList;
                    //         asg.AirshowYearOfInterest = workingASG.AirshowYearOfInterest;
                    //         SaveFileDialog sfd = new SaveFileDialog();
                    //         sfd.Filter = "*.asg.XML|*.asg.xml";
                    //         sfd.Title = "Save an Airshow Group";
                    //         DialogResult dr2 = sfd.ShowDialog();
                    //         if (dr2 == DialogResult.OK)
                    //         {
                    //             Electroimpact.XmlSerialization.Serializer.Save(asg, sfd.FileName);
                    //         }
                    //     }
                    //     catch { }
                    // }
                }

            }
            else
            {
                Console.WriteLine("Here are the duplicates you need to fix:".Pastel(Color.Green));
                foreach (Airshow ashow in duplicateAirshowsFound)
                {
                    Console.WriteLine(ashow.ToString().Pastel(Color.Yellow));
                }
                Console.WriteLine();

                // Open the DeleteAirshowForm
                int Count = copiedList.Count;
                DeleteAirshowForm deleteForm = new DeleteAirshowForm(duplicateAirshowsFound, copiedList);
                deleteForm.ShowDialog();
                Console.WriteLine($"There were {Count - copiedList.Count} airshows removed.".Pastel(Color.Green));
                Console.WriteLine();
                Console.WriteLine("Do you want to make this permenant? (Y/N)".Pastel(Color.Yellow));
                string response2 = Console.ReadLine();
                if (response2.ToLower() == "y")
                {
                    Console.WriteLine("This change is permenant, are you sure?! (Y/N)".Pastel(Color.Red));
                    string confirm = Console.ReadLine();
                    if (confirm.ToLower() == "y")
                    {
                        myAirshows = copiedList;
                        SaveAirshowSchedule(false);
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