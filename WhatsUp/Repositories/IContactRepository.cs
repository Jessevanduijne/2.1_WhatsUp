using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Repositories
{
    interface IContactRepository
    {
        ICollection<Contact> GetAllContactsForAccount(int accountId);
        Contact GetContact(int contactId);
        Contact GetContact(int contactOwnerId, int ownerAccountId);
        Contact GetContact(string mobileNumber, int accountId);
        void AddContact(Contact contact);
        void RemoveContact(Contact contact);
        Contact EditContact(Contact contact);
        IEnumerable<Contact> GetAllContacts();
        void AddOwnerAccountsToContacts(string accountMobileNumber, int accountId);
    }
}
