using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AirshowSchedules.Airshow;


namespace AirshowSchedules
{
    public class ContactEditForm : Form
    {
        private TextBox textBoxName;
        private TextBox textBoxPhone;
        private TextBox textBoxAddress;
        private Button buttonOK;
        private Button buttonCancel;

        public cContact Contact { get; private set; }

        public ContactEditForm(cContact contact)
        {
            this.Contact = contact;
            InitializeComponent();
            BindData();
        }

        private void InitializeComponent()
        {
            this.textBoxName = new TextBox();
            this.textBoxPhone = new TextBox();
            this.textBoxAddress = new TextBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();

            // TextBox for Name
            this.textBoxName.Location = new Point(10, 10);
            this.textBoxName.Width = 200;
            this.textBoxName.PlaceholderText = "Name";

            // TextBox for Phone
            this.textBoxPhone.Location = new Point(10, 40);
            this.textBoxPhone.Width = 200;
            this.textBoxPhone.PlaceholderText = "Phone";

            // TextBox for Address
            this.textBoxAddress.Location = new Point(10, 70);
            this.textBoxAddress.Width = 200;
            this.textBoxAddress.PlaceholderText = "Address";

            // OK Button
            this.buttonOK.Text = "OK";
            this.buttonOK.Location = new Point(10, 100);
            this.buttonOK.Click += new EventHandler(this.ButtonOK_Click);

            // Cancel Button
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Location = new Point(100, 100);
            this.buttonCancel.Click += new EventHandler(this.ButtonCancel_Click);

            // Form
            this.ClientSize = new Size(250, 150);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.textBoxPhone);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Text = "Edit Contact";
        }

        private void BindData()
        {
            this.textBoxName.Text = Contact.name;
            this.textBoxPhone.Text = Contact.phone;
            this.textBoxAddress.Text = Contact.address;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Contact.name = this.textBoxName.Text;
            Contact.phone = this.textBoxPhone.Text;
            Contact.address = this.textBoxAddress.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
