using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsUp.Models.Database_Connection;
using WhatsUp.Models.Domain_Models;
using System.Data.Entity;

namespace WhatsUp.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private WhatsUpContext db = new WhatsUpContext();
        public ICollection<Contact> GetAllContactsForAccount(int accountId)
        {
            ICollection<Contact> contacts = db.Contacts.Where(x => x.ContactOwnerId == accountId).ToList();
            return contacts;
        }

        public Contact GetContact(int contactId)
        {
            Contact contact = db.Contacts.Find(contactId);
            return contact;
        }

        public Contact GetContact(int contactOwnerId, int ownerAccountId) 
        {
            Contact contact = db.Contacts.FirstOrDefault(x => x.ContactOwnerId == contactOwnerId && x.OwnerAccountId == ownerAccountId);
            return contact;
        }

        public Contact GetContact(string mobileNumber, int contactOwnerId) // This method is used when a contact doesn't have an account yet
        {
            Contact contact = db.Contacts.FirstOrDefault(x => x.MobileNumber == mobileNumber && x.ContactOwnerId == contactOwnerId);
            return contact;
        }

        public void AddContact(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();
        }

        public void RemoveContact(Contact contact)
        {
            db.Contacts.Remove(contact);
            db.SaveChanges();
        }

        public Contact EditContact(Contact contact)
        {
            db.Entry(contact).State = EntityState.Modified;
            db.SaveChanges();
            return contact;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            IEnumerable<Contact> contacts = db.Contacts;
            return contacts;
        }

        public void AddOwnerAccountsToContacts(string accountMobileNumber, int accountId)
        {
            IEnumerable<Contact> contacts = db.Contacts.Where(x => x.MobileNumber == accountMobileNumber);
            foreach(Contact c in contacts)
            {
                c.OwnerAccountId = accountId;
                db.Entry(c).State = EntityState.Modified;
            }
            db.SaveChanges();
        }
    }
}