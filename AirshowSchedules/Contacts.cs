﻿using Electroimpact.SettingsFormBuilderV2.Attributes;
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
    public class cContact
    {
        public int ID;
        public string name;
        public string phone;
        public string address;
        [Display(DisplayName = "Email:")]
        public List<string> emailAddresses { get; set; } = new List<string>();
        public List<int> showIDs { get; set; } = new List<int>();

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

        //make a static deepCopy
        public static List<cContact> DeepCopy(List<cContact> contacts)
        {
            List<cContact> result = new List<cContact>();
            foreach (cContact contact in contacts)
            {
                cContact newContact = new cContact();
                newContact.ID = contact.ID;
                newContact.name = contact.name;
                newContact.phone = contact.phone;
                newContact.address = contact.address;
                newContact.emailAddresses = new List<string>(contact.emailAddresses);
                newContact.showIDs = new List<int>(contact.showIDs);
                result.Add(newContact);
            }
            return result;
        }

        public cContact DeepCopy()
        {
            cContact newContact = new cContact();
            newContact.ID = ID;
            newContact.name = name;
            newContact.phone = phone;
            newContact.address = address;
            newContact.emailAddresses = new List<string>(emailAddresses);
            newContact.showIDs = new List<int>(showIDs);
            return newContact;
        }

        public static List<cContact> getContacts(List<cContact> contacts, Airshow show)
        {
            List<cContact> result = contacts.Where(c => c.showIDs.Contains(show.ID)).ToList();
            return result;
        }

        
        public static void addContact(List<cContact> contacts, cContact contactToAdd, Airshow show)
        {
            List<cContact> duplicateContact = contacts.Where(c => c.name.Trim().ToLower() == contactToAdd.name.Trim().ToLower()).ToList();
            // public int id;
            // public string name;
            // public string phone;
            // public string address;
            // public List<string> emailAddresses { get; set; } = new List<string>();
            // public List<int> showIds { get; set; } = new List<int>();

            //bind the contacts to the show and the show to the contact

            cContact contact = contactToAdd.DeepCopy();

            if (duplicateContact.Count == 0)
            {
                int maxID = contacts.Max(c => c.ID);
                int newID = maxID + 1;
                contact.ID = newID;

                contact.showIDs.Clear();
                contact.showIDs.Add(show.ID);
                show.contactIds.Add(contact.ID);

                contacts.Add(contact);
            }
            else if (duplicateContact.Count == 1)
            {
                cContact existingContact = duplicateContact[0];
                if (existingContact.phone == null && contact.phone != null)
                {
                    existingContact.phone = contact.phone;
                }
                if (existingContact.phone != null && contact.phone != null && !existingContact.phone.Contains(contact.phone))
                {
                    existingContact.phone += " / " + contact.phone;
                }
                contact.showIDs.Add(show.ID);
                existingContact.address = contact.address;

                existingContact.showIDs = existingContact.showIDs.Union(contact.showIDs).ToList();
                existingContact.emailAddresses = existingContact.emailAddresses.Union(contact.emailAddresses).ToList();

                show.contactIds.Add(existingContact.ID);
            }
            else
            {
                Console.WriteLine($"Duplicate Contact: {contact.name} appers in the database more than once".Pastel(Color.Red));
                foreach (cContact existingContact in duplicateContact)
                {
                    Console.WriteLine($"ID: {existingContact.ID}".Pastel(Color.Yellow));
                    show.contactIds.Add(existingContact.ID);
                }
            }
        }

        public static void RemoveAirshowReference(List<cContact> contacts, Airshow airshowToRemove)
        {
            foreach(cContact contact in contacts) 
            { 
                contact.showIDs.Remove(airshowToRemove.ID);
            }
        }

        internal bool IsEqual(cContact latestContact)
        {
            if (latestContact.name != name) return false;
            if (latestContact.phone != phone) return false;
            if (latestContact.address != address) return false;
            if (latestContact.emailAddresses.Count != emailAddresses.Count) return false;
            if (latestContact.showIDs.Count != showIDs.Count) return false;

            foreach (string email in latestContact.emailAddresses)
            {
                if (!emailAddresses.Contains(email)) return false;
            }

            foreach (int showID in latestContact.showIDs)
            {
                if (!showIDs.Contains(showID)) return false;
            }

            return true;
        }

        //public void MergeContacts(List<cContact> contactsToMerge, Airshow airshow)
        //{
        //    Console.WriteLine("Merging Contacts");
        //    foreach (cContact testContact in contactsToMerge)
        //    {
        //        List<cContact> matchingContacts = myContacts.Where(c => c.name == testContact.name).ToList();
        //        if (matchingContacts.Count == 0)
        //        {
        //            myContacts.Add(testContact);
        //            Console.WriteLine($"Added {testContact.name}".Pastel(Color.Green));
        //        }
        //        else
        //        {
        //            foreach (cContact matchingContact in matchingContacts)
        //            {
        //                if (matchingContact.phone != testContact.phone)
        //                {
        //                    matchingContact.phone = testContact.phone;
        //                    Console.WriteLine($"Updated {testContact.name} phone number".Pastel(Color.Green));
        //                }
        //            }
        //        }
        //    }
        //}

    }
}