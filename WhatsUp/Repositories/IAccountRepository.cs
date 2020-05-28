using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Repositories
{
    interface IAccountRepository
    {
        void CreateAccount(Account account);
        Account GetAccount(string mobileNumber, string password);
        Account GetAccount(int accountId);
        Account GetAccount(string mobileNumber);
        Boolean CheckIfAccountExists(string mobileNumber);
        IEnumerable<Account> GetAllAccounts();
    }
}
