using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Input_Models;
using WhatsUp.Repositories;

namespace WhatsUp.Models.Business_Logic
{
    public class ContactLogic
    {
        IContactRepository contactRepository = new ContactRepository();
        IAccountRepository accountRepository = new AccountRepository();

        public void CreateContact(string mobileNumber, string contactName, int accountId)
        {
            Contact contact = new Contact();
            contact.MobileNumber = mobileNumber;
            contact.ContactName = contactName;
            contact.ContactOwnerId = accountId;

            Account contactAccount = accountRepository.GetAccount(mobileNumber);
            if (contactAccount != null)
            {
                contact.OwnerAccountId = contactAccount.AccountId;
            }

            contactRepository.AddContact(contact);
        }
    }
}