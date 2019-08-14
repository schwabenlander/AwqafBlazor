using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;

namespace AwqafBlazor.Server.Services
{
    public interface IAccountRepository
    {
        IQueryable<Account> GetAccounts();
        Account GetAccountById(int id);
        Account GetAccount(string account);
        Account AddAccount(Account newAccount);
        Account UpdateAccount(Account updatedAccount);
        Account DeleteAccount(int id);
        int GetMaxAccountId();
        int Save();
    }
}