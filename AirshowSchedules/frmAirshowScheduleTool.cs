using Electroimpact.SettingsFormBuilderV2.Attributes;
using System.Data;
using System.Globalization;
using System.Xml.Serialization;
using static AirshowSchedules.Airshow;
using static AirshowSchedules.cCalenderYear;

namespace AirshowSchedules
{
    public partial class frmAirshowScheduleTool : Form
    {
        FormState myFormState = new FormState();

        //This is the master list
        List<Airshow> myAirshows = new List<Airshow>();

        //this is used for display
        List<Airshow> myFilteredAirshows = new List<Airshow>();

        //not sure
        List<Airshow> myMergedShows = new List<Airshow>();
        cAirshowFileParserSetupTool WorkingFileParserClass = new cAirshowFileParserSetupTool();
        cAirshowScheduleCompare myASGCompare = new cAirshowScheduleCompare();
        AirshowSchedules.Regions myRegions = new AirshowSchedules.Regions();
        private System.Windows.Forms.TextBox TextBox1;

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
        private void LoadGrid(int YearOfInterest)
        {
            //Creates the Saturdays in a given year. Formats the resulting grid.
            List<AirshowWeekend> lSaturdays = cCalenderYear.GetSaturdaysList(YearOfInterest);

            DataTable dataTable = new DataTable();

            for (int nWeekend = 0; nWeekend < 5; nWeekend++)
            {
                dataTable.Columns.Add($" Weekend {nWeekend + 1}");
            }

            for (int nMonth = 0; nMonth < 12; nMonth++)
            {
                DataRow row = dataTable.Rows.Add();
                List<AirshowWeekend> saturdaysthismonth = lSaturdays.Where(x => x.Date.Month == nMonth + 1).ToList();
                for (int nSats = 0; nSats < saturdaysthismonth.Count; nSats++)
                {
                    row[nSats] = saturdaysthismonth[nSats];
                }
            }

            dgvCalendar.DataSource = dataTable;

            int rowCount = dgvCalendar.Rows.Count;
            int columnCount = dgvCalendar.Columns.Count;


            //dgvCalendar.RowHeadersDefaultCellStyle. = DataGridViewColumnSortMode.NotSortable;
            dgvCalendar.RowHeadersVisible = false;

            if (rowCount > 0 && columnCount > 0)
            {
                int rowHeight = dgvCalendar.Height / (rowCount + 1);
                int columnWidth = (int)((double)dgvCalendar.Width * .999) / columnCount;

                // Set the row heights
                foreach (DataGridViewRow row in dgvCalendar.Rows)
                {
                    row.Height = rowHeight;
                }

                // Set the column widths
                foreach (DataGridViewColumn column in dgvCalendar.Columns)
                {
                    column.Width = columnWidth;
                }
            }
        }

        private void LoadShowGrid(List<Airshow> listtheseshows, string ColumnTItle = "Shows This Weekend")
        {
            //This is what happens when you select a weekend in the main grid.  dataGridViewShows is the pop up to the right and it lists the shows for a given weekend. 
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(ColumnTItle);
            for (int ii = 0; ii < listtheseshows.Count; ii++)
            {
                DataRow row = dataTable.Rows.Add();
                row[0] = listtheseshows[ii].ToString();
            }

            dataGridViewShows.DataSource = dataTable;
            dataGridViewShows.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewShows.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            // Create a new DataGridViewCellStyle object for the column headers
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();

            // Set the desired font style to bold
            headerStyle.Font = new Font(dataGridViewShows.Font, FontStyle.Bold);

            // Apply the header style to the column headers
            dataGridViewShows.ColumnHeadersDefaultCellStyle = headerStyle;

            for (int ii = 0; ii < listtheseshows.Count; ii++)
            {
                Airshow ashow = listtheseshows[ii];
                DataGridViewCell cell = dataGridViewShows.Rows[ii].Cells[0];
                switch (ashow.Status)
                {
                    case eStatus.maybe:
                        cell.Style.BackColor = Color.LightYellow;
                        break;
                    case eStatus.verbal:
                        cell.Style.BackColor = Color.LightGreen;
                        break;
                    case eStatus.contract:
                        cell.Style.BackColor = Color.LightBlue;
                        break;
                    case eStatus.pursue:
                        cell.Style.BackColor = Color.LightCoral;
                        break;
                    case eStatus.NO:
                        cell.Style.BackColor = Color.LightGray;
                        break;
                    default:
                        cell.Style.BackColor = Color.White;
                        break;
                }
            }
        }

        private int GetYearOfInterest()
        {
            myFormState = FormState.LoadMe();
            AirshowGroup asg = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase);
            int yearofinterest = asg.AirshowYearOfInterest;
            return yearofinterest;
        }

        private void ColorGrid(List<Airshow> theseshows)
        {
            //this colors the grid and puts an airshow name in there if there is some sort of status associated with it. 
            //if there is a show this weekend at all, the cell is bolded regardless of state. 
            List<Airshow> actionShows = theseshows.Where(x => x.Status != eStatus.none && x.Status != eStatus.NO).ToList();

            //actionShows = actionShows.OrderByDescending(x => x.Status).ToList();
            //foreach (cAirshow ashowwithInterest in actionShows)
            {
                foreach (DataGridViewRow row in dgvCalendar.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellText = cell.Value?.ToString() ?? string.Empty;
                        string[] weekendinquestion = cellText.Split(' ');
                        if (weekendinquestion.Length >= 3)
                        {

                            DateTime dateTime = new DateTime(GetYearOfInterest(), int.Parse(weekendinquestion[0]), int.Parse(weekendinquestion[2]));
                            AirshowWeekend asw = new AirshowWeekend(dateTime);
                            List<Airshow> showsthisweekend = actionShows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
                            showsthisweekend = showsthisweekend.OrderBy(x => x.Status).ToList();
                            if (showsthisweekend.Count == 0)
                                continue;
                            Airshow ashowwithInterest = showsthisweekend[0];

                            if (asw.weekofyear == ashowwithInterest.WeekNumber)
                            {
                                switch (ashowwithInterest.Status)
                                {
                                    case eStatus.pursue:
                                        cell.Style.BackColor = Color.LightCoral;
                                        break;
                                    case eStatus.maybe:
                                        cell.Style.BackColor = Color.LightYellow;
                                        break;
                                    case eStatus.verbal:
                                        cell.Style.BackColor = Color.LightGreen;
                                        break;
                                    case eStatus.contract:
                                        cell.Style.BackColor = Color.LightBlue;
                                        break;
                                    default:
                                        cell.Style.BackColor = Color.White;
                                        break;
                                }
                                cell.Value += $" \n{ashowwithInterest.name_airshow}";
                            }
                        }
                    }
                }
            }
            foreach (DataGridViewRow row in dgvCalendar.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    string cellText = cell.Value?.ToString() ?? string.Empty;
                    string[] weekendinquestion = cellText.Split(' ');
                    if (weekendinquestion.Length >= 3)
                    {
                        DateTime dateTime = new DateTime(2023, int.Parse(weekendinquestion[0]), int.Parse(weekendinquestion[2]));
                        AirshowWeekend asw = new AirshowWeekend(dateTime);
                        List<Airshow> showsthisweek = theseshows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
                        if (showsthisweek.Count > 0)
                        {
                            Font boldFont = new Font(dgvCalendar.DefaultCellStyle.Font, FontStyle.Bold);
                            cell.Style.Font = boldFont;
                        }
                        else
                        {
                            Font regFont = new Font(dgvCalendar.DefaultCellStyle.Font, FontStyle.Regular);
                            cell.Style.Font = regFont;
                        }
                    }
                }
            }
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

        public frmAirshowScheduleTool()
        {
            //if(!DesignMode)
            {
                try
                {
                    InitializeComponent();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Designer: " + ex.Message);
                }
            }
        }

        #region Form Event Callbacks
        private void frmAirshowScheduleTool_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                this.Size = new Size(1650, 1000);
                // set the location to the upper right of the screen
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, 0);


                myFormState = FormState.LoadMe();
                //AirshowGroup asg = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(FileName);
                AirshowGroup asg = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase);
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
        }

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
                AirshowGroup asg = AirshowGroup.LoadMe(ofd.FileName);
                myAirshows = asg.Airshows.myShows;
                myFormState.AirshowYearofInterest = asg.AirshowYearOfInterest;
                myFormState.fnCurrentXMLDataBase = ofd.FileName;
                FormState.SaveMe(myFormState);
                txtOutput.Lines = Airshow.GetLines(myAirshows);
                myFilteredAirshows = myAirshows.ToList();
                this.Enabled = true;
                LoadGrid(myFormState.AirshowYearofInterest);
                //GridTools.LoadShowGrid(dataGridViewShows, myFormState.AirshowYearofInterest);
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
                    List<Airshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.Equals(ashow, false)).ToList();
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

                    List<Airshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.Equals(ashow, false)).ToList();
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
                if (!newairshows.Exists(x => x.Equals(ashow)))
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

        private void button6_Click(object sender, EventArgs e)
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