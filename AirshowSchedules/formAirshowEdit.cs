using static AirshowSchedules.Airshow;

namespace AirshowSchedules
{
    public class AirshowEditForm : Form
    {
        private TextBox textBoxDateStart;
        private TextBox textBoxDateFinish;
        private TextBox textBoxNameAirshow;
        private TextBox textBoxLocationCity;
        private TextBox textBoxLocationState;
        private TextBox textBoxNotes;
        private ComboBox comboBoxStatus;
        private ListBox listBoxPerformers;
        private ListBox listBoxContacts;
        private ListBox listBoxUndauntedNotes;
        private Button buttonAddNote;
        private Button buttonRemoveNote;
        private Button buttonOK;
        private Button buttonCancel;
        private Label label1;
        private Button buttonAddContact;
        private Button buttonRemoveContact;
        private Label label2;
        private Label label3;
        private ListBox listBoxWeblinks;
        private Label label4;
        private Button buttonAddLink;
        private Button btnRemoveLink;
        private Airshow airshow;
        private List<cContact> contacts;

        public AirshowEditForm(Airshow airshow, List<cContact> contacts)
        {
            this.airshow = airshow;
            this.contacts = contacts;
            InitializeComponent();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;



            BindData();
        }

        private void InitializeComponent()
        {
            textBoxDateStart = new TextBox();
            textBoxDateFinish = new TextBox();
            textBoxNameAirshow = new TextBox();
            textBoxLocationCity = new TextBox();
            textBoxLocationState = new TextBox();
            textBoxNotes = new TextBox();
            comboBoxStatus = new ComboBox();
            listBoxPerformers = new ListBox();
            listBoxContacts = new ListBox();
            listBoxUndauntedNotes = new ListBox();
            buttonAddNote = new Button();
            buttonRemoveNote = new Button();
            buttonOK = new Button();
            buttonCancel = new Button();
            label1 = new Label();
            buttonAddContact = new Button();
            buttonRemoveContact = new Button();
            label2 = new Label();
            label3 = new Label();
            listBoxWeblinks = new ListBox();
            label4 = new Label();
            buttonAddLink = new Button();
            btnRemoveLink = new Button();
            SuspendLayout();
            // 
            // textBoxDateStart
            // 
            textBoxDateStart.Location = new Point(10, 10);
            textBoxDateStart.Name = "textBoxDateStart";
            textBoxDateStart.PlaceholderText = "Start Date (yyyy-MM-DD)";
            textBoxDateStart.Size = new Size(200, 23);
            textBoxDateStart.TabIndex = 0;
            textBoxDateStart.Click += TextBoxDateStart_Enter;
            // 
            // textBoxDateFinish
            // 
            textBoxDateFinish.Location = new Point(10, 40);
            textBoxDateFinish.Name = "textBoxDateFinish";
            textBoxDateFinish.PlaceholderText = "Finish Date (yyyy-MM-DD)";
            textBoxDateFinish.Size = new Size(200, 23);
            textBoxDateFinish.TabIndex = 1;
            textBoxDateFinish.Click += TextBoxDateFinish_Enter;
            // 
            // textBoxNameAirshow
            // 
            textBoxNameAirshow.Location = new Point(10, 70);
            textBoxNameAirshow.Name = "textBoxNameAirshow";
            textBoxNameAirshow.PlaceholderText = "Name of Airshow";
            textBoxNameAirshow.Size = new Size(200, 23);
            textBoxNameAirshow.TabIndex = 2;
            // 
            // textBoxLocationCity
            // 
            textBoxLocationCity.Location = new Point(10, 100);
            textBoxLocationCity.Name = "textBoxLocationCity";
            textBoxLocationCity.PlaceholderText = "City";
            textBoxLocationCity.Size = new Size(200, 23);
            textBoxLocationCity.TabIndex = 3;
            // 
            // textBoxLocationState
            // 
            textBoxLocationState.Location = new Point(10, 130);
            textBoxLocationState.Name = "textBoxLocationState";
            textBoxLocationState.PlaceholderText = "State";
            textBoxLocationState.Size = new Size(200, 23);
            textBoxLocationState.TabIndex = 4;
            // 
            // textBoxNotes
            // 
            textBoxNotes.Location = new Point(10, 160);
            textBoxNotes.Multiline = true;
            textBoxNotes.Name = "textBoxNotes";
            textBoxNotes.PlaceholderText = "Notes";
            textBoxNotes.Size = new Size(200, 60);
            textBoxNotes.TabIndex = 5;
            // 
            // comboBoxStatus
            // 
            comboBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStatus.Items.AddRange(new object[] { "contract", "verbal", "maybe", "pursue", "none", "NO" });
            comboBoxStatus.Location = new Point(10, 230);
            comboBoxStatus.Name = "comboBoxStatus";
            comboBoxStatus.Size = new Size(200, 23);
            comboBoxStatus.TabIndex = 6;
            // 
            // listBoxPerformers
            // 
            listBoxPerformers.ItemHeight = 15;
            listBoxPerformers.Location = new Point(216, 26);
            listBoxPerformers.Name = "listBoxPerformers";
            listBoxPerformers.Size = new Size(200, 64);
            listBoxPerformers.TabIndex = 7;
            // 
            // listBoxContacts
            // 
            listBoxContacts.ItemHeight = 15;
            listBoxContacts.Location = new Point(216, 108);
            listBoxContacts.Name = "listBoxContacts";
            listBoxContacts.Size = new Size(200, 94);
            listBoxContacts.TabIndex = 8;
            listBoxContacts.Click += ListBoxContacts_Click;
            // 
            // listBoxUndauntedNotes
            // 
            listBoxUndauntedNotes.ItemHeight = 15;
            listBoxUndauntedNotes.Location = new Point(10, 384);
            listBoxUndauntedNotes.Name = "listBoxUndauntedNotes";
            listBoxUndauntedNotes.Size = new Size(406, 94);
            listBoxUndauntedNotes.TabIndex = 9;
            listBoxUndauntedNotes.SelectedIndexChanged += listBoxUndauntedNotes_SelectedIndexChanged;
            // 
            // buttonAddNote
            // 
            buttonAddNote.Location = new Point(10, 484);
            buttonAddNote.Name = "buttonAddNote";
            buttonAddNote.Size = new Size(75, 23);
            buttonAddNote.TabIndex = 10;
            buttonAddNote.Text = "Add";
            buttonAddNote.Click += ButtonAddNote_Click;
            // 
            // buttonRemoveNote
            // 
            buttonRemoveNote.Location = new Point(91, 484);
            buttonRemoveNote.Name = "buttonRemoveNote";
            buttonRemoveNote.Size = new Size(75, 23);
            buttonRemoveNote.TabIndex = 11;
            buttonRemoveNote.Text = "Remove";
            buttonRemoveNote.Click += ButtonRemoveNote_Click;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(247, 579);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 13;
            buttonOK.Text = "OK";
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(337, 579);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 14;
            buttonCancel.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 366);
            label1.Name = "label1";
            label1.Size = new Size(103, 15);
            label1.TabIndex = 15;
            label1.Text = "Undaunted Notes:";
            // 
            // buttonAddContact
            // 
            buttonAddContact.Location = new Point(216, 208);
            buttonAddContact.Name = "buttonAddContact";
            buttonAddContact.Size = new Size(75, 23);
            buttonAddContact.TabIndex = 16;
            buttonAddContact.Text = "Add";
            buttonAddContact.UseVisualStyleBackColor = true;
            buttonAddContact.Click += buttonAddContact_Click;
            // 
            // buttonRemoveContact
            // 
            buttonRemoveContact.Location = new Point(297, 208);
            buttonRemoveContact.Name = "buttonRemoveContact";
            buttonRemoveContact.Size = new Size(75, 23);
            buttonRemoveContact.TabIndex = 17;
            buttonRemoveContact.Text = "Remove";
            buttonRemoveContact.UseVisualStyleBackColor = true;
            buttonRemoveContact.Click += buttonRemoveContact_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(216, 91);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 18;
            label2.Text = "Contacts:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(216, 9);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 19;
            label3.Text = "Performers:";
            // 
            // listBoxWeblinks
            // 
            listBoxWeblinks.ItemHeight = 15;
            listBoxWeblinks.Location = new Point(10, 278);
            listBoxWeblinks.Name = "listBoxWeblinks";
            listBoxWeblinks.Size = new Size(406, 49);
            listBoxWeblinks.TabIndex = 20;
            listBoxWeblinks.SelectedIndexChanged += listBoxWeblinks_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 260);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 21;
            label4.Text = "Web links:";
            // 
            // buttonAddLink
            // 
            buttonAddLink.Location = new Point(10, 333);
            buttonAddLink.Name = "buttonAddLink";
            buttonAddLink.Size = new Size(75, 23);
            buttonAddLink.TabIndex = 22;
            buttonAddLink.Text = "Add";
            buttonAddLink.Click += buttonAddLink_Click;
            // 
            // btnRemoveLink
            // 
            btnRemoveLink.Location = new Point(91, 333);
            btnRemoveLink.Name = "btnRemoveLink";
            btnRemoveLink.Size = new Size(75, 23);
            btnRemoveLink.TabIndex = 23;
            btnRemoveLink.Text = "Remove";
            btnRemoveLink.Click += btnRemoveLink_Click;
            // 
            // AirshowEditForm
            // 
            ClientSize = new Size(424, 614);
            Controls.Add(buttonAddLink);
            Controls.Add(btnRemoveLink);
            Controls.Add(label4);
            Controls.Add(listBoxWeblinks);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(buttonRemoveContact);
            Controls.Add(buttonAddContact);
            Controls.Add(label1);
            Controls.Add(textBoxDateStart);
            Controls.Add(textBoxDateFinish);
            Controls.Add(textBoxNameAirshow);
            Controls.Add(textBoxLocationCity);
            Controls.Add(textBoxLocationState);
            Controls.Add(textBoxNotes);
            Controls.Add(comboBoxStatus);
            Controls.Add(listBoxPerformers);
            Controls.Add(listBoxContacts);
            Controls.Add(listBoxUndauntedNotes);
            Controls.Add(buttonAddNote);
            Controls.Add(buttonRemoveNote);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            Name = "AirshowEditForm";
            Text = "Edit Airshow";
            ResumeLayout(false);
            PerformLayout();
        }

        private void TextBoxDateFinish_Enter(object? sender, EventArgs e)
        {
            string existingDate = textBoxDateFinish.Text;
            if (string.IsNullOrEmpty(existingDate))
            {
                existingDate = textBoxDateStart.Text;
            }
            string newDate = EditDatePrompt.ShowDialog("Edit Finish Date:", "Edit Date", existingDate);
            if (!string.IsNullOrEmpty(newDate))
            {
                textBoxDateFinish.Text = newDate;
            }
        }

        private void TextBoxDateStart_Enter(object? sender, EventArgs e)
        {
            string existingDate = textBoxDateStart.Text;
            string newDate = EditDatePrompt.ShowDialog("Edit Start Date:", "Edit Date", existingDate);
            if (!string.IsNullOrEmpty(newDate))
            {
                textBoxDateStart.Text = newDate;
            }
        }

        private void BindData()
        {
            this.textBoxDateStart.Text = airshow.date_start;
            this.textBoxDateFinish.Text = airshow.date_finish;
            this.textBoxNameAirshow.Text = airshow.name_airshow;
            this.textBoxLocationCity.Text = airshow.location.city;
            this.textBoxLocationState.Text = airshow.location.state;
            this.textBoxNotes.Text = airshow.Notes_AirshowStuff;
            this.comboBoxStatus.SelectedItem = airshow.Status.ToString();
            this.listBoxPerformers.Items.AddRange(airshow.Performers.performer.ToArray());
            //contacts
            this.listBoxContacts.Items.AddRange(cContact.getContacts(contacts, airshow).ToArray());
            this.listBoxUndauntedNotes.Items.AddRange(airshow.UndauntedNotes.ToArray());
            this.listBoxWeblinks.Items.AddRange(airshow.AirshowLinks.ToArray());
        }


        private void ListBoxContacts_Click(object sender, EventArgs e)
        {
            if (this.listBoxContacts.SelectedItem != null)
            {
                cContact selectedContact = (cContact)this.listBoxContacts.SelectedItem;
                using (ContactEditForm contactEditForm = new ContactEditForm(selectedContact))
                {
                    if (contactEditForm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh the list box to reflect the updated contact
                        int selectedIndex = this.listBoxContacts.SelectedIndex;
                        this.listBoxContacts.Items[selectedIndex] = contactEditForm.Contact;
                    }
                }
            }
        }

        private void ButtonAddNote_Click(object sender, EventArgs e)
        {
            string newNote = UndauntedNotePrompt.ShowDialog("Enter new note:", "Add Note", "", true);
            if (!string.IsNullOrEmpty(newNote))
            {
                this.listBoxUndauntedNotes.Items.Add(newNote);
            }
        }

        private void ButtonRemoveNote_Click(object sender, EventArgs e)
        {
            if (this.listBoxUndauntedNotes.SelectedItem != null)
            {
                this.listBoxUndauntedNotes.Items.Remove(this.listBoxUndauntedNotes.SelectedItem);
            }
        }

        private void listBoxUndauntedNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUndauntedNotes.SelectedItem != null)
            {
                listBoxUndauntedNotes.SelectedIndexChanged -= listBoxUndauntedNotes_SelectedIndexChanged;
                //string newNote = AddressPrompt.ShowDialog("Enter new note:", "Add Note");
                string selectedNote = listBoxUndauntedNotes.SelectedItem.ToString();
                string editedNote = UndauntedNotePrompt.ShowDialog("Edit note:", "Edit note:", selectedNote);
                if (!string.IsNullOrEmpty(editedNote))
                {
                    int selectedIndex = listBoxUndauntedNotes.SelectedIndex;
                    listBoxUndauntedNotes.Items[selectedIndex] = editedNote;
                }
                listBoxUndauntedNotes.SelectedIndexChanged += listBoxUndauntedNotes_SelectedIndexChanged;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            airshow.date_start = this.textBoxDateStart.Text;
            airshow.date_finish = this.textBoxDateFinish.Text;
            airshow.name_airshow = this.textBoxNameAirshow.Text;
            airshow.location.city = this.textBoxLocationCity.Text;
            airshow.location.state = this.textBoxLocationState.Text;
            airshow.Notes_AirshowStuff = this.textBoxNotes.Text;
            airshow.Status = (Airshow.eStatus)Enum.Parse(typeof(Airshow.eStatus), this.comboBoxStatus.SelectedItem.ToString());
            airshow.Performers.performer = new List<string>(this.listBoxPerformers.Items.Cast<string>());
            //contacts
            //Contacts = new List<cContact>(this.listBoxContacts.Items.Cast<cContact>());
            airshow.UndauntedNotes = new List<string>(this.listBoxUndauntedNotes.Items.Cast<string>());
            airshow.AirshowLinks = new List<string>(this.listBoxWeblinks.Items.Cast<string>());

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonAddContact_Click(object sender, EventArgs e)
        {
            cContact newContact = new cContact();
            using (ContactEditForm contactEditForm = new ContactEditForm(newContact))
            {
                if (contactEditForm.ShowDialog() == DialogResult.OK)
                {
                    cContact.addContact(contacts, contactEditForm.Contact, airshow);
                    this.listBoxContacts.Items.Add(contactEditForm.Contact);
                }
            }
        }

        private void buttonRemoveContact_Click(object sender, EventArgs e)
        {
            if (listBoxContacts.SelectedItem != null)
            {
                listBoxContacts.Items.Remove(listBoxContacts.SelectedItem);
            }
        }

        private void buttonAddLink_Click(object sender, EventArgs e)
        {
            string newLink = WebLinkPrompt.ShowDialog("Enter new link:", "Add Link", "");
            if (!string.IsNullOrEmpty(newLink))
            {
                this.listBoxWeblinks.Items.Add(newLink);
            }
        }

        private void btnRemoveLink_Click(object sender, EventArgs e)
        {
            if (listBoxWeblinks.SelectedItem != null)
            {
                listBoxWeblinks.Items.Remove(listBoxWeblinks.SelectedItem);
            }
        }

        private void listBoxWeblinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxWeblinks.SelectedItem != null)
            {
                this.listBoxWeblinks.SelectedIndexChanged -= listBoxWeblinks_SelectedIndexChanged;
                string selectedLink = listBoxWeblinks.SelectedItem.ToString();
                string editedLink = WebLinkPrompt.ShowDialog("Edit link:", "Edit Link", selectedLink);
                if (!string.IsNullOrEmpty(editedLink))
                {
                    int selectedIndex = listBoxWeblinks.SelectedIndex;
                    listBoxWeblinks.Items[selectedIndex] = editedLink;
                }
                this.Enabled = true;
                this.listBoxWeblinks.SelectedIndexChanged += listBoxWeblinks_SelectedIndexChanged;
            }
        }
    }

    public static class UndauntedNotePrompt
    {
        public static string ShowDialog(string text, string caption, string existingText = "", bool prependDate = false)
        {
            // Create form
            Form addressPrompt = new Form
            {
                Width = 500,
                Height = 250, // Adjust size
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                AutoScaleDimensions = new SizeF(8F, 16F), // Adjust for your DPI
                AutoScaleMode = AutoScaleMode.Font
            };

            // Create a TableLayoutPanel for layout management
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10),
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Label
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // TextBox
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Button

            // Label
            Label textLabel = new Label
            {
                Text = text,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // TextBox
            TextBox textBox = new TextBox
            {
                Multiline = true,
                WordWrap = true,
                Dock = DockStyle.Fill
            };

            // Button
            Button confirmation = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right // Align button to the right
            };

            // Add controls to layout
            layout.Controls.Add(textLabel, 0, 0); // Row 0
            layout.Controls.Add(textBox, 0, 1); // Row 1
            layout.Controls.Add(confirmation, 0, 2); // Row 2

            // Add layout to form
            addressPrompt.Controls.Add(layout);

            // Event handlers
            confirmation.Click += (sender, e) => addressPrompt.Close();
            addressPrompt.AcceptButton = confirmation;

            // Populate text box
            textBox.Text = prependDate
                ? $"{DateTime.Now:yyyy:MM:dd} - "
                : existingText;
            textBox.SelectionStart = textBox.Text.Length;

            // Show dialog and return result
            return addressPrompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
    }


    public static class WebLinkPrompt
    {
        public static string ShowDialog(string text, string caption, string existingText = "")
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 500;
                prompt.Height = 200;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = caption;
                prompt.StartPosition = FormStartPosition.CenterScreen;

                Label textLabel = new Label() { Left = 50, Top = 20, Text = text, AutoSize = true };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400, Text = existingText };
                Button confirmation = new Button() { Text = "OK", Left = 350, Width = 100, Top = 100, DialogResult = DialogResult.OK };
                Button copyButton = new Button() { Text = "Copy", Left = 240, Width = 100, Top = 100 };

                confirmation.Click += (sender, e) => { prompt.Close(); };
                if( textBox.Text.Length > 0)
                    copyButton.Click += (sender, e) => { Clipboard.SetText(textBox.Text); };

                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(copyButton);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
            }
        }
    }

    public static class EditDatePrompt
    {
        public static string ShowDialog(string text, string caption, string existingDate = "")
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 300;
                prompt.Height = 200;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = caption;
                prompt.StartPosition = FormStartPosition.CenterScreen;

                Label textLabel = new Label() { Left = 50, Top = 20, Text = text, AutoSize = true };
                DateTimePicker dateTimePicker = new DateTimePicker() { Left = 50, Top = 50, Width = 200 };
                if (DateTime.TryParse(existingDate, out DateTime parsedDate))
                {
                    dateTimePicker.Value = parsedDate;
                }
                Button confirmation = new Button() { Text = "OK", Left = 150, Width = 100, Top = 100, DialogResult = DialogResult.OK };

                confirmation.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(dateTimePicker);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? dateTimePicker.Value.ToString("yyyy-MM-dd") : string.Empty;
            }
        }
    }
}