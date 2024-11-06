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
            toolTip1 = new ToolTip(components);
            dgvCalendar = new DataGridView();
            lstBoxShows = new ListBox();
            dataGridViewShows = new DataGridView();
            lblYearOfInterest = new Label();
            btnAddShow = new Button();
            chklstRegions = new CheckedListBox();
            btnFilterSetAll = new Button();
            btnFilterClearAll = new Button();
            btnFilterShows = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            pareDataFileToolStripMenuItem = new ToolStripMenuItem();
            fileSaveParsedDataFile = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            setActiveDatabaseFileToolStripMenuItem = new ToolStripMenuItem();
            saveDatabaseFileToolStripMenuItem = new ToolStripMenuItem();
            arciveActiveDBToolStripMenuItem = new ToolStripMenuItem();
            dataBaseManagementToolStripMenuItem = new ToolStripMenuItem();
            compareToActiveDBToolStripMenuItem = new ToolStripMenuItem();
            cleanUpDBToolStripMenuItem = new ToolStripMenuItem();
            checkForCancelledShowsToolStripMenuItem = new ToolStripMenuItem();
            updateAdditionalFieldsToolStripMenuItem = new ToolStripMenuItem();
            setYearOfInterestToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            generateCallListToolStripMenuItem = new ToolStripMenuItem();
            generateBookedListToolStripMenuItem = new ToolStripMenuItem();
            generateICASMailingListToolStripMenuItem = new ToolStripMenuItem();
            generateICASMailingListAllInRegionToolStripMenuItem = new ToolStripMenuItem();
            searchToolStripMenuItem = new ToolStripMenuItem();
            contactToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            showHelpFileToolStripMenuItem = new ToolStripMenuItem();
            btnDeleteShow = new Button();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewShows).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
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
            dgvCalendar.Location = new Point(8, 51);
            dgvCalendar.Margin = new Padding(2, 1, 2, 1);
            dgvCalendar.MultiSelect = false;
            dgvCalendar.Name = "dgvCalendar";
            dgvCalendar.RowHeadersWidth = 82;
            dgvCalendar.RowTemplate.Height = 41;
            dgvCalendar.Size = new Size(827, 414);
            dgvCalendar.TabIndex = 11;
            dgvCalendar.CellClick += dataGridView2_CellClick;
            dgvCalendar.CellContentClick += dataGridView2_CellContentClick;
            dgvCalendar.ColumnAdded += dataGridView2_ColumnAdded;
            // 
            // lstBoxShows
            // 
            lstBoxShows.FormattingEnabled = true;
            lstBoxShows.ItemHeight = 15;
            lstBoxShows.Location = new Point(1464, 322);
            lstBoxShows.Margin = new Padding(2, 1, 2, 1);
            lstBoxShows.Name = "lstBoxShows";
            lstBoxShows.Size = new Size(428, 349);
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
            dataGridViewShows.Location = new Point(839, 50);
            dataGridViewShows.Margin = new Padding(2, 1, 2, 1);
            dataGridViewShows.MultiSelect = false;
            dataGridViewShows.Name = "dataGridViewShows";
            dataGridViewShows.RowHeadersWidth = 82;
            dataGridViewShows.RowTemplate.Height = 41;
            dataGridViewShows.Size = new Size(576, 232);
            dataGridViewShows.TabIndex = 13;
            dataGridViewShows.CellClick += dataGridViewShows_CellClick;
            dataGridViewShows.CellContentClick += dataGridViewShows_CellContentClick;
            // 
            // lblYearOfInterest
            // 
            lblYearOfInterest.AutoSize = true;
            lblYearOfInterest.Location = new Point(8, 35);
            lblYearOfInterest.Margin = new Padding(2, 0, 2, 0);
            lblYearOfInterest.Name = "lblYearOfInterest";
            lblYearOfInterest.Size = new Size(38, 15);
            lblYearOfInterest.TabIndex = 14;
            lblYearOfInterest.Text = "label1";
            // 
            // btnAddShow
            // 
            btnAddShow.Location = new Point(841, 290);
            btnAddShow.Margin = new Padding(2, 1, 2, 1);
            btnAddShow.Name = "btnAddShow";
            btnAddShow.Size = new Size(124, 22);
            btnAddShow.TabIndex = 15;
            btnAddShow.Text = "Add A Show";
            btnAddShow.UseVisualStyleBackColor = true;
            btnAddShow.Click += btnAddShow_Click;
            // 
            // chklstRegions
            // 
            chklstRegions.FormattingEnabled = true;
            chklstRegions.Location = new Point(1206, 290);
            chklstRegions.Margin = new Padding(2, 1, 2, 1);
            chklstRegions.Name = "chklstRegions";
            chklstRegions.Size = new Size(212, 148);
            chklstRegions.TabIndex = 22;
            chklstRegions.SelectedIndexChanged += chklstRegions_SelectedIndexChanged;
            // 
            // btnFilterSetAll
            // 
            btnFilterSetAll.Location = new Point(1251, 446);
            btnFilterSetAll.Margin = new Padding(2, 1, 2, 1);
            btnFilterSetAll.Name = "btnFilterSetAll";
            btnFilterSetAll.Size = new Size(80, 22);
            btnFilterSetAll.TabIndex = 23;
            btnFilterSetAll.Text = "Set All";
            btnFilterSetAll.UseVisualStyleBackColor = true;
            btnFilterSetAll.Click += btnFilterSetAll_Click;
            // 
            // btnFilterClearAll
            // 
            btnFilterClearAll.Location = new Point(1335, 446);
            btnFilterClearAll.Margin = new Padding(2, 1, 2, 1);
            btnFilterClearAll.Name = "btnFilterClearAll";
            btnFilterClearAll.Size = new Size(80, 22);
            btnFilterClearAll.TabIndex = 24;
            btnFilterClearAll.Text = "Clear All";
            btnFilterClearAll.UseVisualStyleBackColor = true;
            btnFilterClearAll.Click += btnFilterClearAll_Click;
            // 
            // btnFilterShows
            // 
            btnFilterShows.Location = new Point(1167, 446);
            btnFilterShows.Margin = new Padding(2, 1, 2, 1);
            btnFilterShows.Name = "btnFilterShows";
            btnFilterShows.Size = new Size(80, 22);
            btnFilterShows.TabIndex = 25;
            btnFilterShows.Text = "Filter List";
            btnFilterShows.UseVisualStyleBackColor = true;
            btnFilterShows.Click += btnFilterShows_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, dataBaseManagementToolStripMenuItem, reportsToolStripMenuItem, searchToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(4, 1, 0, 1);
            menuStrip1.Size = new Size(1424, 24);
            menuStrip1.TabIndex = 26;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pareDataFileToolStripMenuItem, fileSaveParsedDataFile, toolStripSeparator1, setActiveDatabaseFileToolStripMenuItem, saveDatabaseFileToolStripMenuItem, arciveActiveDBToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 22);
            fileToolStripMenuItem.Text = "File";
            // 
            // pareDataFileToolStripMenuItem
            // 
            pareDataFileToolStripMenuItem.Name = "pareDataFileToolStripMenuItem";
            pareDataFileToolStripMenuItem.Size = new Size(198, 22);
            pareDataFileToolStripMenuItem.Text = "Parse Data File";
            pareDataFileToolStripMenuItem.Click += fileParseDataFileToolStripMenuItem_Click;
            // 
            // fileSaveParsedDataFile
            // 
            fileSaveParsedDataFile.Name = "fileSaveParsedDataFile";
            fileSaveParsedDataFile.Size = new Size(198, 22);
            fileSaveParsedDataFile.Text = "Save Parsed Data File";
            fileSaveParsedDataFile.Click += fileSaveParsedDataFile_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(195, 6);
            // 
            // setActiveDatabaseFileToolStripMenuItem
            // 
            setActiveDatabaseFileToolStripMenuItem.Name = "setActiveDatabaseFileToolStripMenuItem";
            setActiveDatabaseFileToolStripMenuItem.Size = new Size(198, 22);
            setActiveDatabaseFileToolStripMenuItem.Text = "Set Active Database File";
            setActiveDatabaseFileToolStripMenuItem.Click += setActiveDatabaseFileToolStripMenuItem_Click;
            // 
            // saveDatabaseFileToolStripMenuItem
            // 
            saveDatabaseFileToolStripMenuItem.Name = "saveDatabaseFileToolStripMenuItem";
            saveDatabaseFileToolStripMenuItem.Size = new Size(198, 22);
            saveDatabaseFileToolStripMenuItem.Text = "Save Database File As...";
            saveDatabaseFileToolStripMenuItem.Click += saveDatabaseFileToolStripMenuItem_Click;
            // 
            // arciveActiveDBToolStripMenuItem
            // 
            arciveActiveDBToolStripMenuItem.Name = "arciveActiveDBToolStripMenuItem";
            arciveActiveDBToolStripMenuItem.Size = new Size(198, 22);
            arciveActiveDBToolStripMenuItem.Text = "Arcive Active DB";
            arciveActiveDBToolStripMenuItem.Click += arciveActiveDBToolStripMenuItem_Click;
            // 
            // dataBaseManagementToolStripMenuItem
            // 
            dataBaseManagementToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { compareToActiveDBToolStripMenuItem, cleanUpDBToolStripMenuItem, checkForCancelledShowsToolStripMenuItem, updateAdditionalFieldsToolStripMenuItem, setYearOfInterestToolStripMenuItem });
            dataBaseManagementToolStripMenuItem.Name = "dataBaseManagementToolStripMenuItem";
            dataBaseManagementToolStripMenuItem.Size = new Size(144, 22);
            dataBaseManagementToolStripMenuItem.Text = "Data Base Management";
            // 
            // compareToActiveDBToolStripMenuItem
            // 
            compareToActiveDBToolStripMenuItem.Name = "compareToActiveDBToolStripMenuItem";
            compareToActiveDBToolStripMenuItem.Size = new Size(219, 22);
            compareToActiveDBToolStripMenuItem.Text = "Compare To Active DB";
            compareToActiveDBToolStripMenuItem.Click += compareToActiveDBToolStripMenuItem_Click;
            // 
            // cleanUpDBToolStripMenuItem
            // 
            cleanUpDBToolStripMenuItem.Name = "cleanUpDBToolStripMenuItem";
            cleanUpDBToolStripMenuItem.Size = new Size(219, 22);
            cleanUpDBToolStripMenuItem.Text = "Clean Up DB";
            cleanUpDBToolStripMenuItem.Click += cleanUpDBToolStripMenuItem_Click;
            // 
            // checkForCancelledShowsToolStripMenuItem
            // 
            checkForCancelledShowsToolStripMenuItem.Name = "checkForCancelledShowsToolStripMenuItem";
            checkForCancelledShowsToolStripMenuItem.Size = new Size(219, 22);
            checkForCancelledShowsToolStripMenuItem.Text = "Check For Cancelled Shows";
            checkForCancelledShowsToolStripMenuItem.Click += checkForCancelledShowsToolStripMenuItem_Click;
            // 
            // updateAdditionalFieldsToolStripMenuItem
            // 
            updateAdditionalFieldsToolStripMenuItem.Name = "updateAdditionalFieldsToolStripMenuItem";
            updateAdditionalFieldsToolStripMenuItem.Size = new Size(219, 22);
            updateAdditionalFieldsToolStripMenuItem.Text = "Update Additional Fields";
            updateAdditionalFieldsToolStripMenuItem.Click += updateAdditionalFieldsToolStripMenuItem_Click;
            // 
            // setYearOfInterestToolStripMenuItem
            // 
            setYearOfInterestToolStripMenuItem.Name = "setYearOfInterestToolStripMenuItem";
            setYearOfInterestToolStripMenuItem.Size = new Size(219, 22);
            setYearOfInterestToolStripMenuItem.Text = "Set Year of Interest";
            setYearOfInterestToolStripMenuItem.Click += setYearOfInterestToolStripMenuItem_Click;
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { generateCallListToolStripMenuItem, generateBookedListToolStripMenuItem, generateICASMailingListToolStripMenuItem, generateICASMailingListAllInRegionToolStripMenuItem });
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(59, 22);
            reportsToolStripMenuItem.Text = "Reports";
            // 
            // generateCallListToolStripMenuItem
            // 
            generateCallListToolStripMenuItem.Name = "generateCallListToolStripMenuItem";
            generateCallListToolStripMenuItem.Size = new Size(213, 22);
            generateCallListToolStripMenuItem.Text = "Generate Call List";
            generateCallListToolStripMenuItem.Click += generateCallListToolStripMenuItem_Click;
            // 
            // generateBookedListToolStripMenuItem
            // 
            generateBookedListToolStripMenuItem.Name = "generateBookedListToolStripMenuItem";
            generateBookedListToolStripMenuItem.Size = new Size(213, 22);
            generateBookedListToolStripMenuItem.Text = "Generate Booked List";
            generateBookedListToolStripMenuItem.Click += generateBookedListToolStripMenuItem_Click;
            // 
            // generateICASMailingListToolStripMenuItem
            // 
            generateICASMailingListToolStripMenuItem.Name = "generateICASMailingListToolStripMenuItem";
            generateICASMailingListToolStripMenuItem.Size = new Size(213, 22);
            generateICASMailingListToolStripMenuItem.Text = "Generate ICAS Mailing List";
            generateICASMailingListToolStripMenuItem.Click += generateICASMailingListToolStripMenuItem_Click;
            // 
            // generateICASMailingListAllInRegionToolStripMenuItem
            // 
            generateICASMailingListAllInRegionToolStripMenuItem.Name = "generateICASMailingListAllInRegionToolStripMenuItem";
            generateICASMailingListAllInRegionToolStripMenuItem.Size = new Size(213, 22);
            generateICASMailingListAllInRegionToolStripMenuItem.Text = "All in Filtered Region";
            generateICASMailingListAllInRegionToolStripMenuItem.Click += generateICASMailingListAllInRegionToolStripMenuItem_Click;
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { contactToolStripMenuItem });
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new Size(54, 22);
            searchToolStripMenuItem.Text = "Search";
            // 
            // contactToolStripMenuItem
            // 
            contactToolStripMenuItem.Name = "contactToolStripMenuItem";
            contactToolStripMenuItem.Size = new Size(146, 22);
            contactToolStripMenuItem.Text = "Search Dialog";
            contactToolStripMenuItem.Click += contactToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showHelpFileToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 22);
            helpToolStripMenuItem.Text = "Help";
            // 
            // showHelpFileToolStripMenuItem
            // 
            showHelpFileToolStripMenuItem.Name = "showHelpFileToolStripMenuItem";
            showHelpFileToolStripMenuItem.Size = new Size(152, 22);
            showHelpFileToolStripMenuItem.Text = "Show Help File";
            showHelpFileToolStripMenuItem.Click += showHelpFileToolStripMenuItem_Click;
            // 
            // btnDeleteShow
            // 
            btnDeleteShow.Location = new Point(841, 314);
            btnDeleteShow.Margin = new Padding(2, 1, 2, 1);
            btnDeleteShow.Name = "btnDeleteShow";
            btnDeleteShow.Size = new Size(124, 22);
            btnDeleteShow.TabIndex = 27;
            btnDeleteShow.Text = "Delete A Show";
            btnDeleteShow.UseVisualStyleBackColor = true;
            btnDeleteShow.Click += btnDeleteShow_Click;
            // 
            // button1
            // 
            button1.Location = new Point(871, 351);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 28;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // frmAirshowScheduleTool
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1424, 474);
            Controls.Add(button1);
            Controls.Add(btnDeleteShow);
            Controls.Add(btnFilterShows);
            Controls.Add(btnFilterClearAll);
            Controls.Add(btnFilterSetAll);
            Controls.Add(chklstRegions);
            Controls.Add(btnAddShow);
            Controls.Add(lblYearOfInterest);
            Controls.Add(dataGridViewShows);
            Controls.Add(lstBoxShows);
            Controls.Add(dgvCalendar);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
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
        private ToolTip toolTip1;
        private DataGridView dgvCalendar;
        private ListBox lstBoxShows;
        private DataGridView dataGridViewShows;
        private Label lblYearOfInterest;
        private Button btnAddShow;
        private CheckedListBox chklstRegions;
        private Button btnFilterSetAll;
        private Button btnFilterClearAll;
        private Button btnFilterShows;
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
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem pareDataFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem fileSaveParsedDataFile;
        private ToolStripMenuItem setActiveDatabaseFileToolStripMenuItem;
        private ToolStripMenuItem saveDatabaseFileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem showHelpFileToolStripMenuItem;
        private ToolStripMenuItem dataBaseManagementToolStripMenuItem;
        private ToolStripMenuItem compareToActiveDBToolStripMenuItem;
        private ToolStripMenuItem cleanUpDBToolStripMenuItem;
        private ToolStripMenuItem checkForCancelledShowsToolStripMenuItem;
        private ToolStripMenuItem arciveActiveDBToolStripMenuItem;
        private ToolStripMenuItem updateAdditionalFieldsToolStripMenuItem;
        private ToolStripMenuItem setYearOfInterestToolStripMenuItem;
        private Button button1;
    }
}