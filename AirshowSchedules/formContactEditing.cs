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
            private ListBox listBoxEmailAddresses;
            private Button buttonAddEmail;
            private Button buttonRemoveEmail;
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
                this.listBoxEmailAddresses = new ListBox();
                this.buttonAddEmail = new Button();
                this.buttonRemoveEmail = new Button();
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
    
                // ListBox for Email Addresses
                this.listBoxEmailAddresses.Location = new Point(10, 100);
                this.listBoxEmailAddresses.Width = 200;
                this.listBoxEmailAddresses.Height = 100;
    
                // Button to Add Email
                this.buttonAddEmail.Text = "Add Email";
                this.buttonAddEmail.Location = new Point(220, 100);
                this.buttonAddEmail.Click += new EventHandler(this.ButtonAddEmail_Click);
    
                // Button to Remove Email
                this.buttonRemoveEmail.Text = "Remove Email";
                this.buttonRemoveEmail.Location = new Point(220, 130);
                this.buttonRemoveEmail.Click += new EventHandler(this.ButtonRemoveEmail_Click);
    
                // OK Button
                this.buttonOK.Text = "OK";
                this.buttonOK.Location = new Point(10, 210);
                this.buttonOK.Click += new EventHandler(this.ButtonOK_Click);
    
                // Cancel Button
                this.buttonCancel.Text = "Cancel";
                this.buttonCancel.Location = new Point(100, 210);
                this.buttonCancel.Click += new EventHandler(this.ButtonCancel_Click);
    
                // Form
                this.ClientSize = new Size(350, 250);
                this.Controls.Add(this.textBoxName);
                this.Controls.Add(this.textBoxPhone);
                this.Controls.Add(this.textBoxAddress);
                this.Controls.Add(this.listBoxEmailAddresses);
                this.Controls.Add(this.buttonAddEmail);
                this.Controls.Add(this.buttonRemoveEmail);
                this.Controls.Add(this.buttonOK);
                this.Controls.Add(this.buttonCancel);
                this.Text = "Edit Contact";
            }
    
            private void BindData()
            {
                this.textBoxName.Text = Contact.name;
                this.textBoxPhone.Text = Contact.phone;
                this.textBoxAddress.Text = Contact.address;
                this.listBoxEmailAddresses.Items.AddRange(Contact.emailAddresses.ToArray());
            }
    
            private void ButtonAddEmail_Click(object sender, EventArgs e)
            {
                string newEmail = EmailPrompt.ShowDialog("Enter new email address:", "Add Email");
                if (!string.IsNullOrEmpty(newEmail))
                {
                    this.listBoxEmailAddresses.Items.Add(newEmail);
                }
            }
    
            private void ButtonRemoveEmail_Click(object sender, EventArgs e)
            {
                if (this.listBoxEmailAddresses.SelectedItem != null)
                {
                    this.listBoxEmailAddresses.Items.Remove(this.listBoxEmailAddresses.SelectedItem);
                }
            }
    
            private void ButtonOK_Click(object sender, EventArgs e)
            {
                Contact.name = this.textBoxName.Text;
                Contact.phone = this.textBoxPhone.Text;
                Contact.address = this.textBoxAddress.Text;
                Contact.emailAddresses = new List<string>(this.listBoxEmailAddresses.Items.Cast<string>());
    
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
    
            private void ButtonCancel_Click(object sender, EventArgs e)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    
        public static class EmailPrompt
        {
            public static string ShowDialog(string text, string caption)
            {
                using (Form prompt = new Form())
                {
                    prompt.Width = 500;
                    prompt.Height = 150;
                    prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                    prompt.Text = caption;
                    prompt.StartPosition = FormStartPosition.CenterScreen;
        
                    Label textLabel = new Label() { Left = 50, Top = 20, Text = text, AutoSize = true };
                    TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                    Button confirmation = new Button() { Text = "OK", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
        
                    confirmation.Click += (sender, e) => { prompt.Close(); };
        
                    prompt.Controls.Add(textBox);
                    prompt.Controls.Add(confirmation);
                    prompt.Controls.Add(textLabel);
                    prompt.AcceptButton = confirmation;
        
                    return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
                }
            }
        }
    }
