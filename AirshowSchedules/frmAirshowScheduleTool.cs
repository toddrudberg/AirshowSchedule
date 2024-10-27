using Electroimpact;
using Electroimpact.SettingsFormBuilderV2.Attributes;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using static AirshowSchedules.cAirshow;
using static AirshowSchedules.cCalenderYear;
using static AirshowSchedules.frmAirshowScheduleTool;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static AirshowSchedules.frmAirshowScheduleTool.cAirshowGroup;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
//using static AirshowSchedules.Form1.cAirshowGroup;


namespace AirshowSchedules
{
  public partial class frmAirshowScheduleTool : Form
  {
    cAirshowFormState myFormState = new cAirshowFormState();
    List<cAirshow> myAirshows = new List<cAirshow>();
    List<cAirshow> myFilteredAirshows = new List<cAirshow>();
    List<cAirshow> myMergedShows = new List<cAirshow>();
    cAirshowFileParserSetupTool WorkingFileParserClass = new cAirshowFileParserSetupTool();
    cAirshowScheduleCompare myASGCompare = new cAirshowScheduleCompare();
    cRegions myRegions = new cRegions();
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



    [Serializable]
    public class cAirshowFormState
    {
      public int AirshowYearofInterest = 2023;
      public string fnLastParsedFile = "";
      public string fnCurrentXMLDataBase = "";
      [Display(DisplayName = "Choose the Region File:")]
      [FileBrowseDialog(OpenFileDialogFilter = "Region File (*.xml)|*.xml")]
      public string fnRegions = "";

      public static void SaveMe(cAirshowFormState fs)
      {
        string fng = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowScheduler-FormState");

        Electroimpact.XmlSerialization.Serializer.Save(fs, fng);
      }

      public static cAirshowFormState LoadMe()
      {
        string fng = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowScheduler-FormState");

        cAirshowFormState fs = new cAirshowFormState();

        try
        {
          if (System.IO.File.Exists(fng))
            fs = Electroimpact.XmlSerialization.Serializer.Load<cAirshowFormState>(fng);

          return fs;
        }
        catch { return fs; }
      }
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
    public class cAirshowGroup
    {
      //public cAirshowFileParserSetupTool FileSetup = new cAirshowFileParserSetupTool();
      public cAirshows Airshows = new cAirshows();
      public int AirshowYearOfInterest = 2023;
      public cAirshowGroup()
      { }

      #region Wrapper Classes
      public class cAirshows
      {
        [XmlElement("Airshow")]
        public List<cAirshow> myShows = new List<cAirshow>();
      }
      #endregion


      public static void SaveMe(List<cAirshow> AirShows, string FileName)
      {
        cAirshowGroup myGroup = new cAirshowGroup();
        myGroup.Airshows.myShows = AirShows;
        Electroimpact.XmlSerialization.Serializer.Save(AirShows, FileName);
      }
      public static cAirshowGroup LoadMe(string FileName)
      {
        try
        {
          if (System.IO.File.Exists(FileName))
          {
            cAirshowGroup asg = new cAirshowGroup();
            asg = Electroimpact.XmlSerialization.Serializer.Load<cAirshowGroup>(FileName);
            return asg;
          }
          else
          {
            return new cAirshowGroup();
          }
        }
        catch
        {
          return new cAirshowGroup();
        }
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

    [Serializable]
    public class cRegions
    {
      //Electroimpact.XmlSerialization.SerializableDictionary<string, string> Regions = new Electroimpact.XmlSerialization.SerializableDictionary<string, string>();
      public Dictionary<string, string> Regions = new Dictionary<string, string>();

      public class cValuePair
      {
        public string sState;
        public string sRegion;
      }

      public void BuildFile(string tsvPaste)
      {
        string[] lines = tsvPaste.Split('\n');
        foreach (string s in lines)
        {
          string[] ss = s.Split('\t');
          if (ss.Length > 1)
            Regions.Add(ss[0], ss[1].Trim());
        }
      }

      public List<string> GetRegionList()
      {
        List<string> regions = Regions.Values.ToList();
        regions.Sort();
        string lastRegion = "";
        List<string> uniqueRegions = new List<string>();

        foreach (string rgn in regions)
        {
          if (lastRegion != rgn)
          {
            uniqueRegions.Add(rgn);
            lastRegion = rgn;
          }
        }

        return uniqueRegions;
      }

      public static cRegions LoadMe(string FileName)
      {
        string fng = FileName;
        cRegions fs = new cRegions();

        List<cValuePair> getthedata = new List<cValuePair>();
        try
        {
          if (System.IO.File.Exists(fng))
          {
            getthedata = Electroimpact.XmlSerialization.Serializer.Load<List<cValuePair>>(fng);
          }

          foreach (cValuePair pair in getthedata)
          {
            fs.Regions.Add(pair.sState, pair.sRegion);
          }

          return fs;
        }
        catch { return fs; }
      }

      public static void SaveMe(cRegions fs, string FileName)
      {
        List<cValuePair> pairs = new List<cValuePair>();
        foreach (KeyValuePair<string, string> pair in fs.Regions)
        {
          cValuePair cp = new cValuePair();
          cp.sState = pair.Key;
          cp.sRegion = pair.Value;
          pairs.Add(cp);
        }

        Electroimpact.XmlSerialization.Serializer.Save(pairs, FileName);
      }
    }

    #endregion

    #region Sub Routines
    private void LoadGrid(int YearOfInterest)
    {
      //Creates the Saturdays in a given year. Formats the resulting grid.
      List<cAirshowWeekend> lSaturdays = cCalenderYear.GetSaturdaysList(YearOfInterest);

      DataTable dataTable = new DataTable();

      for (int nWeekend = 0; nWeekend < 5; nWeekend++)
      {
        dataTable.Columns.Add($" Weekend {nWeekend + 1}");
      }

      for (int nMonth = 0; nMonth < 12; nMonth++)
      {
        DataRow row = dataTable.Rows.Add();
        List<cAirshowWeekend> saturdaysthismonth = lSaturdays.Where(x => x.Date.Month == nMonth + 1).ToList();
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

    private void LoadShowGrid(List<cAirshow> listtheseshows, string ColumnTItle = "Shows This Weekend")
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
        cAirshow ashow = listtheseshows[ii];
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
      myFormState = cAirshowFormState.LoadMe();
      cAirshowGroup asg = cAirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase);
      int yearofinterest = asg.AirshowYearOfInterest;
      return yearofinterest;
    }

    private void ColorGrid(List<cAirshow> theseshows)
    {
      //this colors the grid and puts an airshow name in there if there is some sort of status associated with it. 
      //if there is a show this weekend at all, the cell is bolded regardless of state. 
      List<cAirshow> actionShows = theseshows.Where(x => x.Status != eStatus.none && x.Status != eStatus.NO).ToList();

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
              cAirshowWeekend asw = new cAirshowWeekend(dateTime);
              List<cAirshow> showsthisweekend = actionShows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
              showsthisweekend = showsthisweekend.OrderBy(x => x.Status).ToList();
              if (showsthisweekend.Count == 0)
                continue;
              cAirshow ashowwithInterest = showsthisweekend[0];

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
            cAirshowWeekend asw = new cAirshowWeekend(dateTime);
            List<cAirshow> showsthisweek = theseshows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
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

    private void SaveAirshowSchedule(bool DoFileDialogue, List<cAirshow> airshows)
    {
      cAirshowGroup asg = new cAirshowGroup();
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
        myFormState = cAirshowFormState.LoadMe();
        cAirshowGroup asg = cAirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase);
        myAirshows = asg.Airshows.myShows;
        //myFilteredAirshows = myAirshows.ToList();
        myFormState.AirshowYearofInterest = asg.AirshowYearOfInterest;
        lblYearOfInterest.Text = $"Airshow Year of Interest: {asg.AirshowYearOfInterest.ToString()} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
        LoadGrid(myFormState.AirshowYearofInterest);
        myFilteredAirshows = myAirshows.ToList();
        ColorGrid(myFilteredAirshows);
        txtOutput.Lines = cAirshow.GetLines(myAirshows);
        myRegions = cRegions.LoadMe(myFormState.fnRegions);
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

      List<cAirshow> airshows = new List<cAirshow>();

      //WorkingFileParserClass;
      WorkingFileParserClass = cAirshowFileParserSetupTool.LoadMe();

      Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(WorkingFileParserClass);

      DialogResult dr = DialogResult.OK;
      dr = sb.showDialog();

      if (dr == DialogResult.OK)
      {
        this.Enabled = false;
        this.txtOutput.Clear();
        cAirshow.LoadFile(WorkingFileParserClass, airshows);
        //myFormState.AirshowYearofInterest = WorkingFileParserClass.AirshowYear;
        airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();
        txtOutput.Lines = cAirshow.GetLines(airshows);
        myFilteredAirshows = airshows.ToList();
        cAirshowFileParserSetupTool.SaveMe(WorkingFileParserClass);
        this.Enabled = true;
      }
    }

    private void btnCopyToTabs_Click(object sender, EventArgs e)
    {
      Clipboard.Clear();
      myFilteredAirshows = myFilteredAirshows.OrderBy( c => c.WeekNumber ).ToList();
      string lines = cAirshow.GetTabOutput(myFilteredAirshows);
      Clipboard.SetText(lines);
    }

    private void btnSaveAs_Click(object sender, EventArgs e)
    {

      cAirshowGroup asg = new cAirshowGroup();
      List<cAirshow> airshows = new List<cAirshow>();

      if (System.IO.File.Exists(WorkingFileParserClass.sFileName))
      {
        try
        {
          cAirshow.LoadFile(WorkingFileParserClass, airshows);
          airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();
          txtOutput.Lines = cAirshow.GetLines(airshows);
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
        cAirshowGroup asg = cAirshowGroup.LoadMe(ofd.FileName);
        myAirshows = asg.Airshows.myShows;
        myFormState.AirshowYearofInterest = asg.AirshowYearOfInterest;
        myFormState.fnCurrentXMLDataBase = ofd.FileName;
        cAirshowFormState.SaveMe(myFormState);
        txtOutput.Lines = cAirshow.GetLines(myAirshows);
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

        cAirshowGroup asgLeft = Electroimpact.XmlSerialization.Serializer.Load<cAirshowGroup>(myASGCompare.sFileNameLeft);
        txtOutput.Lines = cAirshow.GetLines(asgLeft.Airshows.myShows);
        myFilteredAirshows = asgLeft.Airshows.myShows.ToList();

        cAirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<cAirshowGroup>(myASGCompare.sFileNameRight);
        txtRight.Lines = cAirshow.GetLines(asgRight.Airshows.myShows);

        List<cAirshow> newShows = new List<cAirshow>();

        foreach (cAirshow ashow in asgRight.Airshows.myShows)
        {
          //List<cPly> plys = Ply.Where(ply => ply.SeqId == SeqId).ToList();
          //List<cAirshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.CompareYears(ashow)).ToList();
          List<cAirshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.Equals(ashow, false)).ToList();
          if (foundAirshows.Count > 0)
            continue;
          newShows.Add(ashow);
        }
        myMergedShows.Clear();
        myMergedShows = newShows;

        //txtMerged.Lines = cAirshow.GetLines(myMergedShows);

        foreach (cAirshow ashow in newShows)
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

        cAirshowGroup asgLeft = Electroimpact.XmlSerialization.Serializer.Load<cAirshowGroup>(myASGCompare.sFileNameLeft);
        txtOutput.Lines = cAirshow.GetLines(asgLeft.Airshows.myShows);
        myFilteredAirshows = asgLeft.Airshows.myShows.ToList();

        cAirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<cAirshowGroup>(myASGCompare.sFileNameRight);
        txtRight.Lines = cAirshow.GetLines(asgRight.Airshows.myShows);

        List<cAirshow> newShows = new List<cAirshow>();

        foreach (cAirshow ashow in asgRight.Airshows.myShows)
        {
          //List<cPly> plys = Ply.Where(ply => ply.SeqId == SeqId).ToList();
          //List<cAirshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.CompareYears(ashow)).ToList();
          List<cAirshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.CompareYears(ashow)).ToList();
          if (foundAirshows.Count > 0)
            continue;
          newShows.Add(ashow);
        }

        foreach (cAirshow newAirshow in newShows)
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
      string lines = cAirshow.GetTabOutput(myMergedShows);
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
            cAirshowWeekend asw = new cAirshowWeekend(dateTime);

            List<cAirshow> airshowsthisweek = myAirshows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
            airshowsthisweek = myFilteredAirshows.Where(x => x.WeekNumber == asw.weekofyear).ToList();

            List<cAirshow> mergedAirshowsThisweek = myMergedShows.Where(x => x.WeekNumber == asw.weekofyear).ToList();

            List<string> shownames = new List<string>();

            foreach (cAirshow airshow in airshowsthisweek)
            {
              shownames.Add(airshow.ToString());
            }
            if (shownames.Count > 0)
            {
              foreach (cAirshow airshow in airshowsthisweek)
              {
                lstBoxShows.Items.Add(airshow);
              }
              LoadShowGrid(airshowsthisweek, $"Airshows week of {cell.ToString()}:");
            }

            chklstbox_diff.Items.Clear();
            if (mergedAirshowsThisweek.Count > 0)
            {
              foreach (cAirshow airshow in mergedAirshowsThisweek)
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
        cAirshow theAirshow = (cAirshow)lstBoxShows.Items[lstBoxShows.SelectedIndex];
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
      cAirshow ashow = new cAirshow();
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

        cAirshowGroup asgLeft = Electroimpact.XmlSerialization.Serializer.Load<cAirshowGroup>(myASGCompare.sFileNameLeft);
        txtOutput.Lines = cAirshow.GetLines(asgLeft.Airshows.myShows);
        myFilteredAirshows = asgLeft.Airshows.myShows.ToList();

        cAirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<cAirshowGroup>(myASGCompare.sFileNameRight);
        txtRight.Lines = cAirshow.GetLines(asgRight.Airshows.myShows);

        List<cAirshow> newShows = new List<cAirshow>();

        foreach (cAirshow ashow in asgRight.Airshows.myShows)
        {

          List<cAirshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.Equals(ashow, false)).ToList();
          if (foundAirshows.Count > 0)
            continue;
          newShows.Add(ashow);
        }
        myMergedShows.Clear();
        myMergedShows = newShows;

        //txtMerged.Lines = cAirshow.GetLines(myMergedShows);



        foreach (cAirshow ashow in newShows)
        {
          chklstbox_diff.Items.Add(ashow);



        }

        this.Enabled = true;
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      List<cAirshow> newairshows = myAirshows.ToList();
      List<cAirshow> checkedShows = new List<cAirshow>();
      List<int> indicesToNuke = new List<int>();

      for (int ii = 0; ii < chklstbox_diff.Items.Count; ii++)
      {
        if (chklstbox_diff.GetItemChecked(ii) == true)
        {
          checkedShows.Add((cAirshow)chklstbox_diff.Items[ii]);
          indicesToNuke.Add(ii);
        }
      }

      foreach (cAirshow ashow in checkedShows)
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
      cRegions rgn = new cRegions();
      string clipboard = Clipboard.GetText();
      rgn.BuildFile(clipboard);
      cRegions.SaveMe(rgn, $@"C:\test\Regons.xml");
    }

    private void btnSetRegionFile_Click(object sender, EventArgs e)
    {
      Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(myFormState);

      DialogResult dr = DialogResult.OK;
      dr = sb.showDialog();

      if (dr == DialogResult.OK)
      {
        cAirshowFormState.SaveMe(myFormState);
        myRegions = cRegions.LoadMe(myFormState.fnRegions);
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
      List<cAirshow> FilteredAirshows = new List<cAirshow>();

      foreach (cAirshow airshow in myAirshows)
      {
        string rgn = "";
        string state = airshow.location.State.ToUpper();
        try
        {
          rgn = myRegions.Regions[state];
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
      txtOutput.Lines = cAirshow.GetLines(myFilteredAirshows);
      ColorGrid(myFilteredAirshows);
    }

    private void generateCallListToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<cAirshow> CallReport = myAirshows.Where(x => x.Status == eStatus.pursue || x.Status == eStatus.maybe).ToList();

      CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

      List<string> calltheseguys = new List<string>();
      calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");
      foreach (cAirshow ashow in CallReport)
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
      List<cAirshow> CallReport = myAirshows.Where(x => x.Status == eStatus.contract).ToList();

      CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

      List<string> calltheseguys = new List<string>();
      calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");

      foreach (cAirshow ashow in CallReport)
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
      List<cAirshow> CallReport = myAirshows.Where(x => x.Status == eStatus.pursue || x.Status == eStatus.verbal || x.Status == eStatus.NO || x.Status == eStatus.contract || x.Status == eStatus.maybe ).ToList();

      CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

      List<string> calltheseguys = new List<string>();
      foreach (cAirshow ashow in CallReport)
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
      List<cAirshow> CallReport = myFilteredAirshows.ToList();
      CallReport = CallReport.OrderBy(x => x.WeekNumber).ToList();

      List<string> calltheseguys = new List<string>();
      calltheseguys.Add($"Date\tStatus\tName\tLocation\tNotes\tContact");
      foreach (cAirshow ashow in CallReport)
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
        cAirshow ashow = lstBoxShows.SelectedItem as cAirshow;

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

      List<cAirshow> new_airshows = new List<cAirshow>();
      for (int ii = 0; ii < chklstbox_diff.Items.Count; ii++)
      {
        //if (chklstbox_diff.GetItemChecked(ii) == true)
          new_airshows.Add((cAirshow)chklstbox_diff.Items[ii]);
      }
      List<string> Potential_Duplicates = new List<string>();

      foreach (cAirshow ashow in new_airshows)
      {
        List<cAirshow> lstDuplicates = myAirshows.Where(airshow => airshow.location.Equals(ashow.location)).ToList();

        if (lstDuplicates.Count > 0)
        {
          foreach (cAirshow adup in lstDuplicates)
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
      if( dr == DialogResult.OK)
      {
        List<cAirshow> ret = new List<cAirshow>();
        Clipboard.Clear();
        switch (SearchTerms.SearchFiled)
        {
          case cSearchTerms.eSearchField.ContactName:
            {
              foreach(cAirshow ashow in myFilteredAirshows)
              {
                if (ashow.Contacts.contact.Count > 0)
                {
                  foreach (cContact contact in ashow.Contacts.contact)
                  {
                    if(contact.name.ToLower().Contains(SearchTerms.szSearchTerm.ToLower()))
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
        if( ret.Count > 0)
        {
          Clipboard.Clear();
          ret = ret.OrderBy(c => c.WeekNumber).ToList();
          string lines = cAirshow.GetTabOutput(ret);
          Clipboard.SetText(lines);

          {
            PopupForm puf = new PopupForm();
            puf.TextBox1.Lines = cAirshow.GetLines(ret);
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

  public class cAirshow
  {
    [Display(DisplayName = "Start Date (yyyy-MM-DD):")]
    public string date_start;
    [Display(DisplayName = "Finish Date (yyyy-MM-DD):")]
    public string date_finish;
    public string name_airshow;
    public cLocation location = new cLocation();
    public cPerformers Performers = new cPerformers();
    public cContacts Contacts = new cContacts();
    public string Notes_AirshowStuff = "";
    public enum eStatus
    {
      contract = 0,
      verbal = 1,
      maybe = 2,
      pursue = 3,
      none = 4,      
      NO = 5
    };

    public eStatus Status = eStatus.none;


    /*
     * Tagging a field with [XMLElement] puts the data on a new line
     * Tagging a field with [XMLAttribute] puts the data "in-line"
     * You can always override the name with '("newname")' after the XMLTag
     * Wrapper Classes are for XML Display only.  They are not actually necessary.  
     */

    #region Wrapper Classes
    public class cPerformers
    {
      [XmlElement("Performer")]
      public List<string> performer = new List<string>();
    }

    public class cContacts
    {
      [XmlElement("Contact")]
      public List<cContact> contact = new List<cContact>();
    }
    #endregion

    #region Helper Classes
    public class cLocation
    {
      [XmlIgnore]
      public string? rawstring;
      public string? city;
      public string? state;

      public string State
      {
        get
        {
          if (String.IsNullOrEmpty(state) || state == "")
          {
            if (rawstring == null)
              return "";
            string[] dog = rawstring.Split(',');
            if (dog.Length > 1)
            {
              state = dog[1].Trim();
            }
          }
          return state;
        }
      }

      public string City
      {
        get
        {
          if (String.IsNullOrEmpty(city) || city == "")
          {
            if (String.IsNullOrEmpty(rawstring) || rawstring == "")
              return "";
            string[] dog = rawstring.Split(',');
            if (dog.Length > 0)
            {
              csString oldshitty = new csString();
              city = dog[0].Trim();
              oldshitty.KillChar(ref city, '\"');
            }
          }
          return city;
        }
      }

      public override string ToString()
      {
        return $"{City}, {State}";
      }
      public override bool Equals(object? obj)
      {
        cLocation other = obj as cLocation;
        if (other == null) return false;

        string city1 = other.City.Contains('-') ? other.City.Substring(0, other.City.IndexOf("-")) : other.City;
        string city2 = City.Contains('-') ? City.Substring(0, City.IndexOf("-")) : City;

        if (city1.ToLower() == city2.ToLower() && other.State.ToUpper() == State.ToUpper())
        {


          return true;
        }
        return false;
      }
    }


    public class cContact
    {
      public string name;
      public string phone;
      public string address;

      public cContact()
      { }

    }
    #endregion

    #region Class Functions
    public cAirshow()
    { }

    public int Days
    {
      get
      {
        string[] start = date_start.Split('-');
        string[] end = date_finish.Split('-');
        System.DateTime dstart = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
        System.DateTime dfinish = new DateTime(int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2]));
        TimeSpan days = dfinish - dstart;
        return (int)days.TotalDays + 1;
      }
    }

    public int WeekNumber
    {
      get
      {
        string[] start = date_start.Split('-');
        string[] end = date_finish.Split('-');
        System.DateTime dstart = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
        System.DateTime dfinish = new DateTime(int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2]));
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;
        return cal.GetWeekOfYear(dstart, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
      }
    }

    public int Year
    {
      get
      {
        string[] start = date_start.Split('-');
        System.DateTime dstart = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
        return dstart.Year;
      }
    }

    #endregion

    #region Overrides
    public override bool Equals(object? obj)
    {
      return Equals(obj, true);
    }

    public bool Equals(object? obj, bool checkName = true)
    {
      cAirshow? anAirshow = obj as cAirshow;
      if (anAirshow == null) return false;

      bool NameOK = !checkName || anAirshow.name_airshow == name_airshow;

      if (anAirshow.WeekNumber == WeekNumber && anAirshow.location.Equals(location) && anAirshow.Year == Year && NameOK)
        return true;

      return false;
    }

    public override string ToString()
    {
      return $"{name_airshow.ToString()} - {location.city}, {location.State}, {date_start}";
    }

    public bool CompareYears(object? obj)
    {

      cAirshow? anAirshow = obj as cAirshow;
      if (anAirshow == null) return false;

      if (anAirshow.location.Equals(location) || CompareNames(anAirshow.name_airshow))//&& anAirshow.name_airshow == name_airshow) - sometimes name changes by year.  
        return true;

      return false;

    }

    public bool CompareNames(object? obj)
    {
      string? name = obj as string;
      if (name == null) return false;
      if (name.ToUpper() == this.name_airshow.ToUpper())
        return true;

      csString cs = new csString();
      string name_clean = this.name_airshow.ToUpper();
      char[] kill = { ',', '-', '$', '&', '(', ')', '\t' };
      foreach (char killchar in kill)
      {
        cs.KillChar(ref name_clean, killchar);
      }

      int n = 0;
      string[] ar_name_clean = name_clean.Split(' ');
      foreach (string stest in ar_name_clean)
      {
        if (name.ToUpper().Contains(stest))
          n++;
      }
      if ((n == 1 && ar_name_clean.Length == 1) || (n == 2 && ar_name_clean.Length == 2) || n > 2)
        return true;

      return false;

    }
    #endregion

    #region static functions
    public static string[] GetLines(List<cAirshow> airshows)
    {
      List<string> strings = new List<string>();

      foreach (cAirshow ashow in airshows)
      {
        string sshow = ashow.date_start + "\t" + ashow.date_finish + "\t" + ashow.location.City + ", " + ashow.location.State + "\t" + ashow.name_airshow;



        sshow = ashow.date_start.Length <= 10 ? ashow.date_start.PadRight(10) : ashow.date_start.Substring(0, 10);
        sshow += ashow.date_finish.Length <= 11 ? ashow.date_finish.PadLeft(11) : ashow.date_finish.Substring(0, 11);
        sshow += ashow.location.City.Length <= 30 ? ashow.location.City.PadLeft(30) : ashow.location.City.Substring(0, 30);
        sshow += ashow.location.State.Length <= 2 ? ", " + ashow.location.State.PadLeft(2) : ", " + ashow.location.State.Substring(0, 2);
        sshow += " - ";
        sshow += ashow.name_airshow.Length <= 50 ? ashow.name_airshow.PadRight(50) : ashow.name_airshow.Substring(0, 50);
        strings.Add(sshow);
      }

      return strings.ToArray();
    }
    public static void LoadFile(cAirshowFileParserSetupTool inputfile, List<cAirshow> airshows)
    {
      string[] lines;
      try
      {
        lines = System.IO.File.ReadAllLines(inputfile.sFileName);
      }
      catch(Exception ex) 
      {
        MessageBox.Show(ex.Message);
        return;
      }

      string ScheduleYear = inputfile.AirshowYear.ToString();

      //Electroimpact.FileParser.cFileParse fp = new Electroimpact.FileParser.cFileParse();

      if (inputfile.eFileSource == cAirshowFileParserSetupTool.efilesource.ICAS)
      {
        for (int ii = 0; ii < lines.Length; ii++)
        {
          if (lines[ii].StartsWith(ScheduleYear))
          {//we got one.

            cAirshow airshow = new cAirshow();
            airshow.date_start = lines[ii++];
            airshow.date_finish = lines[ii++];

            string airshowname = lines[ii++].ToLower();
            if (airshowname.Contains("air show"))
              airshowname = airshowname.Replace("air show", "Airshow");

            airshow.name_airshow = airshowname;
            airshow.location.rawstring = lines[ii++];

            List<string> airshowdata = new List<string>();

            int kk = 0;
            bool bPerformers = false;
            for (int jj = ii; jj < lines.Length; jj++)
            {
              if (lines[jj].StartsWith(ScheduleYear))
              {
                //we made it to the start of the next airshow;
                cContact acontact = new cContact();
                foreach (string s in airshowdata)
                {
                  if (s.StartsWith("-")) //we have performers
                  {
                    string addthis = s.Substring(2);  //get rid of the "- "
                    airshow.Performers.performer.Add(addthis);
                    bPerformers = true;
                    continue;
                  }
                  if (kk == 0 && bPerformers == false)
                  {//need to throw out the line.
                    bPerformers = true;  //there are no performers, but we've captured the line we want to capture.
                    continue;
                  }
                  if (kk == 0)
                  {
                    acontact.name = s;
                    kk = 1;
                    continue;
                  }
                  if (kk == 1)
                  {
                    acontact.phone = s;
                    kk = 2;
                    continue;
                  }
                  if (kk == 2)
                  {
                    string darn = "someting wong";
                  }
                }
                ii = jj - 1;
                airshow.Contacts.contact.Add(acontact);
                //airshow.contacts.Add(acontact);
                airshows.Add(airshow);
                break;
              }
              airshowdata.Add(lines[jj]);
            }
          }
        }
      }
      else if (inputfile.eFileSource == cAirshowFileParserSetupTool.efilesource.AirshowStuff)
      {
        for (int ii = 1; ii < lines.Length; ii++)
        {

          string linein = lines[ii];

          csString stringer = new csString();

          //if (linein.Contains("\""))
          //  MessageBox.Show("WTF");

          stringer.KillChar(ref linein, '\"');

          string[] line = linein.Split('\t');

          if (line.Length < 7)
            continue; //something wrong with this line. 


          cAirshow airshow = new cAirshow();
          string[] sDate = line[0].Split('/');
          if (sDate.Length < 3)
            continue;
          airshow.date_start = sDate[2] + "-" + sDate[0] + "-" + sDate[1];


          sDate = line[1].Split('/');
          if (sDate.Length < 3)
            continue;
          airshow.date_finish = sDate[2] + "-" + sDate[0] + "-" + sDate[1];

          string airshowname = line[4].ToLower();
          if (airshowname.Contains("air show"))
            airshowname = airshowname.Replace("air show", "Airshow");

          airshow.name_airshow = airshowname;
          airshow.location.rawstring = line[5].Replace(',', '-') + "," + line[6];

          if (line.Length > 9)
          {
            string sPerformers = line[9];
            string[] Performers = sPerformers.Split(@"/<br>");
            for (int j = 0; j < Performers.Length; j++)
            {
              airshow.Performers.performer.Add(Performers[j]);
            }
          }

          if (line.Length > 10)
          {
            airshow.Notes_AirshowStuff = line[10];
          }

          //int airshowyear;
          if (inputfile.AirshowYear == airshow.Year)
            airshows.Add(airshow);
        }
      }
    }
    internal static string GetTabOutput(List<cAirshow> airshows)
    {
      string outputs = "Week\tStart\tFinish\tDays\tCity\tState\tName\tContact\tPhone\n";
      foreach (cAirshow airshow in airshows)
      {


        string outputline = airshow.WeekNumber.ToString();
        outputline += "\t" + airshow.date_start;
        outputline += "\t" + airshow.date_finish;
        outputline += "\t" + airshow.Days;
        outputline += "\t" + airshow.location.City;
        outputline += "\t" + airshow.location.State;
        outputline += "\t" + airshow.name_airshow;
        foreach (cContact acontact in airshow.Contacts.contact)
        {
          outputline += "\t" + acontact.name;
          outputline += "\t" + acontact.phone;
        }
        outputs += outputline + "\n";
      }
      return outputs;
    }
    #endregion
    //Filter = @"All Files|*.*|Text File (.txt)|*.txt|Word File (.docx ,.doc)|*.docx;*.doc|PDF (.pdf)|*.pdf|Spreadsheet (.xls ,.xlsx)|  *.xls ;*.xlsx|Presentation (.pptx ,.ppt)|*.pptx;*.ppt",

  }

  public class cCalenderYear
  {

    public class cAirshowWeekend
    {
      public DateTime Date;

      public cAirshowWeekend(DateTime date)
      {
        Date = date;
      }


      public override string ToString()
      {
        return $"{Date.Month} - {Date.Day}";
      }

      public int weekofyear
      {
        get
        {
          // Get the week number using the current culture's calendar
          CultureInfo culture = CultureInfo.CurrentCulture;
          Calendar calendar = culture.Calendar;
          int weekNumber = calendar.GetWeekOfYear(Date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
          return weekNumber;
        }
      }
    }

    public static List<cAirshowWeekend> GetSaturdaysList(int year)
    {
      // Create a list to store the Saturdays.
      List<DateTime> saturdays = new List<DateTime>();

      // Loop through the months in the year.
      for (int month = 1; month <= 12; month++)
      {
        // Get the first day of the month.
        DateTime firstDayOfMonth = new DateTime(year, month, 1);

        // Check if the first day of the month is a Saturday.
        if (firstDayOfMonth.DayOfWeek == DayOfWeek.Saturday)
        {
          // Add the first day of the month to the list of Saturdays.
          saturdays.Add(firstDayOfMonth);
        }

        // Loop through the days of the month.
        for (int day = 2; day <= DateTime.DaysInMonth(year, month); day++)
        {
          // Get the current day.
          DateTime currentDay = new DateTime(year, month, day);

          // Check if the current day is a Saturday.
          if (currentDay.DayOfWeek == DayOfWeek.Saturday)
          {
            // Add the current day to the list of Saturdays.
            saturdays.Add(currentDay);
          }
        }
      }

      List<cAirshowWeekend> weekends = new List<cAirshowWeekend>();
      for(int ii = 0; ii < saturdays.Count; ii++)
      {
        weekends.Add(new cAirshowWeekend(saturdays[ii]));
      }

      return weekends;
    }
  }
}