using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirshowSchedules;
    using Electroimpact.SettingsFormBuilderV2.Attributes;
    using System.Data;
    using System.Globalization;
    using System.Xml.Serialization;
    using static AirshowSchedules.Airshow;
    using static AirshowSchedules.cCalenderYear;


public partial class formMain
{
    private void LoadGrid(int YearOfInterest)
    {
        // Unbind the DataGridView from any data source
        dgvCalendar.DataSource = null;
    
        // Clear existing rows and columns
        dgvCalendar.Rows.Clear();
        dgvCalendar.Columns.Clear();
    
        // Creates the Saturdays in a given year. Formats the resulting grid.
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
    
        // Bind the DataTable to the DataGridView
        dgvCalendar.DataSource = dataTable;
    
        int rowCount = dgvCalendar.Rows.Count;
        int columnCount = dgvCalendar.Columns.Count;
    
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
    private void ColorGrid(List<Airshow> theseshows)
    {
        //this colors the grid and puts an airshow name in there if there is some sort of status associated with it. 
        //if there is a show this weekend at all, the cell is bolded regardless of state. 
        List<Airshow> actionShows = theseshows.Where(x => x.Status != eStatus.none && x.Status != eStatus.NO).ToList();

        bool refreshAll = theseshows.Count == myAirshowGroup.GetAirshowsForYear().Count;


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
                        int year = GetYearOfInterest();
                        DateTime dateTime = new DateTime(year, int.Parse(weekendinquestion[0]), int.Parse(weekendinquestion[2]));
                        AirshowWeekend asw = new AirshowWeekend(dateTime);
                        List<Airshow> showsthisweekend = actionShows.Where(x => x.WeekNumber == asw.weekofyear).ToList();
                        showsthisweekend = showsthisweekend.OrderBy(x => x.Status).ToList();
                        if (showsthisweekend.Count == 0)
                        {
                            if (refreshAll)
                            {
                                // string[] weekendinquestion2 = cellText.Split(' ');
                                // string outputToConsole = "";
                                // foreach (string s in weekendinquestion)
                                // {
                                //     outputToConsole += s + " ";
                                // }
                                // Console.WriteLine($"Debugging ColorGrid: {outputToConsole} string array length: {weekendinquestion2.Length}");
                                cell.Style.BackColor = Color.White;
                                cell.Value = asw;
                            }
                            continue;
                        }

                        Airshow ashowwithInterest = showsthisweekend[0];

                        if (asw.weekofyear == ashowwithInterest.WeekNumber)
                        {
                            cell.Value = asw;
                            
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
                    int year = GetYearOfInterest();
                    DateTime dateTime = new DateTime(year, int.Parse(weekendinquestion[0]), int.Parse(weekendinquestion[2]));
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
}
