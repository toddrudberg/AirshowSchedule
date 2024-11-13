using Electroimpact.SettingsFormBuilderV2.Attributes;
using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AirshowSchedules
{
    internal class Contacts
    {
    }

    public class cContact
    {
        public int id;
        public string name;
        public string phone;
        public string address;
        [Display(DisplayName = "Email:")]
        public List<string> emailAddresses { get; set; } = new List<string>();
        public List<int> showIds { get; set; } = new List<int>();

        public cContact()
        { }

        public override string ToString()
        {
            return name.ToString();
        }

        public override bool Equals(object? obj)
        {
            cContact other = obj as cContact;
            if (other == null) return false;
            
            if (other.name.Trim().ToLower() == name.Trim().ToLower())
            {
                return true;
            }
            return false;
        }

        internal static void SaveMe(List<cContact> myContacts, string fileName)
        {
            string json = JsonConvert.SerializeObject(myContacts, Formatting.Indented);
            
            //make sure the directory exists
            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            System.IO.File.WriteAllText(fileName, json);
            Console.WriteLine($"Contacts have been exported to {fileName}".Pastel(Color.LimeGreen));
        }

        internal static List<cContact> LoadMe(string fnContactDataBase, out bool success)
        {
            List<cContact> myContacts = new List<cContact>();
            success = false;
            if (System.IO.File.Exists(fnContactDataBase))
            {
                string json = System.IO.File.ReadAllText(fnContactDataBase);
                myContacts = JsonConvert.DeserializeObject<List<cContact>>(json);
                Console.WriteLine($"Contacts have been imported from {fnContactDataBase}".Pastel(Color.LimeGreen));
                success = true;
            }
            return myContacts;
        }
    }

    public class cContacts
    {
        [XmlElement("Contact")]
        public List<cContact> contact = new List<cContact>();

        public void MergeContacts(cContacts contactsToMerge)
        {
            Console.WriteLine("Merging Contacts");
            foreach (cContact testContact in contactsToMerge.contact)
            {
                List<cContact> matchingContacts = contact.Where(c => c.name == testContact.name).ToList();
                if (matchingContacts.Count == 0)
                {
                    contact.Add(testContact);
                    Console.WriteLine($"Added {testContact.name}".Pastel(Color.Green));
                }
                else
                {
                    foreach (cContact matchingContact in matchingContacts)
                    {
                        if (matchingContact.phone != testContact.phone)
                        {
                            matchingContact.phone = testContact.phone;
                            Console.WriteLine($"Updated {testContact.name} phone number".Pastel(Color.Green));
                        }
                    }
                }
            }
        }
    }
}
