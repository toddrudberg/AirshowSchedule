using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AirshowSchedules
{
    internal class formSearch : Form
    {

        private bool isFormLoading = true;
        private AirshowGroup airshowGroup { get; set; }
        private List<cContact> contacts { get; set; }
        private Regions regions { get; set; }

        private TextBox txtShowName;
        private TextBox txtShowLocation;
        private CheckedListBox clbShowRegion;
        private TextBox txtContactName;
        private Label lblShowName;
        private Label lblShowLocation;
        private ListBox lstAirshows;
        private ListBox lstContacts;
        private Button btnFilterClearAll;
        private Button btnFilterSetAll;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label label1;
        private Label label2;
        private Label label3;
        private CheckedListBox clbStatus;
        private Label label4;
        private Button btnGenerateReport;
        private Label lblContactName;

        public formSearch(AirshowGroup group, List<cContact> contacts, Regions regions)
        {
            airshowGroup = group;
            this.contacts = contacts;
            this.regions = regions;

            InitializeComponent();
            InitializeForm(GetRegions());
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            //fill in the clbStatus with the enumStatus values

        }

        private void InitializeComponent()
        {
            lblShowName = new Label();
            txtShowName = new TextBox();
            lblShowLocation = new Label();
            txtShowLocation = new TextBox();
            clbShowRegion = new CheckedListBox();
            lblContactName = new Label();
            txtContactName = new TextBox();
            lstAirshows = new ListBox();
            lstContacts = new ListBox();
            btnFilterClearAll = new Button();
            btnFilterSetAll = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            clbStatus = new CheckedListBox();
            label4 = new Label();
            btnGenerateReport = new Button();
            SuspendLayout();
            // 
            // lblShowName
            // 
            lblShowName.Location = new Point(20, 20);
            lblShowName.Name = "lblShowName";
            lblShowName.Size = new Size(100, 23);
            lblShowName.TabIndex = 0;
            lblShowName.Text = "Show Name:";
            // 
            // txtShowName
            // 
            txtShowName.Location = new Point(150, 20);
            txtShowName.Name = "txtShowName";
            txtShowName.Size = new Size(200, 23);
            txtShowName.TabIndex = 1;
            txtShowName.TextChanged += txtShowName_TextChanged;
            // 
            // lblShowLocation
            // 
            lblShowLocation.Location = new Point(20, 60);
            lblShowLocation.Name = "lblShowLocation";
            lblShowLocation.Size = new Size(100, 23);
            lblShowLocation.TabIndex = 2;
            lblShowLocation.Text = "Show Location:";
            // 
            // txtShowLocation
            // 
            txtShowLocation.Location = new Point(150, 60);
            txtShowLocation.Name = "txtShowLocation";
            txtShowLocation.Size = new Size(200, 23);
            txtShowLocation.TabIndex = 3;
            txtShowLocation.TextChanged += txtShowLocation_TextChanged;
            // 
            // clbShowRegion
            // 
            clbShowRegion.Location = new Point(503, 20);
            clbShowRegion.Name = "clbShowRegion";
            clbShowRegion.Size = new Size(200, 184);
            clbShowRegion.TabIndex = 0;
            clbShowRegion.ItemCheck += clbShowRegionItemsCheckedChanged;
            // 
            // lblContactName
            // 
            lblContactName.Location = new Point(20, 95);
            lblContactName.Name = "lblContactName";
            lblContactName.Size = new Size(100, 23);
            lblContactName.TabIndex = 6;
            lblContactName.Text = "Contact Name:";
            // 
            // txtContactName
            // 
            txtContactName.Location = new Point(150, 95);
            txtContactName.Name = "txtContactName";
            txtContactName.Size = new Size(200, 23);
            txtContactName.TabIndex = 7;
            txtContactName.TextChanged += txtContactName_TextChanged;
            // 
            // lstAirshows
            // 
            lstAirshows.FormattingEnabled = true;
            lstAirshows.ItemHeight = 15;
            lstAirshows.Location = new Point(20, 225);
            lstAirshows.Name = "lstAirshows";
            lstAirshows.Size = new Size(550, 454);
            lstAirshows.TabIndex = 9;
            lstAirshows.SelectedIndexChanged += lstAirshows_SelectedIndexChanged;
            // 
            // lstContacts
            // 
            lstContacts.FormattingEnabled = true;
            lstContacts.ItemHeight = 15;
            lstContacts.Location = new Point(576, 225);
            lstContacts.Name = "lstContacts";
            lstContacts.Size = new Size(191, 454);
            lstContacts.TabIndex = 10;
            lstContacts.SelectedIndexChanged += lstContacts_SelectedIndexChanged;
            // 
            // btnFilterClearAll
            // 
            btnFilterClearAll.Location = new Point(713, 20);
            btnFilterClearAll.Name = "btnFilterClearAll";
            btnFilterClearAll.Size = new Size(75, 23);
            btnFilterClearAll.TabIndex = 11;
            btnFilterClearAll.Text = "Clear All";
            btnFilterClearAll.UseVisualStyleBackColor = true;
            btnFilterClearAll.Click += btnFilterClearAll_Click;
            // 
            // btnFilterSetAll
            // 
            btnFilterSetAll.Location = new Point(713, 49);
            btnFilterSetAll.Name = "btnFilterSetAll";
            btnFilterSetAll.Size = new Size(75, 23);
            btnFilterSetAll.TabIndex = 12;
            btnFilterSetAll.Text = "Set All";
            btnFilterSetAll.UseVisualStyleBackColor = true;
            btnFilterSetAll.Click += btnFilterSetAll_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(576, 207);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 13;
            label1.Text = "Contacts";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 207);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 14;
            label2.Text = "Airshows";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(503, 2);
            label3.Name = "label3";
            label3.Size = new Size(76, 15);
            label3.TabIndex = 15;
            label3.Text = "Show Region";
            // 
            // clbStatus
            // 
            clbStatus.FormattingEnabled = true;
            clbStatus.Location = new Point(386, 20);
            clbStatus.Name = "clbStatus";
            clbStatus.Size = new Size(111, 130);
            clbStatus.TabIndex = 16;
            clbStatus.ItemCheck += clbStatus_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(386, 2);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 17;
            label4.Text = "Status";
            // 
            // btnGenerateReport
            // 
            btnGenerateReport.Location = new Point(25, 176);
            btnGenerateReport.Name = "btnGenerateReport";
            btnGenerateReport.Size = new Size(127, 23);
            btnGenerateReport.TabIndex = 18;
            btnGenerateReport.Text = "Generate Report";
            btnGenerateReport.UseVisualStyleBackColor = true;
            btnGenerateReport.Click += btnGenerateReport_Click;
            // 
            // formSearch
            // 
            ClientSize = new Size(800, 711);
            Controls.Add(btnGenerateReport);
            Controls.Add(label4);
            Controls.Add(clbStatus);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnFilterSetAll);
            Controls.Add(btnFilterClearAll);
            Controls.Add(lstContacts);
            Controls.Add(lstAirshows);
            Controls.Add(lblShowName);
            Controls.Add(txtShowName);
            Controls.Add(lblShowLocation);
            Controls.Add(txtShowLocation);
            Controls.Add(clbShowRegion);
            Controls.Add(lblContactName);
            Controls.Add(txtContactName);
            Name = "formSearch";
            Text = "Search Form";
            Load += formSearch_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private Regions GetRegions()
        {
            return regions;
        }

        private void InitializeForm(Regions regions)
        {
            // Populate the ComboBox with regions
            foreach (var region in regions.GetRegionList())
            {
                clbShowRegion.Items.Add(region);
            }
            //select all regions by default
            for (int i = 0; i < clbShowRegion.Items.Count; i++)
            {
                clbShowRegion.SetItemChecked(i, true);
            }

            this.clbStatus.DataSource = Enum.GetValues(typeof(Airshow.eStatus));
            //select all the status options
            for (int i = 0; i < clbStatus.Items.Count; i++)
            {
                clbStatus.SetItemChecked(i, true);
            }

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
        }

        private void txtContactName_TextChanged(object sender, EventArgs e)
        {
            performSerch();
        }

        private void performSerch(List<object> updatedCheckedItems = null, List<object> updateStatusCheckedItems = null)
        {
            //we dont't want to modify our masterlist.
            List<Airshow> searchAirshows = Airshow.DeepCopy(airshowGroup.Airshows.myShows);
            //pair down searchAirshow by region

            searchAirshows = filterByRegion(searchAirshows, updatedCheckedItems);
            searchAirshows = filterByStatus(searchAirshows, updateStatusCheckedItems);


            List<cContact> searchContacts = cContact.DeepCopy(this.contacts);


            bool allSearchTermsEmpty = txtContactName.Text == "" && txtShowName.Text == "" && txtShowLocation.Text == "";

            List<cContact> foundContacts = new List<cContact>();
            if (txtContactName.Text != "")
            {
                //lets look through the contacts and find any that contain this sequence of chracters
                foundContacts.Clear();

                foreach (cContact contact in searchContacts)
                {
                    if (contact.name.Trim().ToLower().Contains(txtContactName.Text.Trim().ToLower()))
                    {
                        foundContacts.Add(contact);
                    }
                }
            }

            lstContacts.Items.Clear();
            foreach (cContact contact in foundContacts)
            {
                lstContacts.Items.Add(contact);
            }

            List<Airshow> foundAirshowByContact = new List<Airshow>();
            if (foundContacts.Count > 0)
            {
                foreach (cContact contact in foundContacts)
                {
                    foreach (Airshow show in searchAirshows)
                    {
                        if (show.contactIds.Contains(contact.ID))
                        {
                            foundAirshowByContact.Add(show);
                        }
                    }
                }
            }

            //list to this for the location
            List<Airshow> foundLocations = new List<Airshow>();
            if (txtShowLocation.Text != "")
            {
                foreach (Airshow show in searchAirshows)
                {
                    if (show.location.city.Trim().ToLower().Contains(txtShowLocation.Text.Trim().ToLower()))
                    {
                        foundLocations.Add(show);
                    }
                }
            }

            List<Airshow> foundAirshowNames = new List<Airshow>();
            if (txtShowName.Text != "")
            {
                foreach (Airshow show in searchAirshows)
                {
                    if (show.name_airshow.Trim().ToLower().Contains(txtShowName.Text.Trim().ToLower()))
                    {
                        foundAirshowNames.Add(show);
                    }
                }
            }

            List<Airshow> displayTheseAirshows = new List<Airshow>();
            displayTheseAirshows = foundAirshowByContact;
            //lets add the locations
            foreach (Airshow show in foundLocations)
            {
                if (!displayTheseAirshows.Contains(show))
                {
                    displayTheseAirshows.Add(show);
                }
            }

            //lets add the names
            foreach (Airshow show in foundAirshowNames)
            {
                if (!displayTheseAirshows.Contains(show))
                {
                    displayTheseAirshows.Add(show);
                }
            }

            if (displayTheseAirshows.Count == 0 && allSearchTermsEmpty)
            {
                displayTheseAirshows = searchAirshows;
            }

            lstAirshows.Items.Clear();
            foreach (Airshow show in displayTheseAirshows)
            {
                lstAirshows.Items.Add(show);
            }

        }

        private void txtShowName_TextChanged(object sender, EventArgs e)
        {
            performSerch();
        }

        private void txtShowLocation_TextChanged(object sender, EventArgs e)
        {
            performSerch();
        }

        private void lstAirshows_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAirshows.SelectedItems.Count > 0)
            {
                Airshow theAirshow = (Airshow)lstAirshows.Items[lstAirshows.SelectedIndex];
                List<cContact> myContacts = cContact.DeepCopy(contacts);
                using (AirshowEditForm editForm = new AirshowEditForm(theAirshow, myContacts))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // The Airshow object has been updated
                        //SaveAirshowSchedule(false); // Save the updated airshow schedule
                    }
                }
            }
        }

        private void btnFilterClearAll_Click(object sender, EventArgs e)
        {
            isFormLoading = true;
            //clear all checked items in clbShowRegion
            for (int i = 0; i < clbShowRegion.Items.Count; i++)
            {
                clbShowRegion.SetItemChecked(i, false);
            }
            isFormLoading = false;
            performSerch();
        }

        private void btnFilterSetAll_Click(object sender, EventArgs e)
        {
            isFormLoading = true;
            //set all checked items in clbShowRegion
            for (int i = 0; i < clbShowRegion.Items.Count; i++)
            {
                clbShowRegion.SetItemChecked(i, true);
            }
            isFormLoading = false;
            performSerch();
        }

        private List<Airshow> filterByStatus(List<Airshow> TheseAirshows, List<object> updatedCheckedItems = null)
        {
            // Use the updatedCheckedItems if provided, otherwise fall back to clbStatus.CheckedItems
            var checkedItems = updatedCheckedItems ?? clbStatus.CheckedItems.Cast<object>().ToList();

            // Initialize a new list to store the filtered airshows
            List<Airshow> FilteredAirshows = new List<Airshow>();

            // Iterate over the input list of airshows
            foreach (Airshow airshow in TheseAirshows)
            {
                // Check if the status matches any of the checked items
                foreach (Airshow.eStatus status in checkedItems)
                {
                    if (airshow.Status == status)
                    {
                        FilteredAirshows.Add(airshow);
                    }
                }
            }

            // Return the filtered list of airshows
            return FilteredAirshows;
        }
        private List<Airshow> filterByRegion(List<Airshow> TheseAirshows, List<object> updatedCheckedItems = null)
        {
            // Use the updatedCheckedItems if provided, otherwise fall back to clbShowRegion.CheckedItems
            var checkedItems = updatedCheckedItems ?? clbShowRegion.CheckedItems.Cast<object>().ToList();

            // Initialize a new list to store the filtered airshows
            List<Airshow> FilteredAirshows = new List<Airshow>();

            // Iterate over the input list of airshows
            foreach (Airshow airshow in TheseAirshows)
            {
                string rgn = "";
                string state = airshow.location.state.ToUpper();

                try
                {
                    // Map the state's region using the regions dictionary
                    rgn = regions.myRegions[state];
                }
                catch
                {
                    // Skip this airshow if the region mapping fails
                    continue;
                }

                // Check if the region (rgn) matches any of the checked items
                if (checkedItems.Contains(rgn))
                {
                    FilteredAirshows.Add(airshow);
                }
            }

            // Return the filtered list of airshows
            return FilteredAirshows;
        }


        private void clbShowRegionItemsCheckedChanged(object sender, ItemCheckEventArgs e)
        {
            if (!isFormLoading)
            {
                // Clone current CheckedItems list into a new collection
                var checkedItems = clbShowRegion.CheckedItems.Cast<object>().ToList();

                // Apply the pending change
                var changedItem = clbShowRegion.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    checkedItems.Add(changedItem);
                }
                else
                {
                    checkedItems.Remove(changedItem);
                }
                performSerch(checkedItems);
            }
        }
        private void clbStatus_SelectedIndexChanged(object sender, ItemCheckEventArgs e)
        {
            if (!isFormLoading)
            {
                // Clone current CheckedItems list into a new collection
                var checkedItems = clbStatus.CheckedItems.Cast<object>().ToList();

                // Apply the pending change
                var changedItem = clbStatus.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    checkedItems.Add(changedItem);
                }
                else
                {
                    checkedItems.Remove(changedItem);
                }
                performSerch(null, checkedItems);
            }
        }
        private void formSearch_Load(object sender, EventArgs e)
        {
            isFormLoading = false;
            performSerch();
        }

        private void lstContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstContacts.SelectedItem != null)
            {
                cContact selectedContact = (cContact)this.lstContacts.SelectedItem;
                using (ContactEditForm contactEditForm = new ContactEditForm(selectedContact))
                {
                    if (contactEditForm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh the list box to reflect the updated contact
                        int selectedIndex = this.lstContacts.SelectedIndex;
                        this.lstContacts.Items[selectedIndex] = contactEditForm.Contact;
                    }
                }
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string result = "";
            string header = "";
            header += "Start Date\t";
            header += "End Date\t";
            header += "Airshow Name\t";
            header += "City\t";
            header += "State\t";
            header += "Status\t";
            header += "Undaunted Notes\t";
            int maxContacts = 1;


            foreach (Airshow show in lstAirshows.Items)
            {
                // Add airshow details
                result += show.date_start + "\t"
                        + show.date_finish + "\t"
                        + show.name_airshow + "\t"
                        + show.location.city + "\t"
                        + show.location.state + "\t"
                        + show.Status + "\t";

                // Add Undaunted notes with line breaks
                string undauntedNotes = "";
                foreach(string note in show.UndauntedNotes)
                {
                    undauntedNotes += note + "\r\n";
                }
                if( undauntedNotes.Length > 1 )
                    undauntedNotes = undauntedNotes.Substring(0, undauntedNotes.Length - 2);

                result += "\"" + undauntedNotes + "\"" + "\t";

                // Add contacts
                List<cContact> showContacts = cContact.getContacts(contacts, show);

                string showContactDeets = "";
                string stringShowContacts = "";

                int count = 0;
                foreach (cContact contact in showContacts)
                {                   
                    count++;
                    showContactDeets += contact.name + "\r\n";
                    if( contact.phone != null && contact.phone != "")
                        showContactDeets += contact.phone + "\r\n";

                    foreach( string email in contact.emailAddresses)
                    {
                        showContactDeets += email + "\r\n";
                    }
                    if( showContactDeets.Length > 2)
                        showContactDeets = showContactDeets.Substring(0, showContactDeets.Length - 2);

                    stringShowContacts += "\"" + showContactDeets + "\"" + "\t";
                    showContactDeets = "";
                    if( count > maxContacts)
                    {
                        maxContacts = count;
                    }
                }

                result += stringShowContacts + "\n";
                
            }
            for (int i = 0; i < maxContacts; i++)
            {
                header += "Contact " + (i + 1) + "\t";
            }
            result = header + "\n" + result;

            // Copy to clipboard
            Clipboard.SetText(result);
            // write out to a file on the desktop:
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = desktopPath + "\\AirshowReport.txt";
            System.IO.File.WriteAllText(filePath, result);
        }
    }
}