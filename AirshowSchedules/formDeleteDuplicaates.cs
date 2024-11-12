using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace AirshowSchedules;
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
        checkedListBox = new CheckedListBox();
        btnDelete = new Button();
        btnCancel = new Button();
        btnClose = new Button();
        SuspendLayout();
        // 
        // checkedListBox
        // 
        checkedListBox.FormattingEnabled = true;
        checkedListBox.Location = new Point(14, 14);
        checkedListBox.Margin = new Padding(4, 3, 4, 3);
        checkedListBox.Name = "checkedListBox";
        checkedListBox.Size = new Size(466, 202);
        checkedListBox.TabIndex = 0;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(14, 233);
        btnDelete.Margin = new Padding(4, 3, 4, 3);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(88, 27);
        btnDelete.TabIndex = 1;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(203, 233);
        btnCancel.Margin = new Padding(4, 3, 4, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(88, 27);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // btnClose
        // 
        btnClose.Location = new Point(393, 233);
        btnClose.Margin = new Padding(4, 3, 4, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(88, 27);
        btnClose.TabIndex = 3;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;
        // 
        // DeleteAirshowForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(490, 273);
        Controls.Add(btnClose);
        Controls.Add(btnCancel);
        Controls.Add(btnDelete);
        Controls.Add(checkedListBox);
        Margin = new Padding(4, 3, 4, 3);
        Name = "DeleteAirshowForm";
        Text = "Delete Duplicate Airshows";
        ResumeLayout(false);
    }

    private System.Windows.Forms.CheckedListBox checkedListBox;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnClose;

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
            checkedListBox.Items.Remove(item);
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
}