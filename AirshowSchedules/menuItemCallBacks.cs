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

        Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(WorkingFileParserClass);

        DialogResult dr = DialogResult.OK;
        dr = sb.showDialog();

        if (dr == DialogResult.OK)
        {
            this.Enabled = false;

            Airshow.LoadFile(WorkingFileParserClass, airshows, contacts);
            airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();
            cAirshowFileParserSetupTool.SaveMe(WorkingFileParserClass);
            this.Enabled = true;
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
            catch { }
        }
        MessageBox.Show("Something Wrong.  Be first set this up by using the \"Parse Data File Tool\"");
    }

    private void setActiveDatabaseFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        
        // System.Windows.Forms.OpenFileDialog ofdContacts = new OpenFileDialog();
        // ofdContacts.Filter = "json|*.json";
        // ofdContacts.Title = "Open the Contact Database";
        // string defaultContact = myFormState.fnContactDataBase;
        // if (defaultContact != "")
        // {
        //     ofdContacts.InitialDirectory = System.IO.Path.GetDirectoryName(defaultContact);
        // }
        // DialogResult drContacts = ofdContacts.ShowDialog();
        // if( drContacts == DialogResult.OK)
        // {
        //     myFormState.fnContactDataBase = ofdContacts.FileName;
        //     FormState.SaveMe(myFormState);
        // }
        // else
        // {
        //     MessageBox.Show("No file selected.");
        //     return;
        // }
        
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
            myFilteredAirshows = myAirshowGroup.Airshows.myShows;            
            LoadGrid(myAirshowGroup.AirshowYearOfInterest);
            ColorGrid(myFilteredAirshows);
            lblYearOfInterest.Text = $"Airshow Year of Interest: {asg.AirshowYearOfInterest.ToString()} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
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

        this.Enabled = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Airshow Group Files (*.asg.xml)|*.asg.xml";
        openFileDialog.Title = "Open the most recent Airshow List to find the most recent updates";
        openFileDialog.DefaultExt = "asg.xml";
        DialogResult dr = openFileDialog.ShowDialog();

        if (dr == DialogResult.OK)
        {
            this.Enabled = false;

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
                foreach(int contactId in ashow.contactIds)
                {
                    List<cContact> contactsToAdd = contactsRight.Where(contact => contact.ID == contactId).ToList();
                    newContacts.AddRange(contactsToAdd);
                }
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

            this.Enabled = true;
        }
    }
    private void cleanUpDBToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //check for duplicates in the database
        List<Airshow> duplicateAirshowsFound = new List<Airshow>();
        List<Airshow> airshowsFoundInNewDB = new List<Airshow>();
        List<Airshow> latestAirshowList = new List<Airshow>();

        //for every airshow in the database, compare cities to see if they show up more than once
        List<Airshow> copiedList = Airshow.DeepCopy(myAirshowGroup.Airshows.myShows);
        List<cContact> copiedContacts = cContact.DeepCopy(myContacts);
        foreach (Airshow ashow in copiedList)
        {
            List<Airshow> duplicatesFound = new List<Airshow>();
            duplicatesFound = copiedList.Where(airshow => airshow.location.Equals(ashow.location)).ToList();

            if (duplicatesFound.Count > 1)
            {
                // foreach (Airshow adup in duplicatesFound)
                // {                    
                duplicateAirshowsFound.Add(ashow);
                // }
            }
        }

        //we need an open file dialogue to open up the most recently downloaded asg.xml file
        if (duplicateAirshowsFound.Count == 0)
        {
            MessageBox.Show("No duplicates found.");
            return;
        }
        DialogResult dr = MessageBox.Show($"There are approximately {duplicateAirshowsFound.Count / 2} duplicates.\nDo you want to Open the latest downloaded asg.xml to check which one is valid?", "Open a file?", MessageBoxButtons.YesNo);
        if (dr == DialogResult.Yes)
        {
            this.Enabled = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Airshow Group Files (*.asg.xml)|*.asg.xml";
            openFileDialog.Title = "Open the most recent Airshow List to find the most recent updates";
            openFileDialog.DefaultExt = "asg.xml";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bool success;
                AirshowGroup asgLatest = AirshowGroup.LoadMe(openFileDialog.FileName, out success);
                latestAirshowList = asgLatest.Airshows.myShows.ToList();
                int count = copiedList.Count;

                foreach (Airshow ashow in duplicateAirshowsFound)
                {
                    List<Airshow> airshowsInNewDB = latestAirshowList.Where(airshow => airshow.location.Equals(ashow.location)).ToList();
                    foreach (Airshow airshowInNewDB in airshowsInNewDB)
                    {
                        if (!airshowsFoundInNewDB.Contains(airshowInNewDB))
                        {
                            //string airshowInNewDBstring = string.Concat(airshowInNewDB.ToString(), "\n");
                            airshowsFoundInNewDB.Add(airshowInNewDB);
                        }
                    }
                }

                foreach (Airshow dupShow in duplicateAirshowsFound)
                {
                    List<Airshow> validShows = airshowsFoundInNewDB.Where(airshow => airshow.location.Equals(dupShow.location)).ToList();
                    if (validShows.Count == 1)
                    {
                        
                        if (!validShows[0].IsEqual(dupShow, false))
                        {
                            List<Airshow> airshowToRemove = copiedList.Where(airshow => airshow.IsEqual(dupShow, false)).ToList();
                            
                            foreach (Airshow adup in airshowToRemove)
                            {
                                validShows[0].AppendCustomFields(adup, copiedContacts);                            
                                copiedList.Remove(adup);
                                Console.WriteLine($"Removed {adup.ToString()}".Pastel(Color.Red));
                            }
                        }
                    }
                }
                //bring the Console back to the front
                IntPtr consoleWindow = GetConsoleWindow();
                if (consoleWindow != IntPtr.Zero)
                {
                    SetForegroundWindow(consoleWindow);
                }

                Console.WriteLine();
                Console.WriteLine($"There are {copiedList.Count} airshows left in the list.".Pastel(Color.Green));
                Console.WriteLine($"There are {myAirshowGroup.Airshows.myShows.Count} airshows in the original database.".Pastel(Color.Green));
                Console.WriteLine($"There are {duplicateAirshowsFound.Count / 2} duplicates detected.".Pastel(Color.Green));
                Console.WriteLine($"There are {count - copiedList.Count} duplicates removed.".Pastel(Color.Green));
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("The easy ones are removed, now we will remove shows not in the new database:".Pastel(Color.Yellow));
                Console.WriteLine("Press any key to continue.".Pastel(Color.Green));
                Console.ReadKey();
                Console.WriteLine();


                count = copiedList.Count;
                List<Airshow> showsToRemove = new List<Airshow>();
                foreach (Airshow ashow in copiedList)
                {
                    List<Airshow> showsFound = latestAirshowList.Where(airshow => airshow.IsEqual(ashow, false)).ToList();
                    if (showsFound.Count == 0)
                    {
                        Console.WriteLine($"Airshow {ashow.ToString()} is not in the latest database. Do you want to remove it (Y/N)?".Pastel(Color.Yellow));
                        string response = Console.ReadLine();
                        if (response.ToLower() == "y")
                        {
                            
                            showsToRemove.Add(ashow);
                        }
                    }
                }
                foreach (Airshow airshow in showsToRemove)
                {                    
                    cContact.RemoveAirshowReference(copiedContacts, airshow);
                    copiedList.Remove(airshow);
                }


                Console.WriteLine();
                Console.WriteLine($"There are {copiedList.Count} airshows left in the list.".Pastel(Color.Green));
                Console.WriteLine($"There are {myAirshowGroup.Airshows.myShows.Count} airshows in the original database.".Pastel(Color.Green));
                Console.WriteLine($"We removed {count - copiedList.Count} non-existant Airshows.".Pastel(Color.Green));
                Console.WriteLine();
                Console.WriteLine("Do you want to make this permenant? (Y/N)".Pastel(Color.Yellow));
                string response2 = Console.ReadLine();
                if (response2.ToLower() == "y")
                {
                    Console.WriteLine("This change is permenant, are you sure?! (Y/N)".Pastel(Color.Red));
                    string confirm = Console.ReadLine();
                    if (confirm.ToLower() == "y")
                    {
                        myAirshowGroup.Airshows.myShows = copiedList;
                        myContacts = copiedContacts;
                        SaveAirshowSchedule(false);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Looking for Airshows that have been cancelled.".Pastel(Color.Green));
                List<Airshow> cancelledShows = latestAirshowList.Where(airshow => airshow.name_airshow.ToLower().Contains("cancelled")).ToList();
                List<Airshow> cancelledInDB = new   List<Airshow>();
                List<Airshow> airshowToRemove2 = new List<Airshow>();
                foreach (Airshow ashow in cancelledShows)
                {
                    Console.WriteLine(ashow.ToString().Pastel(Color.Green));
                    List<Airshow> mathingShow = copiedList.Where(airshow => airshow.location.Equals(ashow.location)).ToList();
                    foreach (Airshow adup in mathingShow)
                    {
                        cancelledInDB.Add(adup);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Do these shows match? (Y/N)?".Pastel(Color.Yellow));
                foreach (Airshow ashow in cancelledInDB)
                {
                    List<Airshow> matchingShow = cancelledShows.Where(airshow => airshow.location.Equals(ashow.location)).ToList();
                    
                    foreach(Airshow adup in matchingShow)
                    {
                        Console.WriteLine(adup.ToString().Pastel(Color.Green));
                        Console.WriteLine(ashow.ToString().Pastel(Color.Green));
                        Console.WriteLine("Do these match? (Y/N)".Pastel(Color.Yellow));
                        string response = Console.ReadLine();
                        if (response.ToLower() == "y")
                        {
                            airshowToRemove2.Add(ashow);
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Do you want to remove these shows?".Pastel(Color.Yellow));
                Console.WriteLine();
                foreach(Airshow ashow in airshowToRemove2)
                {
                    Console.WriteLine(ashow.ToString().Pastel(Color.Red));
                }
                Console.WriteLine("(Y to remove, N to skip)?".Pastel(Color.Yellow));
                string response3 = Console.ReadLine();
                if (response3.ToLower() == "y")
                {
                    foreach (Airshow ashow in airshowToRemove2)
                    {
                        cContact.RemoveAirshowReference(copiedContacts, ashow);
                        copiedList.Remove(ashow);

                    }
                    myContacts = copiedContacts;
                    myAirshowGroup.Airshows.myShows = copiedList;
                    SaveAirshowSchedule(false);
                    Console.WriteLine("Shows removed.".Pastel(Color.Green));
                }
                else
                {
                    Console.WriteLine("Shows not removed.".Pastel(Color.Green));
                }
            }
            this.Enabled = true;
        }
        else
        {
            this.Enabled = false;
            Console.WriteLine("Here are the duplicates you need to fix:".Pastel(Color.Green));
            foreach (Airshow ashow in duplicateAirshowsFound)
            {
                Console.WriteLine(ashow.ToString().Pastel(Color.Yellow));
            }
            Console.WriteLine();

            // Open the DeleteAirshowForm
            int Count = copiedList.Count;
            DeleteAirshowForm deleteForm = new DeleteAirshowForm(duplicateAirshowsFound, copiedList, copiedContacts);
            if (deleteForm.ShowDialog() == DialogResult.OK)
            {
                // ask the user if they want to save the merged data
                DialogResult dr2 = MessageBox.Show("Do you want to save the merged data to the active DB?", "Save Merged Data?", MessageBoxButtons.YesNo);
                if (dr2 == DialogResult.Yes)
                {
                    myContacts = copiedContacts;
                    myAirshowGroup.Airshows.myShows = copiedList;
                    myFilteredAirshows = myAirshowGroup.Airshows.myShows;
                    SaveAirshowSchedule(false);
                    Console.WriteLine();
                    Console.WriteLine($"There were {Count - copiedList.Count} airshows removed.".Pastel(Color.Green));
                    Console.WriteLine();
                }
            }
            this.Enabled = true;
        }
    }
    private void checkForCancelledShowsToolStripMenuItem_Click(object sender, EventArgs e)
    {

        List<Airshow> copiedList = Airshow.DeepCopy(myAirshowGroup.Airshows.myShows);
        List<cContact> copiedContacts = cContact.DeepCopy(myContacts);
        List<Airshow> cancelledShows = copiedList.Where(airshow => airshow.name_airshow.ToLower().Contains("cancelled")).ToList();
        List<Airshow> cancelledInNote = copiedList.Where(airshow => airshow.Notes_AirshowStuff.ToLower().Contains("canc")).ToList();
        foreach (Airshow ashow in cancelledInNote)
        {
            if (!cancelledShows.Contains(ashow))
            {
                cancelledShows.Add(ashow);
            }
        }
        if (cancelledShows.Count > 0)
        {
            this.Enabled = false;
            //bring the Console back to the front
            IntPtr consoleWindow = GetConsoleWindow();
            if (consoleWindow != IntPtr.Zero)
            {
                SetForegroundWindow(consoleWindow);
            }
            Console.WriteLine("Here are the cancelled shows:".Pastel(Color.Green));
            foreach (Airshow ashow in cancelledShows)
            {
                Console.WriteLine(ashow.ToString().Pastel(Color.Yellow));
            }
            Console.WriteLine();
            Console.WriteLine("Do you want to remove these shows (Y/N)?".Pastel(Color.Yellow));
            string response3 = Console.ReadLine();
            int Count = copiedList.Count;
            if (response3.ToLower() == "y")
            {
                foreach (Airshow ashow in cancelledShows)
                {
                    cContact.RemoveAirshowReference(copiedContacts, ashow);
                    copiedList.Remove(ashow);
                }
                myAirshowGroup.Airshows.myShows = copiedList;
                SaveAirshowSchedule(false);

                    Console.WriteLine();
                    Console.WriteLine($"There were {Count - copiedList.Count} airshows removed.".Pastel(Color.Green));
                    Console.WriteLine();
            }
            Console.WriteLine("operation aborted".Pastel(Color.Green));
            this.Enabled = true;
        }
        else
        {
            MessageBox.Show("No cancelled shows found.");
        }
    }

    private void updateAdditionalFieldsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<Airshow> copiedASGlist = Airshow.DeepCopy(myAirshowGroup.Airshows.myShows);
        List<cContact> copiedContactList = cContact.DeepCopy(myContacts);
        List<Airshow> latestAirshowList = new List<Airshow>();

        {
            this.Enabled = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Airshow Group Files (*.asg.xml)|*.asg.xml";
            openFileDialog.Title = "Open the most recent Airshow List to find the most recent updates";
            openFileDialog.DefaultExt = "asg.xml";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                IntPtr consoleWindow = GetConsoleWindow();
                bool success;
                AirshowGroup asgLatest = AirshowGroup.LoadMe(openFileDialog.FileName, out success);
                List<cContact> latestContacts = cContact.LoadMe(openFileDialog.FileName.Replace(".asg.xml", ".contacts.json"), out success);
                if (!success)
                {
                    MessageBox.Show("Failed to load the selected file.");
                    this.Enabled = true;
                    return;
                }

                latestAirshowList = asgLatest.Airshows.myShows.ToList();

                foreach (Airshow ashow in copiedASGlist)
                {
                    List<Airshow> showsFound = latestAirshowList.Where(airshow => airshow.IsEqual(ashow, false)).ToList();
                    if (showsFound.Count > 0)
                    {
                        Console.WriteLine($"Airshow {ashow.ToString()} has a change, looking for items to add?".Pastel(Color.Yellow));
                        if (showsFound.Count == 1)
                        {
                            cContact newContact = showsFound[0].my
                            showsFound[0].mergeAdditionalInformation(ashow, showsFound[0]);
                        }
                        else
                        {
                            foreach (Airshow adup in showsFound)
                            {
                                Console.WriteLine("Do these shows match? (Y/N)".Pastel(Color.Yellow));
                                Console.WriteLine(adup.ToString().Pastel(Color.Yellow));
                                Console.WriteLine(ashow.ToString().Pastel(Color.Yellow));

                                Console.WriteLine("Do you want to append the custom fields to this pair? (Y/N)".Pastel(Color.Yellow));
                                string response = Console.ReadLine();
                                if (response.ToLower() == "y")
                                {
                                    showsFound[0].mergeAdditionalInformation(ashow, adup);
                                    break;
                                }
                            }
                        }
                    }
                }
                myAirshowGroup.Airshows.myShows = copiedASGlist;
                SaveAirshowSchedule(false);
                MessageBox.Show("Additional fields updated successfully.");
            }
            else
            {
                MessageBox.Show("No file selected.");
            }

            this.Enabled = true;
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
        Console.WriteLine($"Active DB file archived to: {archivedFilePath}".Pastel(Color.Green));
    }    
}


