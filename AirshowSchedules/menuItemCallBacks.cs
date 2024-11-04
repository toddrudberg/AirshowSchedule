using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirshowSchedules;

public partial class frmAirshowScheduleTool
{

    private void fileParseDataFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //myAirshows.Clear();

        List<Airshow> airshows = new List<Airshow>();

        //WorkingFileParserClass;
        WorkingFileParserClass = cAirshowFileParserSetupTool.LoadMe();

        Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(WorkingFileParserClass);

        DialogResult dr = DialogResult.OK;
        dr = sb.showDialog();

        if (dr == DialogResult.OK)
        {
            this.Enabled = false;

            Airshow.LoadFile(WorkingFileParserClass, airshows);
            //myFormState.AirshowYearofInterest = WorkingFileParserClass.AirshowYear;
            airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();

            myFilteredAirshows = airshows.ToList();
            cAirshowFileParserSetupTool.SaveMe(WorkingFileParserClass);
            this.Enabled = true;
        }
    }
    private void fileSetRegionFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder sb = new Electroimpact.SettingsFormBuilderV2.SettingsFormBuilder(myFormState);

        DialogResult dr = DialogResult.OK;
        dr = sb.showDialog();

        if (dr == DialogResult.OK)
        {
            FormState.SaveMe(myFormState);
            myRegions = Regions.LoadMe(myFormState.fnRegions);
        }
    }

    private void fileSaveParsedDataFile_Click(object sender, EventArgs e)
    {
        AirshowGroup asg = new AirshowGroup();
        List<Airshow> airshows = new List<Airshow>();

        if (System.IO.File.Exists(WorkingFileParserClass.sFileName))
        {
            try
            {
                Airshow.LoadFile(WorkingFileParserClass, airshows);
                airshows = airshows.OrderBy(airshow => airshow.WeekNumber).ToList();

                //myFilteredAirshows = airshows.ToList();
                asg.Airshows.myShows = airshows;
                asg.AirshowYearOfInterest = WorkingFileParserClass.AirshowYear;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "*.asg.XML|*.asg.xml";
                sfd.Title = "Save an Airshow Group";
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Electroimpact.XmlSerialization.Serializer.Save(asg, sfd.FileName);
                }
                return;
            }
            catch { }
        }
        MessageBox.Show("Something Wrong.  Be first set this up by using the \"Parse Data File Tool\"");
    }

    private void setActiveDatabaseFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        myAirshows.Clear();

        System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "XML|*.xml";
        DialogResult dr = ofd.ShowDialog();

        if (dr == DialogResult.OK)
        {
            this.Enabled = false;
            bool success;
            AirshowGroup asg = AirshowGroup.LoadMe(ofd.FileName, out success);
            if (!success) { return; }
            myAirshows = asg.Airshows.myShows;
            myFormState.AirshowYearofInterest = asg.AirshowYearOfInterest;
            myFormState.fnCurrentXMLDataBase = ofd.FileName;
            FormState.SaveMe(myFormState);

            myFilteredAirshows = myAirshows.ToList();
            this.Enabled = true;
            //LoadGrid(myFormState.AirshowYearofInterest);
            ColorGrid(myFilteredAirshows);
            lblYearOfInterest.Text = $"Airshow Year of Interest: {asg.AirshowYearOfInterest.ToString()} - ActiveDB: {myFormState.fnCurrentXMLDataBase}";
        }
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

        DialogResult dr = SelectFilesToCompare(myFormState.fnCurrentXMLDataBase);

        if (dr == DialogResult.OK)
        {
            this.Enabled = false;

            AirshowGroup asgLeft = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameLeft);
            myFilteredAirshows = asgLeft.Airshows.myShows.ToList();

            AirshowGroup asgRight = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(myASGCompare.sFileNameRight);

            List<Airshow> newShows = new List<Airshow>();

            foreach (Airshow ashow in asgRight.Airshows.myShows)
            {
                List<Airshow> foundAirshows = asgLeft.Airshows.myShows.Where(airshow => airshow.IsEqual(ashow, false)).ToList();
                if (foundAirshows.Count > 0)
                    continue;
                newShows.Add(ashow);
            }
            myMergedShows.Clear();
            myMergedShows = newShows;

            // Show the CompareForm
            // create a deep copy of MyAirshows
            List<Airshow> copiedList = Airshow.DeepCopy(myAirshows);
            CompareForm compareForm = new CompareForm(newShows, copiedList, this);
            if (compareForm.ShowDialog() == DialogResult.OK)
            {
                // ask the user if they want to save the merged data
                DialogResult dr2 = MessageBox.Show("Do you want to save the merged data to the active DB?", "Save Merged Data?", MessageBoxButtons.YesNo);
                if (dr2 == DialogResult.Yes)
                {
                    myAirshows = copiedList;
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
        List<Airshow> copiedList = Airshow.DeepCopy(myAirshows);
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
        DialogResult dr = MessageBox.Show($"There are {duplicateAirshowsFound.Count / 2} duplicates.\nDo you want to Open the latest downloaded asg.xml to check which one is valid?", "Open a file?", MessageBoxButtons.YesNo);
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
                    foreach (Airshow adup in airshowsInNewDB)
                    {
                        if (!airshowsFoundInNewDB.Contains(adup))
                        {
                            string airshowInNewDB = string.Concat(adup.ToString(), "\n");
                            airshowsFoundInNewDB.Add(adup);
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
                                validShows[0].AppendCustomFields(adup);
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
                Console.WriteLine($"There are {myAirshows.Count} airshows in the original database.".Pastel(Color.Green));
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
                    copiedList.Remove(airshow);
                }


                Console.WriteLine();
                Console.WriteLine($"There are {copiedList.Count} airshows left in the list.".Pastel(Color.Green));
                Console.WriteLine($"There are {myAirshows.Count} airshows in the original database.".Pastel(Color.Green));
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
                        myAirshows = copiedList;
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
                        copiedList.Remove(ashow);

                    }
                    myAirshows = copiedList;
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
            DeleteAirshowForm deleteForm = new DeleteAirshowForm(duplicateAirshowsFound, copiedList);
            if (deleteForm.ShowDialog() == DialogResult.OK)
            {
                // ask the user if they want to save the merged data
                DialogResult dr2 = MessageBox.Show("Do you want to save the merged data to the active DB?", "Save Merged Data?", MessageBoxButtons.YesNo);
                if (dr2 == DialogResult.Yes)
                {
                    myAirshows = copiedList;
                    myFilteredAirshows = myAirshows;
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

        List<Airshow> copiedList = Airshow.DeepCopy(myAirshows);
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
                    copiedList.Remove(ashow);
                }
                myAirshows = copiedList;
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

        bool success;
        AirshowGroup asg = AirshowGroup.LoadMe(myFormState.fnCurrentXMLDataBase, out success);
        if(!success) 
        { 
            Console.WriteLine("Failed to load the active database file.".Pastel(Color.Red));
            Console.WriteLine($"Active DB File: {actvDBFile}".Pastel(Color.Red));
            Console.WriteLine();
            return; 
        }

        asg.Airshows.myShows = myAirshows;

        Electroimpact.XmlSerialization.Serializer.Save(asg, archivedFilePath);
        Console.WriteLine($"Active DB file archived to: {archivedFilePath}".Pastel(Color.Green));
    }    
}


