namespace AirshowSchedules
{
  partial class frmAirshowScheduleTool
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAirshowScheduleTool));
            btnFileSetup = new Button();
            txtOutput = new TextBox();
            btnCopyToTabs = new Button();
            btnSaveAs = new Button();
            btnReadXML = new Button();
            btnCompareXMLs = new Button();
            txtRight = new TextBox();
            btnNewShows = new Button();
            btnCopyMiddle = new Button();
            toolTip1 = new ToolTip(components);
            dgvCalendar = new DataGridView();
            lstBoxShows = new ListBox();
            dataGridViewShows = new DataGridView();
            lblYearOfInterest = new Label();
            btnAddShow = new Button();
            chklstbox_diff = new CheckedListBox();
            btnCompareNewToDB = new Button();
            btnARCVDB = new Button();
            button1 = new Button();
            button2 = new Button();
            btnSetRegionFile = new Button();
            chklstRegions = new CheckedListBox();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            menuStrip1 = new MenuStrip();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            generateCallListToolStripMenuItem = new ToolStripMenuItem();
            generateBookedListToolStripMenuItem = new ToolStripMenuItem();
            generateICASMailingListToolStripMenuItem = new ToolStripMenuItem();
            generateICASMailingListAllInRegionToolStripMenuItem = new ToolStripMenuItem();
            searchToolStripMenuItem = new ToolStripMenuItem();
            contactToolStripMenuItem = new ToolStripMenuItem();
            btnDeleteShow = new Button();
            btnCheckForDuplicates = new Button();
            btnCheckForDuplicatesInDB = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewShows).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnFileSetup
            // 
            btnFileSetup.Location = new Point(25, 372);
            btnFileSetup.Margin = new Padding(5);
            btnFileSetup.Name = "btnFileSetup";
            btnFileSetup.Size = new Size(142, 38);
            btnFileSetup.TabIndex = 0;
            btnFileSetup.Text = "Parse Data File";
            btnFileSetup.UseVisualStyleBackColor = true;
            btnFileSetup.Click += btnParseAirshowDataFile_Click;
            // 
            // txtOutput
            // 
            txtOutput.Font = new Font("Courier Std", 9F, FontStyle.Bold, GraphicsUnit.Point);
            txtOutput.Location = new Point(25, 37);
            txtOutput.Margin = new Padding(5);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ScrollBars = ScrollBars.Both;
            txtOutput.Size = new Size(662, 327);
            txtOutput.TabIndex = 1;
            // 
            // btnCopyToTabs
            // 
            btnCopyToTabs.Location = new Point(487, 372);
            btnCopyToTabs.Margin = new Padding(5);
            btnCopyToTabs.Name = "btnCopyToTabs";
            btnCopyToTabs.Size = new Size(197, 38);
            btnCopyToTabs.TabIndex = 2;
            btnCopyToTabs.Text = "Copy to Tab Delim";
            btnCopyToTabs.UseVisualStyleBackColor = true;
            btnCopyToTabs.Click += btnCopyToTabs_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(375, 372);
            btnSaveAs.Margin = new Padding(5);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(107, 38);
            btnSaveAs.TabIndex = 3;
            btnSaveAs.Text = "Save As";
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // btnReadXML
            // 
            btnReadXML.Location = new Point(25, 418);
            btnReadXML.Margin = new Padding(5);
            btnReadXML.Name = "btnReadXML";
            btnReadXML.Size = new Size(298, 38);
            btnReadXML.TabIndex = 4;
            btnReadXML.Text = "Set actv DB File";
            btnReadXML.UseVisualStyleBackColor = true;
            btnReadXML.Click += btnReadXML_Click;
            // 
            // btnCompareXMLs
            // 
            btnCompareXMLs.Location = new Point(697, 375);
            btnCompareXMLs.Margin = new Padding(5);
            btnCompareXMLs.Name = "btnCompareXMLs";
            btnCompareXMLs.Size = new Size(187, 38);
            btnCompareXMLs.TabIndex = 5;
            btnCompareXMLs.Text = "Compare XML Files";
            toolTip1.SetToolTip(btnCompareXMLs, "Loops through the right DB and sees if it exists in the left DB.  If not, it returns it to the box above.");
            btnCompareXMLs.UseVisualStyleBackColor = true;
            btnCompareXMLs.Click += btnCompareXMLs_Click;
            // 
            // txtRight
            // 
            txtRight.Font = new Font("Courier Std", 9F, FontStyle.Bold, GraphicsUnit.Point);
            txtRight.Location = new Point(1372, 40);
            txtRight.Margin = new Padding(5);
            txtRight.Multiline = true;
            txtRight.Name = "txtRight";
            txtRight.ScrollBars = ScrollBars.Both;
            txtRight.Size = new Size(662, 322);
            txtRight.TabIndex = 7;
            // 
            // btnNewShows
            // 
            btnNewShows.Location = new Point(1198, 375);
            btnNewShows.Margin = new Padding(5);
            btnNewShows.Name = "btnNewShows";
            btnNewShows.Size = new Size(153, 38);
            btnNewShows.TabIndex = 8;
            btnNewShows.Text = "Compare Years";
            btnNewShows.UseVisualStyleBackColor = true;
            btnNewShows.Click += btnNewShows_Click;
            // 
            // btnCopyMiddle
            // 
            btnCopyMiddle.Location = new Point(697, 418);
            btnCopyMiddle.Margin = new Padding(5);
            btnCopyMiddle.Name = "btnCopyMiddle";
            btnCopyMiddle.Size = new Size(365, 38);
            btnCopyMiddle.TabIndex = 9;
            btnCopyMiddle.Text = "Copy to Tab Delim";
            btnCopyMiddle.UseVisualStyleBackColor = true;
            btnCopyMiddle.Click += btnCopyMiddle_Click;
            // 
            // dgvCalendar
            // 
            dgvCalendar.AllowUserToAddRows = false;
            dgvCalendar.AllowUserToDeleteRows = false;
            dgvCalendar.AllowUserToResizeColumns = false;
            dgvCalendar.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCalendar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCalendar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCalendar.Location = new Point(22, 495);
            dgvCalendar.Margin = new Padding(3, 2, 3, 2);
            dgvCalendar.MultiSelect = false;
            dgvCalendar.Name = "dgvCalendar";
            dgvCalendar.RowHeadersWidth = 82;
            dgvCalendar.RowTemplate.Height = 41;
            dgvCalendar.Size = new Size(1182, 690);
            dgvCalendar.TabIndex = 11;
            dgvCalendar.CellClick += dataGridView2_CellClick;
            dgvCalendar.CellContentClick += dataGridView2_CellContentClick;
            dgvCalendar.ColumnAdded += dataGridView2_ColumnAdded;
            // 
            // lstBoxShows
            // 
            lstBoxShows.FormattingEnabled = true;
            lstBoxShows.ItemHeight = 25;
            lstBoxShows.Location = new Point(2347, 770);
            lstBoxShows.Margin = new Padding(3, 2, 3, 2);
            lstBoxShows.Name = "lstBoxShows";
            lstBoxShows.Size = new Size(609, 579);
            lstBoxShows.TabIndex = 12;
            lstBoxShows.SelectedIndexChanged += lstBoxShows_SelectedIndexChanged;
            // 
            // dataGridViewShows
            // 
            dataGridViewShows.AllowUserToAddRows = false;
            dataGridViewShows.AllowUserToDeleteRows = false;
            dataGridViewShows.AllowUserToResizeColumns = false;
            dataGridViewShows.AllowUserToResizeRows = false;
            dataGridViewShows.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewShows.Location = new Point(1209, 494);
            dataGridViewShows.Margin = new Padding(3, 2, 3, 2);
            dataGridViewShows.MultiSelect = false;
            dataGridViewShows.Name = "dataGridViewShows";
            dataGridViewShows.RowHeadersWidth = 82;
            dataGridViewShows.RowTemplate.Height = 41;
            dataGridViewShows.Size = new Size(823, 387);
            dataGridViewShows.TabIndex = 13;
            dataGridViewShows.CellClick += dataGridViewShows_CellClick;
            dataGridViewShows.CellContentClick += dataGridViewShows_CellContentClick;
            // 
            // lblYearOfInterest
            // 
            lblYearOfInterest.AutoSize = true;
            lblYearOfInterest.Location = new Point(22, 462);
            lblYearOfInterest.Name = "lblYearOfInterest";
            lblYearOfInterest.Size = new Size(59, 25);
            lblYearOfInterest.TabIndex = 14;
            lblYearOfInterest.Text = "label1";
            // 
            // btnAddShow
            // 
            btnAddShow.Location = new Point(1212, 893);
            btnAddShow.Margin = new Padding(3, 2, 3, 2);
            btnAddShow.Name = "btnAddShow";
            btnAddShow.Size = new Size(177, 37);
            btnAddShow.TabIndex = 15;
            btnAddShow.Text = "Add A Show";
            btnAddShow.UseVisualStyleBackColor = true;
            btnAddShow.Click += btnAddShow_Click;
            // 
            // chklstbox_diff
            // 
            chklstbox_diff.FormattingEnabled = true;
            chklstbox_diff.Location = new Point(702, 40);
            chklstbox_diff.Margin = new Padding(3, 2, 3, 2);
            chklstbox_diff.Name = "chklstbox_diff";
            chklstbox_diff.Size = new Size(652, 312);
            chklstbox_diff.TabIndex = 16;
            // 
            // btnCompareNewToDB
            // 
            btnCompareNewToDB.Location = new Point(895, 375);
            btnCompareNewToDB.Margin = new Padding(5);
            btnCompareNewToDB.Name = "btnCompareNewToDB";
            btnCompareNewToDB.Size = new Size(170, 38);
            btnCompareNewToDB.TabIndex = 17;
            btnCompareNewToDB.Text = "new to actv DB";
            btnCompareNewToDB.UseVisualStyleBackColor = true;
            btnCompareNewToDB.Click += btnCompareNewToDB_Click;
            // 
            // btnARCVDB
            // 
            btnARCVDB.Location = new Point(375, 418);
            btnARCVDB.Margin = new Padding(5);
            btnARCVDB.Name = "btnARCVDB";
            btnARCVDB.Size = new Size(310, 38);
            btnARCVDB.TabIndex = 18;
            btnARCVDB.Text = "Archive actv DB File";
            btnARCVDB.UseVisualStyleBackColor = true;
            btnARCVDB.Click += btnARCVDB_Click;
            // 
            // button1
            // 
            button1.Location = new Point(1068, 418);
            button1.Margin = new Padding(5);
            button1.Name = "button1";
            button1.Size = new Size(285, 38);
            button1.TabIndex = 19;
            button1.Text = "Add Selected to actv DB";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(2155, 103);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(268, 37);
            button2.TabIndex = 20;
            button2.Text = "Builds the Region File";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSetRegionFile
            // 
            btnSetRegionFile.Location = new Point(175, 372);
            btnSetRegionFile.Margin = new Padding(5);
            btnSetRegionFile.Name = "btnSetRegionFile";
            btnSetRegionFile.Size = new Size(142, 38);
            btnSetRegionFile.TabIndex = 21;
            btnSetRegionFile.Text = "Set Region File";
            btnSetRegionFile.UseVisualStyleBackColor = true;
            btnSetRegionFile.Click += btnSetRegionFile_Click;
            // 
            // chklstRegions
            // 
            chklstRegions.FormattingEnabled = true;
            chklstRegions.Location = new Point(1733, 893);
            chklstRegions.Margin = new Padding(3, 2, 3, 2);
            chklstRegions.Name = "chklstRegions";
            chklstRegions.Size = new Size(301, 256);
            chklstRegions.TabIndex = 22;
            chklstRegions.SelectedIndexChanged += chklstRegions_SelectedIndexChanged;
            // 
            // button3
            // 
            button3.Location = new Point(1797, 1153);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(115, 37);
            button3.TabIndex = 23;
            button3.Text = "Set All";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(1917, 1153);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(115, 37);
            button4.TabIndex = 24;
            button4.Text = "Clear All";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(1677, 1153);
            button5.Margin = new Padding(3, 2, 3, 2);
            button5.Name = "button5";
            button5.Size = new Size(115, 37);
            button5.TabIndex = 25;
            button5.Text = "Filter List";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { reportsToolStripMenuItem, searchToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(2052, 33);
            menuStrip1.TabIndex = 26;
            menuStrip1.Text = "menuStrip1";
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { generateCallListToolStripMenuItem, generateBookedListToolStripMenuItem, generateICASMailingListToolStripMenuItem, generateICASMailingListAllInRegionToolStripMenuItem });
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(89, 29);
            reportsToolStripMenuItem.Text = "Reports";
            // 
            // generateCallListToolStripMenuItem
            // 
            generateCallListToolStripMenuItem.Name = "generateCallListToolStripMenuItem";
            generateCallListToolStripMenuItem.Size = new Size(321, 34);
            generateCallListToolStripMenuItem.Text = "Generate Call List";
            generateCallListToolStripMenuItem.Click += generateCallListToolStripMenuItem_Click;
            // 
            // generateBookedListToolStripMenuItem
            // 
            generateBookedListToolStripMenuItem.Name = "generateBookedListToolStripMenuItem";
            generateBookedListToolStripMenuItem.Size = new Size(321, 34);
            generateBookedListToolStripMenuItem.Text = "Generate Booked List";
            generateBookedListToolStripMenuItem.Click += generateBookedListToolStripMenuItem_Click;
            // 
            // generateICASMailingListToolStripMenuItem
            // 
            generateICASMailingListToolStripMenuItem.Name = "generateICASMailingListToolStripMenuItem";
            generateICASMailingListToolStripMenuItem.Size = new Size(321, 34);
            generateICASMailingListToolStripMenuItem.Text = "Generate ICAS Mailing List";
            generateICASMailingListToolStripMenuItem.Click += generateICASMailingListToolStripMenuItem_Click;
            // 
            // generateICASMailingListAllInRegionToolStripMenuItem
            // 
            generateICASMailingListAllInRegionToolStripMenuItem.Name = "generateICASMailingListAllInRegionToolStripMenuItem";
            generateICASMailingListAllInRegionToolStripMenuItem.Size = new Size(321, 34);
            generateICASMailingListAllInRegionToolStripMenuItem.Text = "All in Filtered Region";
            generateICASMailingListAllInRegionToolStripMenuItem.Click += generateICASMailingListAllInRegionToolStripMenuItem_Click;
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { contactToolStripMenuItem });
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new Size(80, 29);
            searchToolStripMenuItem.Text = "Search";
            // 
            // contactToolStripMenuItem
            // 
            contactToolStripMenuItem.Name = "contactToolStripMenuItem";
            contactToolStripMenuItem.Size = new Size(223, 34);
            contactToolStripMenuItem.Text = "Search Dialog";
            contactToolStripMenuItem.Click += contactToolStripMenuItem_Click;
            // 
            // btnDeleteShow
            // 
            btnDeleteShow.Location = new Point(1212, 933);
            btnDeleteShow.Margin = new Padding(3, 2, 3, 2);
            btnDeleteShow.Name = "btnDeleteShow";
            btnDeleteShow.Size = new Size(177, 37);
            btnDeleteShow.TabIndex = 27;
            btnDeleteShow.Text = "Delete A Show";
            btnDeleteShow.UseVisualStyleBackColor = true;
            btnDeleteShow.Click += btnDeleteShow_Click;
            // 
            // btnCheckForDuplicates
            // 
            btnCheckForDuplicates.Location = new Point(1361, 372);
            btnCheckForDuplicates.Margin = new Padding(5);
            btnCheckForDuplicates.Name = "btnCheckForDuplicates";
            btnCheckForDuplicates.Size = new Size(348, 38);
            btnCheckForDuplicates.TabIndex = 28;
            btnCheckForDuplicates.Text = "Check Selected For Dups in actv DB";
            btnCheckForDuplicates.UseVisualStyleBackColor = true;
            btnCheckForDuplicates.Click += btnCheckForDuplicates_Click;
            // 
            // btnCheckForDuplicatesInDB
            // 
            btnCheckForDuplicatesInDB.Location = new Point(1361, 418);
            btnCheckForDuplicatesInDB.Name = "btnCheckForDuplicatesInDB";
            btnCheckForDuplicatesInDB.Size = new Size(348, 34);
            btnCheckForDuplicatesInDB.TabIndex = 29;
            btnCheckForDuplicatesInDB.Text = "Clean Up DB";
            btnCheckForDuplicatesInDB.UseVisualStyleBackColor = true;
            btnCheckForDuplicatesInDB.Click += btnCheckForDuplicatesInDB_Click;
            // 
            // frmAirshowScheduleTool
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2052, 1273);
            Controls.Add(btnCheckForDuplicatesInDB);
            Controls.Add(btnCheckForDuplicates);
            Controls.Add(btnDeleteShow);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(chklstRegions);
            Controls.Add(btnSetRegionFile);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(btnARCVDB);
            Controls.Add(btnCompareNewToDB);
            Controls.Add(chklstbox_diff);
            Controls.Add(btnAddShow);
            Controls.Add(lblYearOfInterest);
            Controls.Add(dataGridViewShows);
            Controls.Add(lstBoxShows);
            Controls.Add(dgvCalendar);
            Controls.Add(btnCopyMiddle);
            Controls.Add(btnNewShows);
            Controls.Add(txtRight);
            Controls.Add(btnCompareXMLs);
            Controls.Add(btnReadXML);
            Controls.Add(btnSaveAs);
            Controls.Add(btnCopyToTabs);
            Controls.Add(txtOutput);
            Controls.Add(btnFileSetup);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(5);
            Name = "frmAirshowScheduleTool";
            Text = "Undaunted Airshows Schedule Maker";
            Load += frmAirshowScheduleTool_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewShows).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnFileSetup;
        private TextBox txtOutput;
        private Button btnCopyToTabs;
        private Button btnSaveAs;
        private Button btnReadXML;
        private Button btnCompareXMLs;
        private TextBox txtRight;
        private Button btnNewShows;
        private Button btnCopyMiddle;
        private ToolTip toolTip1;
        private DataGridView dgvCalendar;
        private ListBox lstBoxShows;
        private DataGridView dataGridViewShows;
        private Label lblYearOfInterest;
        private Button btnAddShow;
        private CheckedListBox chklstbox_diff;
        private Button btnCompareNewToDB;
        private Button btnARCVDB;
        private Button button1;
        private Button button2;
        private Button btnSetRegionFile;
        private CheckedListBox chklstRegions;
        private Button button3;
        private Button button4;
        private Button button5;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem generateCallListToolStripMenuItem;
        private ToolStripMenuItem generateBookedListToolStripMenuItem;
        private ToolStripMenuItem generateICASMailingListToolStripMenuItem;
        private ToolStripMenuItem generateICASMailingListAllInRegionToolStripMenuItem;
        private Button btnDeleteShow;
        private Button btnCheckForDuplicates;
    private ToolStripMenuItem searchToolStripMenuItem;
    private ToolStripMenuItem contactToolStripMenuItem;
        private Button btnCheckForDuplicatesInDB;
    }
}