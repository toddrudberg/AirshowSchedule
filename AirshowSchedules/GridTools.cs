using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirshowSchedules
{
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using static AirshowSchedules.Airshow;

    public static class GridTools
    {
        public static void LoadShowGrid(DataGridView dataGridViewShows, List<Airshow> listtheseshows, string ColumnTItle = "Shows This Weekend")
        {
            // This is what happens when you select a weekend in the main grid. dataGridViewShows is the pop up to the right and it lists the shows for a given weekend.
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
    }
}
