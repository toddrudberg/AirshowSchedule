using System;
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
        private Button btnSearch;
        private Label lblShowName;
        private Label lblShowLocation;
        private Label lblShowRegion;
        private ListBox lstAirshows;
        private ListBox lstContacts;
        private Button btnFilterClearAll;
        private Button btnFilterSetAll;
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
        }

        private void InitializeComponent()
        {
            lblShowName = new Label();
            txtShowName = new TextBox();
            lblShowLocation = new Label();
            txtShowLocation = new TextBox();
            lblShowRegion = new Label();
            clbShowRegion = new CheckedListBox();
            lblContactName = new Label();
            txtContactName = new TextBox();
            btnSearch = new Button();
            lstAirshows = new ListBox();
            lstContacts = new ListBox();
            btnFilterClearAll = new Button();
            btnFilterSetAll = new Button();
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
            // lblShowRegion
            // 
            lblShowRegion.Location = new Point(397, 20);
            lblShowRegion.Name = "lblShowRegion";
            lblShowRegion.Size = new Size(100, 23);
            lblShowRegion.TabIndex = 4;
            lblShowRegion.Text = "Show Region:";
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
            // btnSearch
            // 
            btnSearch.Location = new Point(20, 181);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 8;
            btnSearch.Text = "Search";
            btnSearch.Click += BtnSearch_Click;
            // 
            // lstAirshows
            // 
            lstAirshows.FormattingEnabled = true;
            lstAirshows.ItemHeight = 15;
            lstAirshows.Location = new Point(20, 210);
            lstAirshows.Name = "lstAirshows";
            lstAirshows.Size = new Size(550, 469);
            lstAirshows.TabIndex = 9;
            lstAirshows.SelectedIndexChanged += lstAirshows_SelectedIndexChanged;
            // 
            // lstContacts
            // 
            lstContacts.FormattingEnabled = true;
            lstContacts.ItemHeight = 15;
            lstContacts.Location = new Point(576, 210);
            lstContacts.Name = "lstContacts";
            lstContacts.Size = new Size(191, 469);
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
            // formSearch
            // 
            ClientSize = new Size(800, 711);
            Controls.Add(btnFilterSetAll);
            Controls.Add(btnFilterClearAll);
            Controls.Add(lstContacts);
            Controls.Add(lstAirshows);
            Controls.Add(lblShowName);
            Controls.Add(txtShowName);
            Controls.Add(lblShowLocation);
            Controls.Add(txtShowLocation);
            Controls.Add(lblShowRegion);
            Controls.Add(clbShowRegion);
            Controls.Add(lblContactName);
            Controls.Add(txtContactName);
            Controls.Add(btnSearch);
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
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
        }

        private void txtContactName_TextChanged(object sender, EventArgs e)
        {
            performSerch();
        }

        private void performSerch(List<object> updatedCheckedItems = null)
        {
            //we dont't want to modify our masterlist.
            List<Airshow> searchAirshows = Airshow.DeepCopy(airshowGroup.Airshows.myShows);
            //pair down searchAirshow by region

            searchAirshows = filterByRegion(searchAirshows, updatedCheckedItems);
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
    }
}