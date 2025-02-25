﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AirshowSchedules
{
    public partial class CompareForm : Form
    {
        private List<Airshow> activeDB;
        private List<cContact> activeContacts;
        private List<cContact> newContacts;

        public CompareForm(List<Airshow> newAirshows, List<cContact> newContacts, List<Airshow> activeDB, List<cContact> activeContacts, System.Windows.Forms.Form mainForm)
        {
            InitializeComponent(mainForm);
            CenterToMainForm(mainForm);
            PopulateCheckedListBox(newAirshows);
            
            this.activeDB = activeDB;
            this.activeContacts = activeContacts;
            this.newContacts = newContacts;
        }

        private void InitializeComponent(System.Windows.Forms.Form mainForm)
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
            this.checkedListBox.Size = new System.Drawing.Size((int)(mainForm.Width *.65), (int)(mainForm.Height*.5));
            this.checkedListBox.TabIndex = 0;
            
            
            int buttonHeight = 40;
            int buttonWidth = 150;
            int buttonLocationX = 12;
            int buttonStep = buttonWidth + buttonLocationX;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(buttonLocationX, this.checkedListBox.Bottom + 10);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnMerge.TabIndex = 1;
            this.btnMerge.Text = "Merge All";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnManualMerge
            // 
            buttonLocationX += buttonStep;
            this.btnManualMerge.Location = new System.Drawing.Point(buttonLocationX, this.checkedListBox.Bottom + 10);
            this.btnManualMerge.Name = "btnManualMerge";
            this.btnManualMerge.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnManualMerge.TabIndex = 2;
            this.btnManualMerge.Text = "Merge Selected";
            this.btnManualMerge.UseVisualStyleBackColor = true;
            this.btnManualMerge.Click += new System.EventHandler(this.btnManualMerge_Click);
            // 
            // btnCancel
            // 
            buttonLocationX += buttonStep;
            this.btnCancel.Location = new System.Drawing.Point(buttonLocationX, this.checkedListBox.Bottom + 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            buttonLocationX += buttonStep;
            this.btnClose.Location = new System.Drawing.Point(buttonLocationX, this.checkedListBox.Bottom + 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = @"Close && Save";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // 
            // CompareForm
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size((int)(mainForm.Width * 0.75), (int)(mainForm.Height * 0.75));
            this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(
                    mainForm.Location.X,
                    mainForm.Location.Y
                );
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
            foreach (Airshow airshowToAdd in checkedListBox.Items)
            {
                
                mergeContactAndAirshow(airshowToAdd);
                
            }
            this.checkedListBox.Items.Clear();
        }

        private void btnManualMerge_Click(object sender, EventArgs e)
        {
            List<Airshow> airshowsToRemove = new List<Airshow>();
            foreach (Airshow airshowToAdd in checkedListBox.CheckedItems)
            {
                mergeContactAndAirshow(airshowToAdd);
                airshowsToRemove.Add(airshowToAdd);
            }

            foreach (var airshow in airshowsToRemove)
            {
                checkedListBox.Items.Remove(airshow);
            }
        }

        private void mergeContactAndAirshow(Airshow airshowToAdd)
        {
            //since this list is made from a fresh download, the airshowID and contactID lists aren't synched up
            //we have to assign an airshowID and copy it to the contact
            //we have to add the new contactID to the airshow.contactIds.  
            //we have to remove the original contactID from the airshow.contactIds.

            //this should only return 1 contact because at this stage we aren't referencing the activeDB
            List<cContact> airshowContacts = cContact.getContacts(newContacts, airshowToAdd);
            int airshowIDtoDelete = airshowToAdd.ID;
            airshowToAdd.ID = activeDB.Max(a => a.ID) + 1;
            if (airshowContacts.Count > 0)
            {
                List<int> contactsToDelete = new List<int>();
                foreach (var contact in airshowContacts)
                {
                    contactsToDelete.Add(contact.ID);

                    //before adding the contact, we need to remove the bogus airshowID.  
                    contact.showIDs.Remove(airshowIDtoDelete);
                    if (!(contact.name == null || contact.name.Trim() == ""))
                    {
                        cContact.addContact(activeContacts, contact, airshowToAdd);
                    }
                }
            }

            activeDB.Add(airshowToAdd);
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

        private void CenterToMainForm(System.Windows.Forms.Form mainForm)
        {
            if (mainForm != null)
            {

            }
        }
    }
}