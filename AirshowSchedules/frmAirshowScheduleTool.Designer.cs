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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAirshowScheduleTool));
            toolTip1 = new ToolTip(components);
            dgvCalendar = new DataGridView();
            lstBoxShows = new ListBox();
            dataGridViewShows = new DataGridView();
            lblYearOfInterest = new Label();
            btnAddShow = new Button();
            chklstRegions = new CheckedListBox();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            toolStripTextBox1 = new ToolStripTextBox();
            pareDataFileToolStripMenuItem = new ToolStripMenuItem();
            fileSaveParsedDataFile = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            setRegionFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            setActiveDatabaseFileToolStripMenuItem = new ToolStripMenuItem();
            saveDatabaseFileToolStripMenuItem = new ToolStripMenuItem();
            dataBaseManagementToolStripMenuItem = new ToolStripMenuItem();
            compareToActiveDBToolStripMenuItem = new ToolStripMenuItem();
            cleanUpDBToolStripMenuItem = new ToolStripMenuItem();
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
            checkForCancelledShowsToolStripMenuItem = new ToolStripMenuItem();
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
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCalendar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvCalendar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCalendar.Location = new Point(12, 85);
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
            lstBoxShows.Location = new Point(2092, 537);
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
            dataGridViewShows.Location = new Point(1199, 84);
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
            lblYearOfInterest.Location = new Point(12, 58);
            lblYearOfInterest.Name = "lblYearOfInterest";
            lblYearOfInterest.Size = new Size(59, 25);
            lblYearOfInterest.TabIndex = 14;
            lblYearOfInterest.Text = "label1";
            // 
            // btnAddShow
            // 
            btnAddShow.Location = new Point(1202, 483);
            btnAddShow.Margin = new Padding(3, 2, 3, 2);
            btnAddShow.Name = "btnAddShow";
            btnAddShow.Size = new Size(177, 37);
            btnAddShow.TabIndex = 15;
            btnAddShow.Text = "Add A Show";
            btnAddShow.UseVisualStyleBackColor = true;
            btnAddShow.Click += btnAddShow_Click;
            // 
            // chklstRegions
            // 
            chklstRegions.FormattingEnabled = true;
            chklstRegions.Location = new Point(1723, 483);
            chklstRegions.Margin = new Padding(3, 2, 3, 2);
            chklstRegions.Name = "chklstRegions";
            chklstRegions.Size = new Size(301, 256);
            chklstRegions.TabIndex = 22;
            chklstRegions.SelectedIndexChanged += chklstRegions_SelectedIndexChanged;
            // 
            // button3
            // 
            button3.Location = new Point(1787, 743);
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
            button4.Location = new Point(1907, 743);
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
            button5.Location = new Point(1667, 743);
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, dataBaseManagementToolStripMenuItem, reportsToolStripMenuItem, searchToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(2515, 33);
            menuStrip1.TabIndex = 26;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripTextBox1, pareDataFileToolStripMenuItem, fileSaveParsedDataFile, toolStripSeparator1, setRegionFileToolStripMenuItem, toolStripSeparator2, setActiveDatabaseFileToolStripMenuItem, saveDatabaseFileToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.Enabled = false;
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.Size = new Size(200, 31);
            toolStripTextBox1.Text = "ICAS Data File";
            // 
            // pareDataFileToolStripMenuItem
            // 
            pareDataFileToolStripMenuItem.Name = "pareDataFileToolStripMenuItem";
            pareDataFileToolStripMenuItem.Size = new Size(302, 34);
            pareDataFileToolStripMenuItem.Text = "Parse Data File";
            pareDataFileToolStripMenuItem.Click += fileParseDataFileToolStripMenuItem_Click;
            // 
            // fileSaveParsedDataFile
            // 
            fileSaveParsedDataFile.Name = "fileSaveParsedDataFile";
            fileSaveParsedDataFile.Size = new Size(302, 34);
            fileSaveParsedDataFile.Text = "Save Parsed Data File";
            fileSaveParsedDataFile.Click += fileSaveParsedDataFile_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(299, 6);
            // 
            // setRegionFileToolStripMenuItem
            // 
            setRegionFileToolStripMenuItem.Name = "setRegionFileToolStripMenuItem";
            setRegionFileToolStripMenuItem.Size = new Size(302, 34);
            setRegionFileToolStripMenuItem.Text = "Set Region File";
            setRegionFileToolStripMenuItem.Click += fileSetRegionFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(299, 6);
            // 
            // setActiveDatabaseFileToolStripMenuItem
            // 
            setActiveDatabaseFileToolStripMenuItem.Name = "setActiveDatabaseFileToolStripMenuItem";
            setActiveDatabaseFileToolStripMenuItem.Size = new Size(302, 34);
            setActiveDatabaseFileToolStripMenuItem.Text = "Set Active Database File";
            setActiveDatabaseFileToolStripMenuItem.Click += setActiveDatabaseFileToolStripMenuItem_Click;
            // 
            // saveDatabaseFileToolStripMenuItem
            // 
            saveDatabaseFileToolStripMenuItem.Name = "saveDatabaseFileToolStripMenuItem";
            saveDatabaseFileToolStripMenuItem.Size = new Size(302, 34);
            saveDatabaseFileToolStripMenuItem.Text = "Save Database File";
            saveDatabaseFileToolStripMenuItem.Click += saveDatabaseFileToolStripMenuItem_Click;
            // 
            // dataBaseManagementToolStripMenuItem
            // 
            dataBaseManagementToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { compareToActiveDBToolStripMenuItem, cleanUpDBToolStripMenuItem, checkForCancelledShowsToolStripMenuItem });
            dataBaseManagementToolStripMenuItem.Name = "dataBaseManagementToolStripMenuItem";
            dataBaseManagementToolStripMenuItem.Size = new Size(216, 29);
            dataBaseManagementToolStripMenuItem.Text = "Data Base Management";
            // 
            // compareToActiveDBToolStripMenuItem
            // 
            compareToActiveDBToolStripMenuItem.Name = "compareToActiveDBToolStripMenuItem";
            compareToActiveDBToolStripMenuItem.Size = new Size(329, 34);
            compareToActiveDBToolStripMenuItem.Text = "Compare To Active DB";
            compareToActiveDBToolStripMenuItem.Click += compareToActiveDBToolStripMenuItem_Click;
            // 
            // cleanUpDBToolStripMenuItem
            // 
            cleanUpDBToolStripMenuItem.Name = "cleanUpDBToolStripMenuItem";
            cleanUpDBToolStripMenuItem.Size = new Size(329, 34);
            cleanUpDBToolStripMenuItem.Text = "Clean Up DB";
            cleanUpDBToolStripMenuItem.Click += cleanUpDBToolStripMenuItem_Click;
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
            contactToolStripMenuItem.Size = new Size(270, 34);
            contactToolStripMenuItem.Text = "Search Dialog";
            contactToolStripMenuItem.Click += contactToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showHelpFileToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(65, 29);
            helpToolStripMenuItem.Text = "Help";
            // 
            // showHelpFileToolStripMenuItem
            // 
            showHelpFileToolStripMenuItem.Name = "showHelpFileToolStripMenuItem";
            showHelpFileToolStripMenuItem.Size = new Size(231, 34);
            showHelpFileToolStripMenuItem.Text = "Show Help File";
            showHelpFileToolStripMenuItem.Click += showHelpFileToolStripMenuItem_Click;
            // 
            // btnDeleteShow
            // 
            btnDeleteShow.Location = new Point(1202, 523);
            btnDeleteShow.Margin = new Padding(3, 2, 3, 2);
            btnDeleteShow.Name = "btnDeleteShow";
            btnDeleteShow.Size = new Size(177, 37);
            btnDeleteShow.TabIndex = 27;
            btnDeleteShow.Text = "Delete A Show";
            btnDeleteShow.UseVisualStyleBackColor = true;
            btnDeleteShow.Click += btnDeleteShow_Click;
            // 
            // checkForCancelledShowsToolStripMenuItem
            // 
            checkForCancelledShowsToolStripMenuItem.Name = "checkForCancelledShowsToolStripMenuItem";
            checkForCancelledShowsToolStripMenuItem.Size = new Size(329, 34);
            checkForCancelledShowsToolStripMenuItem.Text = "Check For Cancelled Shows";
            checkForCancelledShowsToolStripMenuItem.Click += checkForCancelledShowsToolStripMenuItem_Click;
            // 
            // frmAirshowScheduleTool
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2515, 1273);
            Controls.Add(btnDeleteShow);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(chklstRegions);
            Controls.Add(btnAddShow);
            Controls.Add(lblYearOfInterest);
            Controls.Add(dataGridViewShows);
            Controls.Add(lstBoxShows);
            Controls.Add(dgvCalendar);
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
        private ToolTip toolTip1;
        private DataGridView dgvCalendar;
        private ListBox lstBoxShows;
        private DataGridView dataGridViewShows;
        private Label lblYearOfInterest;
        private Button btnAddShow;
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
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem pareDataFileToolStripMenuItem;
        private ToolStripMenuItem setRegionFileToolStripMenuItem;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem fileSaveParsedDataFile;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem setActiveDatabaseFileToolStripMenuItem;
        private ToolStripMenuItem saveDatabaseFileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem showHelpFileToolStripMenuItem;
        private ToolStripMenuItem dataBaseManagementToolStripMenuItem;
        private ToolStripMenuItem compareToActiveDBToolStripMenuItem;
        private ToolStripMenuItem cleanUpDBToolStripMenuItem;
        private ToolStripMenuItem checkForCancelledShowsToolStripMenuItem;
    }
}