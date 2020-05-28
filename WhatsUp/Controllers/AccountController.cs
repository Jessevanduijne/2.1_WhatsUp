using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsUp.Repositories;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Input_Models;
using WhatsUp.Models.Business_Logic;
using System.Web.Security;

namespace WhatsUp.Controllers
{
    public class AccountController : Controller
    {
        private IAccountRepository accountRepository = new AccountRepository();
        private IContactRepository contactRepository = new ContactRepository();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {            
            if (ModelState.IsValid)
            {
                // Hash password to match registered hashed password:
                byte[] data = System.Text.Encoding.ASCII.GetBytes(model.Password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                string password = System.Text.Encoding.ASCII.GetString(data);

                // Find account:
                Account account = accountRepository.GetAccount(model.MobileNumber, password);
                if (account != null)
                {
                    FormsAuthentication.SetAuthCookie(account.MobileNumber, false);
                    Session["loggedin_account"] = account;                    
                    return RedirectToAction("Index", "Contact");
                }
                else ModelState.AddModelError("Login_error", "The mobile number or password provided is incorrect.");                
            }
            else ModelState.AddModelError("", "The input provided is invalid.");
            return View(model); // Same view, including model errors
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {               
                if (!accountRepository.CheckIfAccountExists(model.MobileNumber)) // The mobileNumber must not exist yet
                {
                    Account account = new Account();
                    account.MobileNumber = model.MobileNumber;
                    account.FirstName = model.FirstName;
                    account.LastName = model.LastName;

                    // Hash password:
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(model.Password);
                    data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                    account.Password = System.Text.Encoding.ASCII.GetString(data);

                    // Add to Database:
                    accountRepository.CreateAccount(account);

                    // Multiple accounts can add the same contact, a contact which does not have an account yet
                    // ..if so, an accountId is linked to the contact when it creates it's account
                    contactRepository.AddOwnerAccountsToContacts(account.MobileNumber, account.AccountId);
                    return RedirectToAction("Login", "Account");
                }                
                else ModelState.AddModelError("", "This mobile number already has an account. Try logging in with it!");                               
            }            
            return View(model); // Same view, including model errors
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // Clear session
            return RedirectToAction("Login", "Account");
        }
    }
}