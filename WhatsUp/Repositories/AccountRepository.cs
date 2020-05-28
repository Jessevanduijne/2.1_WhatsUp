using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WhatsUp.Models.Database_Connection;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private WhatsUpContext db = new WhatsUpContext();

        public void CreateAccount(Account account)
        {
            db.Accounts.Add(account);            
            db.SaveChanges();
        }

        public Account GetAccount(string mobileNumber, string password)
        {
            Account account = db.Accounts.SingleOrDefault(x => x.MobileNumber == mobileNumber && x.Password == password);           
            return account;
        }     
        
        public Account GetAccount(int accountId)
        {
            Account account = db.Accounts.SingleOrDefault(x => x.AccountId == accountId);
            return account;
        }

        public Account GetAccount(string mobileNumber)
        {
            Account account = db.Accounts.SingleOrDefault(x => x.MobileNumber == mobileNumber);
            return account;
        }

        public Boolean CheckIfAccountExists(string mobileNumber)
        {
            bool check = db.Accounts.Any(x => x.MobileNumber == mobileNumber);
            return check;
        }
        
        public IEnumerable<Account> GetAllAccounts()
        {
            IEnumerable<Account> accounts = db.Accounts;
            return accounts;
        }
    }
}