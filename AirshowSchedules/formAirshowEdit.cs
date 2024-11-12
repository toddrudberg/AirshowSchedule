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
        private Button buttonEditNote;
        private Button buttonOK;
        private Button buttonCancel;
        private Label label1;
        private Airshow airshow;

        public AirshowEditForm(Airshow airshow)
        {
            this.airshow = airshow;
            InitializeComponent();
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
            buttonEditNote = new Button(); // New Edit Note button
            buttonOK = new Button();
            buttonCancel = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // textBoxDateStart
            // 
            textBoxDateStart.Location = new Point(10, 10);
            textBoxDateStart.Name = "textBoxDateStart";
            textBoxDateStart.PlaceholderText = "Start Date (yyyy-MM-DD)";
            textBoxDateStart.Size = new Size(200, 23);
            textBoxDateStart.TabIndex = 0;
            // 
            // textBoxDateFinish
            // 
            textBoxDateFinish.Location = new Point(10, 40);
            textBoxDateFinish.Name = "textBoxDateFinish";
            textBoxDateFinish.PlaceholderText = "Finish Date (yyyy-MM-DD)";
            textBoxDateFinish.Size = new Size(200, 23);
            textBoxDateFinish.TabIndex = 1;
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
            listBoxPerformers.Location = new Point(220, 10);
            listBoxPerformers.Name = "listBoxPerformers";
            listBoxPerformers.Size = new Size(200, 94);
            listBoxPerformers.TabIndex = 7;
            // 
            // listBoxContacts
            // 
            listBoxContacts.ItemHeight = 15;
            listBoxContacts.Location = new Point(220, 120);
            listBoxContacts.Name = "listBoxContacts";
            listBoxContacts.Size = new Size(200, 94);
            listBoxContacts.TabIndex = 8;
            listBoxContacts.Click += ListBoxContacts_Click;
            // 
            // listBoxUndauntedNotes
            // 
            listBoxUndauntedNotes.ItemHeight = 15;
            listBoxUndauntedNotes.Location = new Point(10, 274);
            listBoxUndauntedNotes.Name = "listBoxUndauntedNotes";
            listBoxUndauntedNotes.Size = new Size(200, 94);
            listBoxUndauntedNotes.TabIndex = 9;
            // 
            // buttonAddNote
            // 
            buttonAddNote.Location = new Point(220, 274);
            buttonAddNote.Name = "buttonAddNote";
            buttonAddNote.Size = new Size(75, 23);
            buttonAddNote.TabIndex = 10;
            buttonAddNote.Text = "Add Note";
            buttonAddNote.Click += ButtonAddNote_Click;
            // 
            // buttonRemoveNote
            // 
            buttonRemoveNote.Location = new Point(220, 304);
            buttonRemoveNote.Name = "buttonRemoveNote";
            buttonRemoveNote.Size = new Size(75, 23);
            buttonRemoveNote.TabIndex = 11;
            buttonRemoveNote.Text = "Remove Note";
            buttonRemoveNote.Click += ButtonRemoveNote_Click;
            // 
            // buttonEditNote
            // 
            buttonEditNote.Location = new Point(220, 334);
            buttonEditNote.Name = "buttonEditNote";
            buttonEditNote.Size = new Size(75, 23);
            buttonEditNote.TabIndex = 12;
            buttonEditNote.Text = "Edit Note";
            buttonEditNote.Click += ButtonEditNote_Click;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(10, 384);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 13;
            buttonOK.Text = "OK";
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(100, 384);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 14;
            buttonCancel.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 256);
            label1.Name = "label1";
            label1.Size = new Size(103, 15);
            label1.TabIndex = 15;
            label1.Text = "Undaunted Notes:";
            // 
            // AirshowEditForm
            // 
            ClientSize = new Size(450, 421);
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
            Controls.Add(buttonEditNote); // Add the Edit Note button to the form
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            Name = "AirshowEditForm";
            Text = "Edit Airshow";
            ResumeLayout(false);
            PerformLayout();
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
            this.listBoxContacts.Items.AddRange(airshow.Contacts.contact.ToArray());
            this.listBoxUndauntedNotes.Items.AddRange(airshow.UndauntedNotes.ToArray());
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

        private void ButtonEditNote_Click(object sender, EventArgs e)
        {
            if (listBoxUndauntedNotes.SelectedItem != null)
            {
                //string newNote = AddressPrompt.ShowDialog("Enter new note:", "Add Note");
                string selectedNote = listBoxUndauntedNotes.SelectedItem.ToString();
                string editedNote = UndauntedNotePrompt.ShowDialog("Edit note:", "Edit note:", selectedNote);
                if (!string.IsNullOrEmpty(editedNote))
                {
                    int selectedIndex = listBoxUndauntedNotes.SelectedIndex;
                    listBoxUndauntedNotes.Items[selectedIndex] = editedNote;
                }
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
            airshow.Contacts.contact = new List<Airshow.cContact>(this.listBoxContacts.Items.Cast<Airshow.cContact>());
            airshow.UndauntedNotes = new List<string>(this.listBoxUndauntedNotes.Items.Cast<string>());

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }

    public static class UndauntedNotePrompt
    {
        public static string ShowDialog(string text, string caption, string existingText = "", bool newNote = false)
        {
            Form addressPrompt = new Form()
            {
                Width = 500,
                Height = 250, // Increased height
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox()
            {
                Left = 50,
                Top = 50,
                Width = 400,
                Height = 100, // Increased height
                Multiline = true,
                WordWrap = true // Enable text wrapping
            };
            Button confirmation = new Button() { Text = "OK", Left = 350, Width = 100, Top = 160, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { addressPrompt.Close(); };
            addressPrompt.Controls.Add(textBox);
            addressPrompt.Controls.Add(confirmation);
            addressPrompt.Controls.Add(textLabel);
            addressPrompt.AcceptButton = confirmation;
            if (newNote)
            {
                textBox.Text = $"{DateTime.Now:yyyy:MM:dd} - ";
            }
            else
            {
                textBox.Text = existingText;
            }
            textBox.SelectionStart = textBox.Text.Length;
    
            return addressPrompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
    }
}