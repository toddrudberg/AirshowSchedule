﻿


namespace AirshowSchedules
{
    partial class formMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            toolTip1 = new ToolTip(components);
            dgvCalendar = new DataGridView();
            lstBoxShows = new ListBox();
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
            toolStripSeparator2 = new ToolStripSeparator();
            setActiveContactDBToolStripMenuItem = new ToolStripMenuItem();
            setActiveContactFileToolStripMenuItem = new ToolStripMenuItem();
            dataBaseManagementToolStripMenuItem = new ToolStripMenuItem();
            compareToActiveDBToolStripMenuItem = new ToolStripMenuItem();
            cleanUpDBToolStripMenuItem = new ToolStripMenuItem();
            checkForCancelledShowsToolStripMenuItem = new ToolStripMenuItem();
            updateAdditionalFieldsToolStripMenuItem = new ToolStripMenuItem();
            setYearOfInterestToolStripMenuItem = new ToolStripMenuItem();
            exportContactsToolStripMenuItem = new ToolStripMenuItem();
            searchToolStripMenuItem = new ToolStripMenuItem();
            advancedSearchToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            showHelpFileToolStripMenuItem = new ToolStripMenuItem();
            btnDeleteShow = new Button();
            labelWeekendSelected = new Label();
            linkLabelContacts = new LinkLabel();
            linkLabelDB = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).BeginInit();
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
            lstBoxShows.DrawMode = DrawMode.OwnerDrawFixed;
            lstBoxShows.FormattingEnabled = true;
            lstBoxShows.ItemHeight = 30;
            lstBoxShows.Location = new Point(839, 62);
            lstBoxShows.Name = "lstBoxShows";
            lstBoxShows.Size = new Size(574, 214);
            lstBoxShows.TabIndex = 12;
            lstBoxShows.DrawItem += lstBoxShows_DrawItem;
            lstBoxShows.DoubleClick += lstBoxShows_DoubleClick;
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
            chklstRegions.Location = new Point(1132, 287);
            chklstRegions.Margin = new Padding(2, 1, 2, 1);
            chklstRegions.Name = "chklstRegions";
            chklstRegions.Size = new Size(283, 184);
            chklstRegions.TabIndex = 22;
            chklstRegions.SelectedIndexChanged += chklstRegions_SelectedIndexChanged;
            // 
            // btnFilterSetAll
            // 
            btnFilterSetAll.Location = new Point(950, 449);
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
            btnFilterClearAll.Location = new Point(1034, 449);
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
            btnFilterShows.Location = new Point(866, 449);
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, dataBaseManagementToolStripMenuItem, searchToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(4, 1, 0, 1);
            menuStrip1.Size = new Size(1419, 24);
            menuStrip1.TabIndex = 26;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pareDataFileToolStripMenuItem, fileSaveParsedDataFile, toolStripSeparator1, setActiveDatabaseFileToolStripMenuItem, saveDatabaseFileToolStripMenuItem, arciveActiveDBToolStripMenuItem, toolStripSeparator2, setActiveContactDBToolStripMenuItem, setActiveContactFileToolStripMenuItem });
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
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(195, 6);
            // 
            // setActiveContactDBToolStripMenuItem
            // 
            setActiveContactDBToolStripMenuItem.Name = "setActiveContactDBToolStripMenuItem";
            setActiveContactDBToolStripMenuItem.Size = new Size(198, 22);
            setActiveContactDBToolStripMenuItem.Text = "Set Active Contact DB";
            setActiveContactDBToolStripMenuItem.Click += setActiveContactDBToolStripMenuItem_Click;
            // 
            // setActiveContactFileToolStripMenuItem
            // 
            setActiveContactFileToolStripMenuItem.Name = "setActiveContactFileToolStripMenuItem";
            setActiveContactFileToolStripMenuItem.Size = new Size(198, 22);
            setActiveContactFileToolStripMenuItem.Text = "Save Contact DB As..";
            setActiveContactFileToolStripMenuItem.Click += saveContactFileAs;
            // 
            // dataBaseManagementToolStripMenuItem
            // 
            dataBaseManagementToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { compareToActiveDBToolStripMenuItem, cleanUpDBToolStripMenuItem, checkForCancelledShowsToolStripMenuItem, updateAdditionalFieldsToolStripMenuItem, setYearOfInterestToolStripMenuItem, exportContactsToolStripMenuItem });
            dataBaseManagementToolStripMenuItem.Name = "dataBaseManagementToolStripMenuItem";
            dataBaseManagementToolStripMenuItem.Size = new Size(144, 22);
            dataBaseManagementToolStripMenuItem.Text = "Data Base Management";
            // 
            // compareToActiveDBToolStripMenuItem
            // 
            compareToActiveDBToolStripMenuItem.Name = "compareToActiveDBToolStripMenuItem";
            compareToActiveDBToolStripMenuItem.Size = new Size(234, 22);
            compareToActiveDBToolStripMenuItem.Text = "Compare To Active DB";
            compareToActiveDBToolStripMenuItem.Click += compareToActiveDBToolStripMenuItem_Click;
            // 
            // cleanUpDBToolStripMenuItem
            // 
            cleanUpDBToolStripMenuItem.Name = "cleanUpDBToolStripMenuItem";
            cleanUpDBToolStripMenuItem.Size = new Size(234, 22);
            cleanUpDBToolStripMenuItem.Text = "Clean Up DB";
            cleanUpDBToolStripMenuItem.Click += cleanUpDBToolStripMenuItem_Click;
            // 
            // checkForCancelledShowsToolStripMenuItem
            // 
            checkForCancelledShowsToolStripMenuItem.Name = "checkForCancelledShowsToolStripMenuItem";
            checkForCancelledShowsToolStripMenuItem.Size = new Size(234, 22);
            checkForCancelledShowsToolStripMenuItem.Text = "Check For Cancelled Shows";
            checkForCancelledShowsToolStripMenuItem.Click += checkForCancelledShowsToolStripMenuItem_Click;
            // 
            // updateAdditionalFieldsToolStripMenuItem
            // 
            updateAdditionalFieldsToolStripMenuItem.Name = "updateAdditionalFieldsToolStripMenuItem";
            updateAdditionalFieldsToolStripMenuItem.Size = new Size(234, 22);
            updateAdditionalFieldsToolStripMenuItem.Text = "Update Additional Fields";
            updateAdditionalFieldsToolStripMenuItem.Click += updateAdditionalFieldsToolStripMenuItem_Click;
            // 
            // setYearOfInterestToolStripMenuItem
            // 
            setYearOfInterestToolStripMenuItem.Name = "setYearOfInterestToolStripMenuItem";
            setYearOfInterestToolStripMenuItem.Size = new Size(234, 22);
            setYearOfInterestToolStripMenuItem.Text = "Set Year of Interest";
            setYearOfInterestToolStripMenuItem.Click += setYearOfInterestToolStripMenuItem_Click;
            // 
            // exportContactsToolStripMenuItem
            // 
            exportContactsToolStripMenuItem.Name = "exportContactsToolStripMenuItem";
            exportContactsToolStripMenuItem.Size = new Size(234, 22);
            exportContactsToolStripMenuItem.Text = "Reform DB extracting contacts";
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { advancedSearchToolStripMenuItem });
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new Size(54, 22);
            searchToolStripMenuItem.Text = "Search";
            // 
            // advancedSearchToolStripMenuItem
            // 
            advancedSearchToolStripMenuItem.Name = "advancedSearchToolStripMenuItem";
            advancedSearchToolStripMenuItem.Size = new Size(165, 22);
            advancedSearchToolStripMenuItem.Text = "Advanced Search";
            advancedSearchToolStripMenuItem.Click += advancedSearchToolStripMenuItem_Click;
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
            // labelWeekendSelected
            // 
            labelWeekendSelected.Location = new Point(840, 44);
            labelWeekendSelected.Name = "labelWeekendSelected";
            labelWeekendSelected.Size = new Size(572, 15);
            labelWeekendSelected.TabIndex = 28;
            labelWeekendSelected.Text = "labelWeekendSelected";
            // 
            // linkLabelContacts
            // 
            linkLabelContacts.AutoSize = true;
            linkLabelContacts.Location = new Point(8, 35);
            linkLabelContacts.Name = "linkLabelContacts";
            linkLabelContacts.Size = new Size(101, 15);
            linkLabelContacts.TabIndex = 29;
            linkLabelContacts.TabStop = true;
            linkLabelContacts.Text = "linkLabelContacts";
            linkLabelContacts.LinkClicked += linkLabelContacts_LinkClicked;
            // 
            // linkLabelDB
            // 
            linkLabelDB.AutoSize = true;
            linkLabelDB.Location = new Point(8, 20);
            linkLabelDB.Name = "linkLabelDB";
            linkLabelDB.Size = new Size(69, 15);
            linkLabelDB.TabIndex = 30;
            linkLabelDB.TabStop = true;
            linkLabelDB.Text = "linkLabelDB";
            linkLabelDB.LinkClicked += linkLabelDB_LinkClicked;
            // 
            // formMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1419, 477);
            Controls.Add(linkLabelDB);
            Controls.Add(linkLabelContacts);
            Controls.Add(labelWeekendSelected);
            Controls.Add(btnDeleteShow);
            Controls.Add(btnFilterShows);
            Controls.Add(btnFilterClearAll);
            Controls.Add(btnFilterSetAll);
            Controls.Add(chklstRegions);
            Controls.Add(btnAddShow);
            Controls.Add(lstBoxShows);
            Controls.Add(dgvCalendar);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            Name = "formMain";
            Text = "Undaunted Airshows Schedule Maker";
            Load += frmAirshowScheduleTool_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void lstBoxShows_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return; // Ignore invalid indices

            // Get the current item
            Airshow ashow = (Airshow)lstBoxShows.Items[e.Index];

            // Determine the background color based on the item's status
            Color backgroundColor;
            switch (ashow.Status)
            {
                case Airshow.eStatus.maybe:
                    backgroundColor = Color.LightYellow;
                    break;
                case Airshow.eStatus.verbal:
                    backgroundColor = Color.LightGreen;
                    break;
                case Airshow.eStatus.contract:
                    backgroundColor = Color.LightBlue;
                    break;
                case Airshow.eStatus.pursue:
                    backgroundColor = Color.LightCoral;
                    break;
                case Airshow.eStatus.NO:
                    backgroundColor = Color.LightGray;
                    break;
                default:
                    backgroundColor = Color.White; // Default background color
                    break;
            }

            // Modify the background color if the item is selected
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backgroundColor = Color.DodgerBlue; // Highlighted background for selected items
            }

            // Draw the background
            using (Brush backgroundBrush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            }

            // Draw the text (use contrasting color for selected items)
            Color textColor = ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                ? Color.White
                : Color.Black;

            using (Brush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(
                    ashow.ToString(),     // Text to draw
                    e.Font,               // Use the ListBox's font
                    textBrush,            // Brush for text color
                    e.Bounds,             // Bounds of the current item
                    StringFormat.GenericDefault // Default text alignment
                );
            }

            // Draw the focus rectangle if the item has focus
            e.DrawFocusRectangle();
        }





        #endregion
        private ToolTip toolTip1;
        private DataGridView dgvCalendar;
        private ListBox lstBoxShows;
        private Button btnAddShow;
        private CheckedListBox chklstRegions;
        private Button btnFilterSetAll;
        private Button btnFilterClearAll;
        private Button btnFilterShows;
        private MenuStrip menuStrip1;
        private Button btnDeleteShow;
        private Button btnCheckForDuplicates;
        private ToolStripMenuItem searchToolStripMenuItem;
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
        private ToolStripMenuItem exportContactsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem setActiveContactFileToolStripMenuItem;
        private ToolStripMenuItem setActiveContactDBToolStripMenuItem;
        private ToolStripMenuItem advancedSearchToolStripMenuItem;
        private Label labelWeekendSelected;
        private LinkLabel linkLabelContacts;
        private LinkLabel linkLabelDB;
    }
}