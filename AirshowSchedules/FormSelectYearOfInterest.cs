using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AirshowSchedules
{
    public class FormYearSelction : Form
    {
        private ComboBox comboBoxYears;
        private Button buttonOK;

        public int SelectedYear { get; private set; }

        public FormYearSelction(int likelyYear)
        {
            InitializeComponent();
            SetLikelyYear(likelyYear);
        }

        private void InitializeComponent()
        {
            this.comboBoxYears = new ComboBox();
            this.buttonOK = new Button();

            // ComboBox
            this.comboBoxYears.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxYears.Location = new Point(10, 10);
            this.comboBoxYears.Width = 100;
            for (int year = 2024; year <= 2035; year++)
            {
                this.comboBoxYears.Items.Add(year);
            }

            // OK Button
            this.buttonOK.Text = "OK";
            this.buttonOK.Location = new Point(120, 10);
            this.buttonOK.Click += new EventHandler(this.ButtonOK_Click);

            // Form
            this.ClientSize = new Size(250, 50);
            this.Controls.Add(this.comboBoxYears);
            this.Controls.Add(this.buttonOK);
            this.Text = "Select Year of Interest";
        }

        private void SetLikelyYear(int likelyYear)
        {
            if (comboBoxYears.Items.Contains(likelyYear))
            {
                comboBoxYears.SelectedItem = likelyYear;
            }
            else
            {
                comboBoxYears.SelectedIndex = 0;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.SelectedYear = (int)this.comboBoxYears.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

