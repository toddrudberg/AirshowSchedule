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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAirshowScheduleTool));
      this.btnFileSetup = new System.Windows.Forms.Button();
      this.txtOutput = new System.Windows.Forms.TextBox();
      this.btnCopyToTabs = new System.Windows.Forms.Button();
      this.btnSaveAs = new System.Windows.Forms.Button();
      this.btnReadXML = new System.Windows.Forms.Button();
      this.btnCompareXMLs = new System.Windows.Forms.Button();
      this.txtRight = new System.Windows.Forms.TextBox();
      this.btnNewShows = new System.Windows.Forms.Button();
      this.btnCopyMiddle = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.dgvCalendar = new System.Windows.Forms.DataGridView();
      this.lstBoxShows = new System.Windows.Forms.ListBox();
      this.dataGridViewShows = new System.Windows.Forms.DataGridView();
      this.lblYearOfInterest = new System.Windows.Forms.Label();
      this.btnAddShow = new System.Windows.Forms.Button();
      this.chklstbox_diff = new System.Windows.Forms.CheckedListBox();
      this.btnCompareNewToDB = new System.Windows.Forms.Button();
      this.btnARCVDB = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.btnSetRegionFile = new System.Windows.Forms.Button();
      this.chklstRegions = new System.Windows.Forms.CheckedListBox();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.generateCallListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.generateBookedListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.generateICASMailingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.generateICASMailingListAllInRegionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.btnDeleteShow = new System.Windows.Forms.Button();
      this.button6 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShows)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnFileSetup
      // 
      this.btnFileSetup.Location = new System.Drawing.Point(25, 372);
      this.btnFileSetup.Margin = new System.Windows.Forms.Padding(5);
      this.btnFileSetup.Name = "btnFileSetup";
      this.btnFileSetup.Size = new System.Drawing.Size(142, 38);
      this.btnFileSetup.TabIndex = 0;
      this.btnFileSetup.Text = "Parse Data File";
      this.btnFileSetup.UseVisualStyleBackColor = true;
      this.btnFileSetup.Click += new System.EventHandler(this.btnParseAirshowDataFile_Click);
      // 
      // txtOutput
      // 
      this.txtOutput.Font = new System.Drawing.Font("Courier Std", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.txtOutput.Location = new System.Drawing.Point(25, 37);
      this.txtOutput.Margin = new System.Windows.Forms.Padding(5);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtOutput.Size = new System.Drawing.Size(662, 327);
      this.txtOutput.TabIndex = 1;
      // 
      // btnCopyToTabs
      // 
      this.btnCopyToTabs.Location = new System.Drawing.Point(487, 372);
      this.btnCopyToTabs.Margin = new System.Windows.Forms.Padding(5);
      this.btnCopyToTabs.Name = "btnCopyToTabs";
      this.btnCopyToTabs.Size = new System.Drawing.Size(197, 38);
      this.btnCopyToTabs.TabIndex = 2;
      this.btnCopyToTabs.Text = "Copy to Tab Delim";
      this.btnCopyToTabs.UseVisualStyleBackColor = true;
      this.btnCopyToTabs.Click += new System.EventHandler(this.btnCopyToTabs_Click);
      // 
      // btnSaveAs
      // 
      this.btnSaveAs.Location = new System.Drawing.Point(375, 372);
      this.btnSaveAs.Margin = new System.Windows.Forms.Padding(5);
      this.btnSaveAs.Name = "btnSaveAs";
      this.btnSaveAs.Size = new System.Drawing.Size(107, 38);
      this.btnSaveAs.TabIndex = 3;
      this.btnSaveAs.Text = "Save As";
      this.btnSaveAs.UseVisualStyleBackColor = true;
      this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
      // 
      // btnReadXML
      // 
      this.btnReadXML.Location = new System.Drawing.Point(25, 418);
      this.btnReadXML.Margin = new System.Windows.Forms.Padding(5);
      this.btnReadXML.Name = "btnReadXML";
      this.btnReadXML.Size = new System.Drawing.Size(298, 38);
      this.btnReadXML.TabIndex = 4;
      this.btnReadXML.Text = "Set actv DB File";
      this.btnReadXML.UseVisualStyleBackColor = true;
      this.btnReadXML.Click += new System.EventHandler(this.btnReadXML_Click);
      // 
      // btnCompareXMLs
      // 
      this.btnCompareXMLs.Location = new System.Drawing.Point(697, 375);
      this.btnCompareXMLs.Margin = new System.Windows.Forms.Padding(5);
      this.btnCompareXMLs.Name = "btnCompareXMLs";
      this.btnCompareXMLs.Size = new System.Drawing.Size(187, 38);
      this.btnCompareXMLs.TabIndex = 5;
      this.btnCompareXMLs.Text = "Compare XML Files";
      this.toolTip1.SetToolTip(this.btnCompareXMLs, "Loops through the right DB and sees if it exists in the left DB.  If not, it retu" +
        "rns it to the box above.");
      this.btnCompareXMLs.UseVisualStyleBackColor = true;
      this.btnCompareXMLs.Click += new System.EventHandler(this.btnCompareXMLs_Click);
      // 
      // txtRight
      // 
      this.txtRight.Font = new System.Drawing.Font("Courier Std", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.txtRight.Location = new System.Drawing.Point(1372, 40);
      this.txtRight.Margin = new System.Windows.Forms.Padding(5);
      this.txtRight.Multiline = true;
      this.txtRight.Name = "txtRight";
      this.txtRight.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtRight.Size = new System.Drawing.Size(662, 322);
      this.txtRight.TabIndex = 7;
      // 
      // btnNewShows
      // 
      this.btnNewShows.Location = new System.Drawing.Point(1198, 375);
      this.btnNewShows.Margin = new System.Windows.Forms.Padding(5);
      this.btnNewShows.Name = "btnNewShows";
      this.btnNewShows.Size = new System.Drawing.Size(153, 38);
      this.btnNewShows.TabIndex = 8;
      this.btnNewShows.Text = "Compare Years";
      this.btnNewShows.UseVisualStyleBackColor = true;
      this.btnNewShows.Click += new System.EventHandler(this.btnNewShows_Click);
      // 
      // btnCopyMiddle
      // 
      this.btnCopyMiddle.Location = new System.Drawing.Point(697, 418);
      this.btnCopyMiddle.Margin = new System.Windows.Forms.Padding(5);
      this.btnCopyMiddle.Name = "btnCopyMiddle";
      this.btnCopyMiddle.Size = new System.Drawing.Size(365, 38);
      this.btnCopyMiddle.TabIndex = 9;
      this.btnCopyMiddle.Text = "Copy to Tab Delim";
      this.btnCopyMiddle.UseVisualStyleBackColor = true;
      this.btnCopyMiddle.Click += new System.EventHandler(this.btnCopyMiddle_Click);
      // 
      // dgvCalendar
      // 
      this.dgvCalendar.AllowUserToAddRows = false;
      this.dgvCalendar.AllowUserToDeleteRows = false;
      this.dgvCalendar.AllowUserToResizeColumns = false;
      this.dgvCalendar.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvCalendar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvCalendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCalendar.Location = new System.Drawing.Point(25, 495);
      this.dgvCalendar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.dgvCalendar.MultiSelect = false;
      this.dgvCalendar.Name = "dgvCalendar";
      this.dgvCalendar.RowHeadersWidth = 82;
      this.dgvCalendar.RowTemplate.Height = 41;
      this.dgvCalendar.Size = new System.Drawing.Size(1182, 690);
      this.dgvCalendar.TabIndex = 11;
      this.dgvCalendar.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
      this.dgvCalendar.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
      this.dgvCalendar.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView2_ColumnAdded);
      // 
      // lstBoxShows
      // 
      this.lstBoxShows.FormattingEnabled = true;
      this.lstBoxShows.ItemHeight = 25;
      this.lstBoxShows.Location = new System.Drawing.Point(2347, 770);
      this.lstBoxShows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.lstBoxShows.Name = "lstBoxShows";
      this.lstBoxShows.Size = new System.Drawing.Size(609, 579);
      this.lstBoxShows.TabIndex = 12;
      this.lstBoxShows.SelectedIndexChanged += new System.EventHandler(this.lstBoxShows_SelectedIndexChanged);
      // 
      // dataGridViewShows
      // 
      this.dataGridViewShows.AllowUserToAddRows = false;
      this.dataGridViewShows.AllowUserToDeleteRows = false;
      this.dataGridViewShows.AllowUserToResizeColumns = false;
      this.dataGridViewShows.AllowUserToResizeRows = false;
      this.dataGridViewShows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewShows.Location = new System.Drawing.Point(1212, 495);
      this.dataGridViewShows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.dataGridViewShows.MultiSelect = false;
      this.dataGridViewShows.Name = "dataGridViewShows";
      this.dataGridViewShows.RowHeadersWidth = 82;
      this.dataGridViewShows.RowTemplate.Height = 41;
      this.dataGridViewShows.Size = new System.Drawing.Size(823, 387);
      this.dataGridViewShows.TabIndex = 13;
      this.dataGridViewShows.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewShows_CellClick);
      this.dataGridViewShows.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewShows_CellContentClick);
      // 
      // lblYearOfInterest
      // 
      this.lblYearOfInterest.AutoSize = true;
      this.lblYearOfInterest.Location = new System.Drawing.Point(22, 462);
      this.lblYearOfInterest.Name = "lblYearOfInterest";
      this.lblYearOfInterest.Size = new System.Drawing.Size(59, 25);
      this.lblYearOfInterest.TabIndex = 14;
      this.lblYearOfInterest.Text = "label1";
      // 
      // btnAddShow
      // 
      this.btnAddShow.Location = new System.Drawing.Point(1212, 893);
      this.btnAddShow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnAddShow.Name = "btnAddShow";
      this.btnAddShow.Size = new System.Drawing.Size(177, 37);
      this.btnAddShow.TabIndex = 15;
      this.btnAddShow.Text = "Add A Show";
      this.btnAddShow.UseVisualStyleBackColor = true;
      this.btnAddShow.Click += new System.EventHandler(this.btnAddShow_Click);
      // 
      // chklstbox_diff
      // 
      this.chklstbox_diff.FormattingEnabled = true;
      this.chklstbox_diff.Location = new System.Drawing.Point(702, 40);
      this.chklstbox_diff.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.chklstbox_diff.Name = "chklstbox_diff";
      this.chklstbox_diff.Size = new System.Drawing.Size(652, 312);
      this.chklstbox_diff.TabIndex = 16;
      // 
      // btnCompareNewToDB
      // 
      this.btnCompareNewToDB.Location = new System.Drawing.Point(895, 375);
      this.btnCompareNewToDB.Margin = new System.Windows.Forms.Padding(5);
      this.btnCompareNewToDB.Name = "btnCompareNewToDB";
      this.btnCompareNewToDB.Size = new System.Drawing.Size(170, 38);
      this.btnCompareNewToDB.TabIndex = 17;
      this.btnCompareNewToDB.Text = "new to actv DB";
      this.btnCompareNewToDB.UseVisualStyleBackColor = true;
      this.btnCompareNewToDB.Click += new System.EventHandler(this.btnCompareNewToDB_Click);
      // 
      // btnARCVDB
      // 
      this.btnARCVDB.Location = new System.Drawing.Point(375, 418);
      this.btnARCVDB.Margin = new System.Windows.Forms.Padding(5);
      this.btnARCVDB.Name = "btnARCVDB";
      this.btnARCVDB.Size = new System.Drawing.Size(310, 38);
      this.btnARCVDB.TabIndex = 18;
      this.btnARCVDB.Text = "Archive actv DB File";
      this.btnARCVDB.UseVisualStyleBackColor = true;
      this.btnARCVDB.Click += new System.EventHandler(this.btnARCVDB_Click);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(1068, 418);
      this.button1.Margin = new System.Windows.Forms.Padding(5);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(285, 38);
      this.button1.TabIndex = 19;
      this.button1.Text = "Add Selected to actv DB";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(2155, 103);
      this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(268, 37);
      this.button2.TabIndex = 20;
      this.button2.Text = "Builds the Region File";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // btnSetRegionFile
      // 
      this.btnSetRegionFile.Location = new System.Drawing.Point(175, 372);
      this.btnSetRegionFile.Margin = new System.Windows.Forms.Padding(5);
      this.btnSetRegionFile.Name = "btnSetRegionFile";
      this.btnSetRegionFile.Size = new System.Drawing.Size(142, 38);
      this.btnSetRegionFile.TabIndex = 21;
      this.btnSetRegionFile.Text = "Set Region File";
      this.btnSetRegionFile.UseVisualStyleBackColor = true;
      this.btnSetRegionFile.Click += new System.EventHandler(this.btnSetRegionFile_Click);
      // 
      // chklstRegions
      // 
      this.chklstRegions.FormattingEnabled = true;
      this.chklstRegions.Location = new System.Drawing.Point(1733, 893);
      this.chklstRegions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.chklstRegions.Name = "chklstRegions";
      this.chklstRegions.Size = new System.Drawing.Size(301, 256);
      this.chklstRegions.TabIndex = 22;
      this.chklstRegions.SelectedIndexChanged += new System.EventHandler(this.chklstRegions_SelectedIndexChanged);
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(1797, 1153);
      this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(115, 37);
      this.button3.TabIndex = 23;
      this.button3.Text = "Set All";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(1917, 1153);
      this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(115, 37);
      this.button4.TabIndex = 24;
      this.button4.Text = "Clear All";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(1677, 1153);
      this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(115, 37);
      this.button5.TabIndex = 25;
      this.button5.Text = "Filter List";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportsToolStripMenuItem,
            this.searchToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
      this.menuStrip1.Size = new System.Drawing.Size(2052, 33);
      this.menuStrip1.TabIndex = 26;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // reportsToolStripMenuItem
      // 
      this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateCallListToolStripMenuItem,
            this.generateBookedListToolStripMenuItem,
            this.generateICASMailingListToolStripMenuItem,
            this.generateICASMailingListAllInRegionToolStripMenuItem});
      this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
      this.reportsToolStripMenuItem.Size = new System.Drawing.Size(89, 29);
      this.reportsToolStripMenuItem.Text = "Reports";
      // 
      // generateCallListToolStripMenuItem
      // 
      this.generateCallListToolStripMenuItem.Name = "generateCallListToolStripMenuItem";
      this.generateCallListToolStripMenuItem.Size = new System.Drawing.Size(321, 34);
      this.generateCallListToolStripMenuItem.Text = "Generate Call List";
      this.generateCallListToolStripMenuItem.Click += new System.EventHandler(this.generateCallListToolStripMenuItem_Click);
      // 
      // generateBookedListToolStripMenuItem
      // 
      this.generateBookedListToolStripMenuItem.Name = "generateBookedListToolStripMenuItem";
      this.generateBookedListToolStripMenuItem.Size = new System.Drawing.Size(321, 34);
      this.generateBookedListToolStripMenuItem.Text = "Generate Booked List";
      this.generateBookedListToolStripMenuItem.Click += new System.EventHandler(this.generateBookedListToolStripMenuItem_Click);
      // 
      // generateICASMailingListToolStripMenuItem
      // 
      this.generateICASMailingListToolStripMenuItem.Name = "generateICASMailingListToolStripMenuItem";
      this.generateICASMailingListToolStripMenuItem.Size = new System.Drawing.Size(321, 34);
      this.generateICASMailingListToolStripMenuItem.Text = "Generate ICAS Mailing List";
      this.generateICASMailingListToolStripMenuItem.Click += new System.EventHandler(this.generateICASMailingListToolStripMenuItem_Click);
      // 
      // generateICASMailingListAllInRegionToolStripMenuItem
      // 
      this.generateICASMailingListAllInRegionToolStripMenuItem.Name = "generateICASMailingListAllInRegionToolStripMenuItem";
      this.generateICASMailingListAllInRegionToolStripMenuItem.Size = new System.Drawing.Size(321, 34);
      this.generateICASMailingListAllInRegionToolStripMenuItem.Text = "All in Filtered Region";
      this.generateICASMailingListAllInRegionToolStripMenuItem.Click += new System.EventHandler(this.generateICASMailingListAllInRegionToolStripMenuItem_Click);
      // 
      // searchToolStripMenuItem
      // 
      this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contactToolStripMenuItem});
      this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
      this.searchToolStripMenuItem.Size = new System.Drawing.Size(80, 29);
      this.searchToolStripMenuItem.Text = "Search";
      // 
      // contactToolStripMenuItem
      // 
      this.contactToolStripMenuItem.Name = "contactToolStripMenuItem";
      this.contactToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
      this.contactToolStripMenuItem.Text = "Search Dialog";
      this.contactToolStripMenuItem.Click += new System.EventHandler(this.contactToolStripMenuItem_Click);
      // 
      // btnDeleteShow
      // 
      this.btnDeleteShow.Location = new System.Drawing.Point(1212, 933);
      this.btnDeleteShow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnDeleteShow.Name = "btnDeleteShow";
      this.btnDeleteShow.Size = new System.Drawing.Size(177, 37);
      this.btnDeleteShow.TabIndex = 27;
      this.btnDeleteShow.Text = "Delete A Show";
      this.btnDeleteShow.UseVisualStyleBackColor = true;
      this.btnDeleteShow.Click += new System.EventHandler(this.btnDeleteShow_Click);
      // 
      // button6
      // 
      this.button6.Location = new System.Drawing.Point(1359, 418);
      this.button6.Margin = new System.Windows.Forms.Padding(5);
      this.button6.Name = "button6";
      this.button6.Size = new System.Drawing.Size(285, 69);
      this.button6.TabIndex = 28;
      this.button6.Text = "Check Selected For Dups in actv DB";
      this.button6.UseVisualStyleBackColor = true;
      this.button6.Click += new System.EventHandler(this.button6_Click);
      // 
      // frmAirshowScheduleTool
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(2052, 1273);
      this.Controls.Add(this.button6);
      this.Controls.Add(this.btnDeleteShow);
      this.Controls.Add(this.button5);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.chklstRegions);
      this.Controls.Add(this.btnSetRegionFile);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.btnARCVDB);
      this.Controls.Add(this.btnCompareNewToDB);
      this.Controls.Add(this.chklstbox_diff);
      this.Controls.Add(this.btnAddShow);
      this.Controls.Add(this.lblYearOfInterest);
      this.Controls.Add(this.dataGridViewShows);
      this.Controls.Add(this.lstBoxShows);
      this.Controls.Add(this.dgvCalendar);
      this.Controls.Add(this.btnCopyMiddle);
      this.Controls.Add(this.btnNewShows);
      this.Controls.Add(this.txtRight);
      this.Controls.Add(this.btnCompareXMLs);
      this.Controls.Add(this.btnReadXML);
      this.Controls.Add(this.btnSaveAs);
      this.Controls.Add(this.btnCopyToTabs);
      this.Controls.Add(this.txtOutput);
      this.Controls.Add(this.btnFileSetup);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new System.Windows.Forms.Padding(5);
      this.Name = "frmAirshowScheduleTool";
      this.Text = "Undaunted Airshows Schedule Maker";
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShows)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

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
        private Button button6;
    private ToolStripMenuItem searchToolStripMenuItem;
    private ToolStripMenuItem contactToolStripMenuItem;
  }
}