using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AirshowSchedules
{
    public partial class CompareForm : Form
    {
        private List<Airshow> activeDB;

        public CompareForm(List<Airshow> newAirshows, List<Airshow> activeDB)
        {
            InitializeComponent();
            PopulateCheckedListBox(newAirshows);
            this.activeDB = activeDB;
        }

        private void InitializeComponent()
        {
            this.checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnManualMerge = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
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
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(12, 202);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 23);
            this.btnMerge.TabIndex = 1;
            this.btnMerge.Text = "Merge All";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnManualMerge
            // 
            this.btnManualMerge.Location = new System.Drawing.Point(93, 202);
            this.btnManualMerge.Name = "btnManualMerge";
            this.btnManualMerge.Size = new System.Drawing.Size(100, 23);
            this.btnManualMerge.TabIndex = 2;
            this.btnManualMerge.Text = "Merge Selected";
            this.btnManualMerge.UseVisualStyleBackColor = true;
            this.btnManualMerge.Click += new System.EventHandler(this.btnManualMerge_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(199, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(280, 202);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 237);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnManualMerge);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.checkedListBox);
            this.Name = "CompareForm";
            this.Text = "Compare Airshows";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CompareForm_FormClosing);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.CheckedListBox checkedListBox;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnManualMerge;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;

        private void PopulateCheckedListBox(List<Airshow> airshows)
        {
            foreach (var airshow in airshows)
            {
                checkedListBox.Items.Add(airshow, false);
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            foreach (var item in checkedListBox.Items)
            {
                Airshow airshow = (Airshow)item;
                activeDB.Add(airshow);
            }

            MessageBox.Show($"{checkedListBox.Items.Count} airshows merged successfully.");
            this.checkedListBox.Items.Clear();
        }

        private void btnManualMerge_Click(object sender, EventArgs e)
        {
            List<Airshow> airshowsToRemove = new List<Airshow>();
            foreach (var item in checkedListBox.CheckedItems)
            {
                Airshow airshow = (Airshow)item;
                activeDB.Add(airshow);
                airshowsToRemove.Add(airshow);
                
            }

            foreach (var airshow in airshowsToRemove)
            {
                checkedListBox.Items.Remove(airshow);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CompareForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}