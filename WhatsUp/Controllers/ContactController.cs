using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WhatsUp.Repositories;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Business_Logic;
using WhatsUp.Models.Input_Models;

namespace WhatsUp.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private IContactRepository contactRepository = new ContactRepository();
        private IAccountRepository accountRepository = new AccountRepository();
        private ContactLogic contactLogic = new ContactLogic();

        private Account GetAccount()
        {
            Account account = (Account)Session["loggedin_account"];
            return account;
        }

        public ActionResult Index()
        {
            Account account = GetAccount();
            IEnumerable<Contact> contacts = contactRepository.GetAllContactsForAccount(account.AccountId);
                     
            return View(contacts);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ContactModel model)
        {
            Account account = GetAccount();

            Contact contact = contactRepository.GetContact(model.MobileNumber, account.AccountId);

            if (ModelState.IsValid)
            {     
                if (contact == null) // Can't add contacts twice
                {
                    if (model.MobileNumber != account.MobileNumber) // Can't add yourself
                    {
                        contactLogic.CreateContact(model.MobileNumber, model.ContactName, account.AccountId);
                        return RedirectToAction("Index");
                    }
                    else ModelState.AddModelError("", "You can't add yourself as a contact");
                }
                else ModelState.AddModelError("", "You can't add the same number twice");
            }
            else ModelState.AddModelError("", "Your input is invalid");     
            return View(model); // Show view again including model error
        }
        
        [HttpGet]
        public ActionResult Remove(int contactId)
        {
            Contact contact = contactRepository.GetContact(contactId);
            return View(contact);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int contactId)
        {
            Contact contact = contactRepository.GetContact(contactId);
            contactRepository.RemoveContact(contact);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int contactId)
        {
            Contact contact = contactRepository.GetContact(contactId);
            ContactModel model = new ContactModel();

            model.ContactName = contact.ContactName;
            model.MobileNumber = contact.MobileNumber;
            model.ContactId = contact.ContactId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ContactModel model)
        {
            Account account = GetAccount();

            if (ModelState.IsValid)
            {
                Contact contact = contactRepository.GetContact(model.ContactId);
                contact.ContactName = model.ContactName;
                contact.MobileNumber = model.MobileNumber;

                contactRepository.AddOwnerAccountsToContacts(contact.MobileNumber, account.AccountId);              
                contactRepository.EditContact(contact);
                return RedirectToAction("Index", "Contact");
            }
            else ModelState.AddModelError("", "Please don't leave any values empty");
            return View(model);
        }

        [HttpGet]
        public ActionResult PhoneBook()
        {
            IEnumerable<Account> accounts = accountRepository.GetAllAccounts();
            return View(accounts);
        }
    }
}