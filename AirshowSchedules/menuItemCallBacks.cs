using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirshowSchedules;

public partial class formMain
{

    private void fileParseDataFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<Airshow> airshows = new List<Airshow>();
        List<cContact> contacts = new List<cContact>();

        //WorkingFileParserClass;
        WorkingFileParserClass = cAirshowFileParserSetupTool.LoadMe();

        using var setup = new FormImportSetup(WorkingFileParserClass);
        DialogResult dr = setup.ShowDialog(this);

        if (dr == DialogResult.OK)
        {
            this.Enabled = false;

            try
            {
                Airshow.LoadFile(WorkingFileParserClass, airshows, contacts);
                cAirshowFileParserSetupTool.SaveMe(WorkingFileParserClass);
                MessageBox.Show($"Imported {airshows.Count} shows and {contacts.Count} contacts for {WorkingFileParserClass.AirshowYear}.\nUse Save Parsed Data File to save a separate import for comparison.", "ICAS import");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Import failed");
            }
            finally { this.Enabled = true; }
        }
    }
    private void fileSaveParsedDataFile_Click(object sender, EventArgs e)
    {
        AirshowGroup asg = new AirshowGroup();
        List<Airshow> airshows = new List<Airshow>();
        List<cContact> contacts = new List<cContact>();

        if (System.IO.File.Exists(WorkingFileParserClass.sFileName))
        {
            try
            {
                Airshow.LoadFile(WorkingFileParserClass, airshows, contacts);
                airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();
                asg.Airshows.myShows = airshows;
                asg.AirshowYearOfInterest = WorkingFileParserClass.AirshowYear;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "*.asg.XML|*.asg.xml";
                sfd.Title = "Save an Airshow Group";
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Electroimpact.XmlSerialization.Serializer.Save(asg, sfd.FileName);
                    cContact.SaveMe(contacts, sfd.FileName.Replace(".asg.xml", ".contacts.json"));
                    Console.WriteLine($"Airshow Group saved to {sfd.FileName}".Pastel(Color.Green));
                    Console.WriteLine($"Contacts saved to {sfd.FileName.Replace(".asg.xml", ".contacts.json")}".Pastel(Color.Green));
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to save import");
                return;
            }
        }
        MessageBox.Show("Something Wrong.  Be first set this up by using the \"Parse Data File Tool\"");
    }

    private void setActiveDatabaseFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        
        System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "XML|*.xml";
        ofd.Title = "Open the Airshow Database";
        string defaultFile = myFormState.fnCurrentXMLDataBase;
        if (defaultFile != "")
        {
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(defaultFile);
        }
        DialogResult dr = ofd.ShowDialog();

        if (dr == DialogResult.OK)
        {
            this.Enabled = false;
            bool success;
            AirshowGroup asg = AirshowGroup.LoadMe(ofd.FileName, out success);
            if (!success) { return; }
            myAirshowGroup = asg;

            myFormState.fnCurrentXMLDataBase = ofd.FileName;
            FormState.SaveMe(myFormState);
            myFilteredAirshows = myAirshowGroup.GetAirshowsForYear();            
            LoadGrid(myAirshowGroup.AirshowYearOfInterest);
            ColorGrid(myFilteredAirshows);
            linkLabelDB.Text = $"Airshow Year of Interest: {asg.AirshowYearOfInterest.ToString()} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
            linkLabelContacts.Text = $"Contacts: {myFormState.fnContactDataBase}";
            this.Enabled = true;
        }
    }

    private void saveContactFileAs(object sender, EventArgs e)
    {
        SaveContacts(true);
    }

    private void saveDatabaseFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SaveAirshowSchedule(true);
    }

    private void showHelpFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ReadmeForm readmeForm = new ReadmeForm();
        readmeForm.Show();
    }

    private void compareToActiveDBToolStripMenuItem_Click(object sender, EventArgs e)
    {

        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Airshow Group Files (*.asg.xml)|*.asg.xml";
        openFileDialog.Title = "Open the most recent Airshow List to find the most recent updates";
        openFileDialog.DefaultExt = "asg.xml";
        DialogResult dr = openFileDialog.ShowDialog();

        if (dr == DialogResult.OK)
        {
            AirshowGroup asgLeft = myAirshowGroup;

            AirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(openFileDialog.FileName);
            bool success;
            List<cContact> contactsRight = cContact.LoadMe(openFileDialog.FileName.Replace(".asg.xml", ".contacts.json"), out success);

            List<Airshow> newShows = new List<Airshow>();
            List<cContact> newContacts = new List<cContact>();

            foreach (Airshow ashow in asgRight.Airshows.myShows)
            {
                List<Airshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.IsEqual(ashow, false)).ToList();
                if (foundAirshows.Count > 0)
                    continue;
                newShows.Add(ashow);
                // get the contacts for this airshow
                List<cContact> contactsToAdd = cContact.getContacts(contactsRight, ashow);
                Console.WriteLine($"{ashow.name_airshow} has {contactsToAdd.Count}");

                newContacts.AddRange(contactsToAdd);
            }

            // Show the CompareForm
            // create a deep copy of MyAirshows
            List<Airshow> copiedList = Airshow.DeepCopy(myAirshowGroup.Airshows.myShows);
            List<cContact> copiedContacts = cContact.DeepCopy(myContacts);
            CompareForm compareForm = new CompareForm(newShows, newContacts, copiedList, copiedContacts, this);
            if (compareForm.ShowDialog() == DialogResult.OK)
            {
                // ask the user if they want to save the merged data
                DialogResult dr2 = MessageBox.Show("Do you want to save the merged data to the active DB?", "Save Merged Data?", MessageBoxButtons.YesNo);
                if (dr2 == DialogResult.Yes)
                {
                    myAirshowGroup.Airshows.myShows = copiedList;
                    myContacts = copiedContacts;
                    SaveAirshowSchedule(false);
                }
            }
        }
    }
    private void arciveActiveDBToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string actvDBFile = myFormState.fnCurrentXMLDataBase;
    
        // Extract the filename from actvDBFile
        string fileName = System.IO.Path.GetFileName(actvDBFile);
    
        // Get the current date and time
        DateTime now = DateTime.Now;
    
        // Format the date and time as a string
        string dateTimeString = now.ToString("yyyy-MM-dd_HH-mm-ss");
    
        // Prepend the formatted date and time to the filename
        string archivedFileName = $"{dateTimeString}_{fileName}";
    
        // Use the archivedFileName as needed
        Console.WriteLine($"Archived file name: {archivedFileName}");

        string directory = System.IO.Path.GetDirectoryName(actvDBFile);
        
        //add archive folder
        directory = System.IO.Path.Combine(directory, "Archive");
        
        //let's see if the directory exists
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }
        // Combine the directory and new filename

        string archivedFilePath = System.IO.Path.Combine(directory, archivedFileName);

        // Use the archivedFilePath as needed
        Console.WriteLine($"Archived file path: {archivedFilePath}");

        Electroimpact.XmlSerialization.Serializer.Save(myAirshowGroup, archivedFilePath);
        cContact.SaveMe(myContacts, archivedFilePath.Replace(".asg.xml", ".contacts.json"));
        Console.WriteLine($"Active DB file archived to: {archivedFilePath}".Pastel(Color.Green));
    }    
}


