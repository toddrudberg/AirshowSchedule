﻿using Electroimpact;
using Electroimpact.SettingsFormBuilderV2.Attributes;
using Newtonsoft.Json;
using Pastel;
using System.Globalization;
using System.Xml.Serialization;
using static AirshowSchedules.formMain;

namespace AirshowSchedules
{
    public class Airshow
    {
        public int ID;
        [Display(DisplayName = "Start Date (yyyy-MM-DD):")]
        public string date_start { get; set; }

        [Display(DisplayName = "Finish Date (yyyy-MM-DD):")]
        public string date_finish { get; set; }

        [Display(DisplayName = "Date Added (yyyy-MM-DD):")]
        public string date_added { get; set; }

        [Display(DisplayName = "Airshow Name:")]
        public string name_airshow { get; set; }

        [Display(DisplayName = "Location:")]
        public cLocation location { get; set; } = new cLocation();

        [Display(DisplayName = "Performers:")]
        public cPerformers Performers { get; set; } = new cPerformers();

        [Display(DisplayName = "Notes:")]
        public string Notes_AirshowStuff { get; set; } = "";

        [Display(DisplayName = "Undaunted Notes:")]
        public List<string> UndauntedNotes { get; set; } = new List<string>();

        [Display(DisplayName = "Links To Show:")]
        public List<string> AirshowLinks { get; set; } = new List<string>();

        public enum eStatus
        {
            contract = 0,
            verbal = 1,
            maybe = 2,
            pursue = 3,
            none = 4,
            NO = 5
        };

        [Display(DisplayName = "Status:")]
        public eStatus Status { get; set; } = eStatus.none;
        
        public void mergeAdditionalInformation(Airshow masterShow, Airshow newShow, List<cContact> copiedContacts, List<cContact> latestContacts)
        {
            List<cContact> masterContacts = cContact.getContacts(copiedContacts, masterShow);
            Console.WriteLine($"Master: {masterShow.name_airshow} ID {masterShow.ID} has {masterContacts.Count} contacts.".Pastel(Color.LimeGreen));
            foreach (cContact contact in masterContacts)
            {
                Console.WriteLine($"{contact.name} with ID: {contact.ID}");
            }

            List<cContact> newContacts = cContact.getContacts(latestContacts, newShow);
            Console.WriteLine($"New: {newShow.name_airshow} ID {newShow.ID} has {newContacts.Count} contacts.".Pastel(Color.LimeGreen));
            foreach (cContact contact in newContacts)
            {
                Console.WriteLine($"{contact.name} with ID: {contact.ID}");
            }

            if(newContacts.Count > 1)
            {
                MessageBox.Show("Something Wrong.".Pastel(Color.Green));
                List<cContact> test = cContact.getContacts(latestContacts, newShow);
            }

            foreach (cContact newContact in newContacts)
            {
                
                List<cContact> existingContact = masterContacts.Where(c => c.name.Trim().ToLower() == newContact.name.Trim().ToLower()).ToList();
                if (existingContact.Count == 0)
                {
                    if (newContact.name.Trim() != "")
                    {
                        // There is no existing contact, so legitimately need to add a contact.
                        cContact.addContact(copiedContacts, newContact, masterShow);
                    }
                }
            }
            Notes_AirshowStuff += newShow.Notes_AirshowStuff;
            masterShow.Performers.MergePerformers(newShow.Performers);
            Console.WriteLine($"Merging Undaunted Notes: {newShow.UndauntedNotes.Count}".Pastel(Color.Green));
            masterShow.UndauntedNotes = masterShow.UndauntedNotes.Union(newShow.UndauntedNotes).ToList();
            Console.WriteLine($"Merging Airshow Links: {newShow.AirshowLinks.Count}".Pastel(Color.Green));
            masterShow.AirshowLinks = masterShow.AirshowLinks.Union(newShow.AirshowLinks).ToList();
        }
        public void AppendCustomFields(Airshow dupAirshow, List<cContact> copiedContacts)
        {
            //merge the contact lists
            //remove the dupAirShow ID from the contacts

            //this.contactIds = this.contactIds.Union(dupAirshow.contactIds).ToList();
            cContact.RemoveAirshowReference(copiedContacts, dupAirshow);

            this.UndauntedNotes = this.UndauntedNotes.Union(dupAirshow.UndauntedNotes).ToList();
            this.AirshowLinks = this.AirshowLinks.Union(dupAirshow.AirshowLinks).ToList();

            Notes_AirshowStuff += dupAirshow.Notes_AirshowStuff;

            if(dupAirshow.Status < Status)
            {
              Status = dupAirshow.Status;
            }
        }
        /*
         * Tagging a field with [XMLElement] puts the data on a new line
         * Tagging a field with [XMLAttribute] puts the data "in-line"
         * You can always override the name with '("newname")' after the XMLTag
         * Wrapper Classes are for XML Display only.  They are not actually necessary.  
         */

        #region Wrapper Classes
        public class cPerformers
        {
            [XmlElement("Performer")]
            public List<string> performer = new List<string>();

            public void MergePerformers(cPerformers performersToMerge)
            {
                Console.WriteLine("Merging Performers");
                foreach (string testPerformer in performersToMerge.performer)
                {
                    if (!performer.Contains(testPerformer))
                    {
                        Console.WriteLine($"Added Performer: {testPerformer}".Pastel(Color.Green));
                        performer.Add(testPerformer);
                    }
                }
            }
        }

        #endregion

        #region Helper Classes
        public class cLocation
        {
            [XmlIgnore]
            public string? rawstring;
            [XmlIgnore]
            private string? City;
            [XmlIgnore]
            private string? State;

            public string city
            {
                get
                {
                    if (String.IsNullOrEmpty(City) || City == "")
                    {
                        if (String.IsNullOrEmpty(rawstring) || rawstring == "")
                            return "";
                        string[] dog = rawstring.Split(',');
                        if (dog.Length > 0)
                        {
                            City = dog[0].Trim();
                            City = City.Replace("\\", "");
                        }
                    }
                    return City;
                }
                set
                {
                    City = value;
                }
            }
            public string state
            {
                get
                {
                    if (String.IsNullOrEmpty(State) || State == "")
                    {
                        if (rawstring == null)
                            return "";
                        string[] dog = rawstring.Split(',');
                        if (dog.Length > 1)
                        {
                            State = dog[1].Trim();
                        }
                    }
                    return State;
                }
                set
                {
                    State = value;
                }
            }

            public override string ToString()
            {
                return $"{City}, {State}";
            }
            public override bool Equals(object? obj)
            {
                cLocation other = obj as cLocation;
                if (other == null) return false;

                string city1 = other.City.Contains('-') ? other.City.Substring(0, other.City.IndexOf("-")) : other.City;
                string city2 = City.Contains('-') ? City.Substring(0, City.IndexOf("-")) : City;

                if (city1.ToLower() == city2.ToLower() && other.State.ToUpper() == State.ToUpper())
                {
                    return true;
                }
                return false;
            }
        }


        #endregion

        #region Class Functions
        public Airshow()
        { }

        public int Days
        {
            get
            {
                string[] start = date_start.Split('-');
                string[] end = date_finish.Split('-');
                System.DateTime dstart = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
                System.DateTime dfinish = new DateTime(int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2]));
                TimeSpan days = dfinish - dstart;
                return (int)days.TotalDays + 1;
            }
        }

        public int WeekNumber
        {
            get
            {
                string[] start = date_start.Split('-');
                string[] end = date_finish.Split('-');
                System.DateTime dstart = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
                System.DateTime dfinish = new DateTime(int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2]));
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;
                return cal.GetWeekOfYear(dstart, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }
        }

        public int Year
        {
            get
            {
                string[] start = date_start.Split('-');
                System.DateTime dstart = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
                return dstart.Year;
            }
        }

        #endregion

        #region Overrides

        public static List<Airshow> DeepCopy(List<Airshow> originalList)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Airshow>));
                serializer.Serialize(stream, originalList);
                stream.Position = 0;
                return (List<Airshow>)serializer.Deserialize(stream);
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Airshow other = (Airshow)obj;

            // Compare key fields
            bool locationEquals = location != null && location.Equals(other.location);
            bool dateStartEquals = string.Equals(date_start, other.date_start, StringComparison.OrdinalIgnoreCase);
            bool dateFinishEquals = string.Equals(date_finish, other.date_finish, StringComparison.OrdinalIgnoreCase);
            bool nameEquals = string.Equals(name_airshow, other.name_airshow, StringComparison.OrdinalIgnoreCase);
            bool notesEqual = string.Equals(Notes_AirshowStuff, other.Notes_AirshowStuff, StringComparison.OrdinalIgnoreCase);

            // Compare lists (Performers and Contacts) if they are relevant for equality
            //bool performersEqual = Performers != null && other.Performers != null &&
            //                       Performers.performer.SequenceEqual(other.Performers.performer);
            //bool contactsEqual = Contacts != null && other.Contacts != null &&
            //                     Contacts.contact.SequenceEqual(other.Contacts.contact);

            // Define equality based on relevant properties
            return locationEquals && dateStartEquals && dateFinishEquals && nameEquals && notesEqual;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            // Hash code generation for fields, with null checks
            hash = hash * 23 + (location != null ? location.GetHashCode() : 0);
            hash = hash * 23 + (date_start != null ? date_start.GetHashCode(StringComparison.OrdinalIgnoreCase) : 0);
            hash = hash * 23 + (date_finish != null ? date_finish.GetHashCode(StringComparison.OrdinalIgnoreCase) : 0);
            hash = hash * 23 + (name_airshow != null ? name_airshow.GetHashCode(StringComparison.OrdinalIgnoreCase) : 0);
            hash = hash * 23 + (Notes_AirshowStuff != null ? Notes_AirshowStuff.GetHashCode(StringComparison.OrdinalIgnoreCase) : 0);

            // Generate hash codes for lists (Performers and Contacts) if relevant
            //hash = hash * 23 + (Performers != null ? Performers.performer.Aggregate(0, (h, s) => h ^ s.GetHashCode(StringComparison.OrdinalIgnoreCase)) : 0);
            //hash = hash * 23 + (Contacts != null ? Contacts.contact.Aggregate(0, (h, c) => h ^ c.GetHashCode()) : 0);

            return hash;
        }


        public bool IsEqual(object? obj, bool checkName = true)
        {
            Airshow? anAirshow = obj as Airshow;
            if (anAirshow == null) return false;

            bool NameOK = !checkName || anAirshow.name_airshow == name_airshow;

            if (anAirshow.WeekNumber == WeekNumber && anAirshow.location.Equals(location) && anAirshow.Year == Year && NameOK)
                return true;

            return false;
        }

        public override string ToString()
        {
            //let's get the number of days for this show:
            string[] start = date_start.Split('-');
            string[] end = date_finish.Split('-');
            System.DateTime dstart = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
            System.DateTime dfinish = new DateTime(int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2]));
            TimeSpan days = dfinish - dstart;
            int totalDays = days.TotalDays > 0 ? (int)days.TotalDays + 1 : 1;
            string daysstring = "";
            if( totalDays == 1)
            {
                daysstring = "1 day";
            }
            else
            {
                daysstring = totalDays.ToString() + " days";
            }
            return $"{name_airshow.ToString()} - {location.city}, {location.state}, {date_start} - {daysstring}";
        }

        public bool CompareYears(object? obj)
        {

            Airshow? anAirshow = obj as Airshow;
            if (anAirshow == null) return false;

            if (anAirshow.location.Equals(location) || CompareNames(anAirshow.name_airshow))//&& anAirshow.name_airshow == name_airshow) - sometimes name changes by year.  
                return true;

            return false;

        }

        public bool CompareNames(object? obj)
        {
            string? name = obj as string;
            if (name == null) return false;
            if (name.ToUpper() == this.name_airshow.ToUpper())
                return true;

            string name_clean = this.name_airshow.Trim().ToUpper();
            char[] kill = { ',', '-', '$', '&', '(', ')', '\t' };
            foreach (char killchar in kill)
            {

                name_clean = name_clean.Replace(killchar.ToString(), "");
            }

            int n = 0;
            string[] ar_name_clean = name_clean.Split(' ');
            foreach (string stest in ar_name_clean)
            {
                if (name.ToUpper().Contains(stest))
                    n++;
            }
            if ((n == 1 && ar_name_clean.Length == 1) || (n == 2 && ar_name_clean.Length == 2) || n > 2)
                return true;

            return false;

        }
        #endregion

        #region static functions
        public static string[] GetLines(List<Airshow> airshows)
        {
            List<string> strings = new List<string>();

            foreach (Airshow ashow in airshows)
            {
                string sshow = ashow.date_start + "\t" + ashow.date_finish + "\t" + ashow.location.city + ", " + ashow.location.state + "\t" + ashow.name_airshow;



                sshow = ashow.date_start.Length <= 10 ? ashow.date_start.PadRight(10) : ashow.date_start.Substring(0, 10);
                sshow += ashow.date_finish.Length <= 11 ? ashow.date_finish.PadLeft(11) : ashow.date_finish.Substring(0, 11);
                sshow += ashow.location.city.Length <= 30 ? ashow.location.city.PadLeft(30) : ashow.location.city.Substring(0, 30);
                sshow += ashow.location.state.Length <= 2 ? ", " + ashow.location.state.PadLeft(2) : ", " + ashow.location.state.Substring(0, 2);
                sshow += " - ";
                sshow += ashow.name_airshow.Length <= 50 ? ashow.name_airshow.PadRight(50) : ashow.name_airshow.Substring(0, 50);
                strings.Add(sshow);
            }

            return strings.ToArray();
        }
        public static void LoadFile(cAirshowFileParserSetupTool inputfile, List<Airshow> airshows, List<cContact> contacts)
        {
            int contactID = 0;
            int airshowID = 0;
            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(inputfile.sFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            string ScheduleYear = inputfile.AirshowYear.ToString();

            //Electroimpact.FileParser.cFileParse fp = new Electroimpact.FileParser.cFileParse();

            if (inputfile.eFileSource == cAirshowFileParserSetupTool.efilesource.ICAS)
            {
                for (int ii = 0; ii < lines.Length; ii++)
                {
                    // the show starts with a date, we look for the year
                    // line 1 is the start date
                    // line 2 is the end date
                    // line 3 is the name of the show
                    // line 4 is the location
                    // performers start with a -
                    // contacts are after performers and start with a name

                    if (lines[ii].StartsWith(ScheduleYear))
                    {//we got one.

                        Airshow airshow = new Airshow();
                        airshow.date_start = lines[ii++];
                        airshow.date_finish = lines[ii++];
                        airshow.date_added = DateTime.Now.ToString("yyyy-MM-dd");

                        string airshowname = lines[ii++].ToLower();
                        if (airshowname.Contains("air show"))
                            airshowname = airshowname.Replace("air show", "Airshow");

                        airshow.name_airshow = airshowname;
                        airshow.location.rawstring = lines[ii++];
                        string test = airshow.location.city;
                        test = airshow.location.state;

                        List<string> airshowdata = new List<string>();

                        int kk = 0;
                        bool bPerformers = false;
                        for (int jj = ii; jj < lines.Length; jj++)
                        {
                            if (lines[jj].StartsWith(ScheduleYear))
                            {
                                //we made it to the start of the next airshow;
                                cContact acontact = new cContact();
                                foreach (string s in airshowdata)
                                {
                                    if (s.StartsWith("-")) //we have performers
                                    {
                                        string addthis = s.Substring(2);  //get rid of the "- "
                                        airshow.Performers.performer.Add(addthis);
                                        bPerformers = true;
                                        continue;
                                    }
                                    if (kk == 0 && bPerformers == false)
                                    {//need to throw out the line.
                                        bPerformers = true;  //there are no performers, but we've captured the line we want to capture.
                                        continue;
                                    }
                                    if (kk == 0)
                                    {
                                        acontact.name = s;
                                        kk = 1;
                                        continue;
                                    }
                                    if (kk == 1)
                                    {
                                        acontact.phone = s;
                                        kk = 2;
                                        continue;
                                    }
                                    if (kk == 2)
                                    {
                                        string darn = "someting wong";
                                    }
                                }
                                ii = jj - 1;
                                acontact.ID = contactID;
                                acontact.showIDs.Add(airshowID);

                                //airshow.contactIds.Add(contactID);
                                airshow.ID = airshowID;
                                
                                contactID++;
                                airshowID++;

                                contacts.Add(acontact);
                                airshows.Add(airshow);
                                break;
                            }
                            airshowdata.Add(lines[jj]);
                        }
                    }
                }
            }
        }
        internal static string GetTabOutput(List<Airshow> airshows)
        {
            string outputs = "Week\tStart\tFinish\tDays\tCity\tState\tName\tContact\tPhone\n";
            foreach (Airshow airshow in airshows)
            {


                string outputline = airshow.WeekNumber.ToString();
                outputline += "\t" + airshow.date_start;
                outputline += "\t" + airshow.date_finish;
                outputline += "\t" + airshow.Days;
                outputline += "\t" + airshow.location.city;
                outputline += "\t" + airshow.location.state;
                outputline += "\t" + airshow.name_airshow;
                //contacts
                //foreach (cContact acontact in airshow.Contacts.contact)
                //{
                //    outputline += "\t" + acontact.name;
                //    outputline += "\t" + acontact.phone;
                //}
                outputs += outputline + "\n";
            }
            return outputs;
        }
        #endregion
    }
}
