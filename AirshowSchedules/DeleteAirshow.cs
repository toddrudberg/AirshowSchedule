using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirshowSchedules
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public partial class DeleteAirshowForm : Form
    {
        private List<Airshow> duplicateList;
        private List<Airshow> copiedList;

        public DeleteAirshowForm(List<Airshow> duplicateList, List<Airshow> copiedList)
        {
            InitializeComponent();
            this.duplicateList = duplicateList.OrderBy(airshow => airshow.name_airshow).ToList();
            PopulateCheckedListBox();
            this.copiedList = copiedList;
        }

        private void InitializeComponent()
        {
            this.checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBox
            // 
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.Location = new System.Drawing.Point(12, 12);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(360, 184);
            this.checkedListBox.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(297, 202);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // DeleteAirshowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 237);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.checkedListBox);
            this.Name = "DeleteAirshowForm";
            this.Text = "Delete Airshows";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.CheckedListBox checkedListBox;
        private System.Windows.Forms.Button btnDelete;

        private void PopulateCheckedListBox()
        {
            foreach (var airshow in duplicateList)
            {
                checkedListBox.Items.Add(airshow, false);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<Airshow> itemsToRemove = new List<Airshow>();

            foreach (var item in checkedListBox.CheckedItems)
            {
                itemsToRemove.Add((Airshow)item);
            }

            foreach (var item in itemsToRemove)
            {
                copiedList.Remove(item);
                duplicateList.Remove(item);
                checkedListBox.Items.Remove(item);
            }
        }
    }
}
