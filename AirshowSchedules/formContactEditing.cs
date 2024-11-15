using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private Button buttonEditEmail;
        private Button buttonCopyEmail;
        private Button buttonOK;
        private Button buttonCancel;
        private Label label1;

        public cContact Contact { get; private set; }

        public ContactEditForm(cContact contact)
        {
            this.Contact = contact;
            InitializeComponent();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            BindData();
        }

        private void InitializeComponent()
        {
            textBoxName = new TextBox();
            textBoxPhone = new TextBox();
            textBoxAddress = new TextBox();
            listBoxEmailAddresses = new ListBox();
            buttonAddEmail = new Button();
            buttonRemoveEmail = new Button();
            buttonEditEmail = new Button();
            buttonCopyEmail = new Button();
            buttonOK = new Button();
            buttonCancel = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(10, 10);
            textBoxName.Name = "textBoxName";
            textBoxName.PlaceholderText = "Name";
            textBoxName.Size = new Size(200, 23);
            textBoxName.TabIndex = 0;
            // 
            // textBoxPhone
            // 
            textBoxPhone.Location = new Point(10, 40);
            textBoxPhone.Name = "textBoxPhone";
            textBoxPhone.PlaceholderText = "Phone";
            textBoxPhone.Size = new Size(200, 23);
            textBoxPhone.TabIndex = 1;
            // 
            // textBoxAddress
            // 
            textBoxAddress.Location = new Point(10, 70);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.PlaceholderText = "Address";
            textBoxAddress.Size = new Size(200, 23);
            textBoxAddress.TabIndex = 2;
            // 
            // listBoxEmailAddresses
            // 
            listBoxEmailAddresses.ItemHeight = 15;
            listBoxEmailAddresses.Location = new Point(10, 115);
            listBoxEmailAddresses.Name = "listBoxEmailAddresses";
            listBoxEmailAddresses.Size = new Size(200, 94);
            listBoxEmailAddresses.TabIndex = 3;
            // 
            // buttonAddEmail
            // 
            buttonAddEmail.Location = new Point(220, 115);
            buttonAddEmail.Name = "buttonAddEmail";
            buttonAddEmail.Size = new Size(75, 23);
            buttonAddEmail.TabIndex = 4;
            buttonAddEmail.Text = "Add Email";
            buttonAddEmail.Click += ButtonAddEmail_Click;
            // 
            // buttonRemoveEmail
            // 
            buttonRemoveEmail.Location = new Point(220, 145);
            buttonRemoveEmail.Name = "buttonRemoveEmail";
            buttonRemoveEmail.Size = new Size(75, 23);
            buttonRemoveEmail.TabIndex = 5;
            buttonRemoveEmail.Text = "Remove Email";
            buttonRemoveEmail.Click += ButtonRemoveEmail_Click;
            // 
            // buttonEditEmail
            // 
            buttonEditEmail.Location = new Point(220, 175);
            buttonEditEmail.Name = "buttonEditEmail";
            buttonEditEmail.Size = new Size(75, 23);
            buttonEditEmail.TabIndex = 6;
            buttonEditEmail.Text = "Edit Email";
            buttonEditEmail.Click += ButtonEditEmail_Click;
            // 
            // buttonCopyEmail
            // 
            buttonCopyEmail.Location = new Point(220, 205);
            buttonCopyEmail.Name = "buttonCopyEmail";
            buttonCopyEmail.Size = new Size(75, 23);
            buttonCopyEmail.TabIndex = 7;
            buttonCopyEmail.Text = "Copy Email";
            buttonCopyEmail.Click += ButtonCopyEmail_Click;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(10, 225);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 8;
            buttonOK.Text = "OK";
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(100, 225);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 9;
            buttonCancel.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 96);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 10;
            label1.Text = "Email Addresses:";
            // 
            // ContactEditForm
            // 
            ClientSize = new Size(350, 270);
            Controls.Add(label1);
            Controls.Add(textBoxName);
            Controls.Add(textBoxPhone);
            Controls.Add(textBoxAddress);
            Controls.Add(listBoxEmailAddresses);
            Controls.Add(buttonAddEmail);
            Controls.Add(buttonRemoveEmail);
            Controls.Add(buttonEditEmail);
            Controls.Add(buttonCopyEmail);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            Name = "ContactEditForm";
            Text = "Edit Contact";
            ResumeLayout(false);
            PerformLayout();
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

        private void ButtonEditEmail_Click(object sender, EventArgs e)
        {
            if (this.listBoxEmailAddresses.SelectedItem != null)
            {
                string selectedEmail = this.listBoxEmailAddresses.SelectedItem.ToString();
                string editedEmail = EmailPrompt.ShowDialog("Edit email address:", "Edit Email", selectedEmail);
                if (!string.IsNullOrEmpty(editedEmail))
                {
                    int selectedIndex = this.listBoxEmailAddresses.SelectedIndex;
                    this.listBoxEmailAddresses.Items[selectedIndex] = editedEmail;
                }
            }
        }

        private void ButtonCopyEmail_Click(object sender, EventArgs e)
        {
            if (this.listBoxEmailAddresses.SelectedItem != null)
            {
                string selectedEmail = this.listBoxEmailAddresses.SelectedItem.ToString();
                Clipboard.SetText(selectedEmail);
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
        public static string ShowDialog(string text, string caption, string existingText = "")
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 500;
                prompt.Height = 150;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = caption;
                prompt.StartPosition = FormStartPosition.CenterScreen;

                Label textLabel = new Label() { Left = 50, Top = 20, Text = text, AutoSize = true };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 300, Text = existingText };
                Button confirmation = new Button() { Text = "OK", Left = 360, Width = 100, Top = 50, DialogResult = DialogResult.OK };

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