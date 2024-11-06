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

        private Airshow airshow;

        public AirshowEditForm(Airshow airshow)
        {
            this.airshow = airshow;
            InitializeComponent();
            BindData();
        }

        private void InitializeComponent()
        {
            this.textBoxDateStart = new TextBox();
            this.textBoxDateFinish = new TextBox();
            this.textBoxNameAirshow = new TextBox();
            this.textBoxLocationCity = new TextBox();
            this.textBoxLocationState = new TextBox();
            this.textBoxNotes = new TextBox();
            this.comboBoxStatus = new ComboBox();
            this.listBoxPerformers = new ListBox();
            this.listBoxContacts = new ListBox();
            this.listBoxUndauntedNotes = new ListBox();
            this.buttonAddNote = new Button();
            this.buttonRemoveNote = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();

            // TextBox for Date Start
            this.textBoxDateStart.Location = new Point(10, 10);
            this.textBoxDateStart.Width = 200;
            this.textBoxDateStart.PlaceholderText = "Start Date (yyyy-MM-DD)";

            // TextBox for Date Finish
            this.textBoxDateFinish.Location = new Point(10, 40);
            this.textBoxDateFinish.Width = 200;
            this.textBoxDateFinish.PlaceholderText = "Finish Date (yyyy-MM-DD)";

            // TextBox for Name Airshow
            this.textBoxNameAirshow.Location = new Point(10, 70);
            this.textBoxNameAirshow.Width = 200;
            this.textBoxNameAirshow.PlaceholderText = "Name of Airshow";

            // TextBox for Location City
            this.textBoxLocationCity.Location = new Point(10, 100);
            this.textBoxLocationCity.Width = 200;
            this.textBoxLocationCity.PlaceholderText = "City";

            // TextBox for Location State
            this.textBoxLocationState.Location = new Point(10, 130);
            this.textBoxLocationState.Width = 200;
            this.textBoxLocationState.PlaceholderText = "State";

            // TextBox for Notes
            this.textBoxNotes.Location = new Point(10, 160);
            this.textBoxNotes.Width = 200;
            this.textBoxNotes.Height = 60;
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.PlaceholderText = "Notes";

            // ComboBox for Status
            this.comboBoxStatus.Location = new Point(10, 230);
            this.comboBoxStatus.Width = 200;
            this.comboBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStatus.Items.AddRange(Enum.GetNames(typeof(Airshow.eStatus)));

            // ListBox for Performers
            this.listBoxPerformers.Location = new Point(220, 10);
            this.listBoxPerformers.Width = 200;
            this.listBoxPerformers.Height = 100;

            // ListBox for Contacts
            this.listBoxContacts.Location = new Point(220, 120);
            this.listBoxContacts.Width = 200;
            this.listBoxContacts.Height = 100;
            this.listBoxContacts.Click += new EventHandler(this.ListBoxContacts_Click);

            // ListBox for Undaunted Notes
            this.listBoxUndauntedNotes.Location = new Point(10, 270);
            this.listBoxUndauntedNotes.Width = 200;
            this.listBoxUndauntedNotes.Height = 100;

            // Button to Add Note
            this.buttonAddNote.Text = "Add Note";
            this.buttonAddNote.Location = new Point(220, 270);
            this.buttonAddNote.Click += new EventHandler(this.ButtonAddNote_Click);

            // Button to Remove Note
            this.buttonRemoveNote.Text = "Remove Note";
            this.buttonRemoveNote.Location = new Point(220, 300);
            this.buttonRemoveNote.Click += new EventHandler(this.ButtonRemoveNote_Click);

            // OK Button
            this.buttonOK.Text = "OK";
            this.buttonOK.Location = new Point(10, 380);
            this.buttonOK.Click += new EventHandler(this.ButtonOK_Click);

            // Cancel Button
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Location = new Point(100, 380);
            this.buttonCancel.Click += new EventHandler(this.ButtonCancel_Click);

            // Form
            this.ClientSize = new Size(450, 420);
            this.Controls.Add(this.textBoxDateStart);
            this.Controls.Add(this.textBoxDateFinish);
            this.Controls.Add(this.textBoxNameAirshow);
            this.Controls.Add(this.textBoxLocationCity);
            this.Controls.Add(this.textBoxLocationState);
            this.Controls.Add(this.textBoxNotes);
            this.Controls.Add(this.comboBoxStatus);
            this.Controls.Add(this.listBoxPerformers);
            this.Controls.Add(this.listBoxContacts);
            this.Controls.Add(this.listBoxUndauntedNotes);
            this.Controls.Add(this.buttonAddNote);
            this.Controls.Add(this.buttonRemoveNote);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Text = "Edit Airshow";
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
            string newNote = Prompt.ShowDialog("Enter new note:", "Add Note");
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

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
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