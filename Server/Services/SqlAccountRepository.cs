using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;

namespace AwqafBlazor.Server.Services
{
    public class SqlAccountRepository : IAccountRepository
    {
        private readonly AwqafDbContext _db;

        public SqlAccountRepository(AwqafDbContext db)
        {
            _db = db;
        }

        public IQueryable<Account> GetAccounts()
        {
            return _db.Accounts;
        }

        public Account GetAccountById(int id)
        {
            return _db.Accounts.Find(id);
        }

        public Account GetAccount(string account)
        {
            return _db.Accounts.SingleOrDefault(a => a.AccountName == account);
        }

        public int GetMaxAccountId()
        {
            return _db.Accounts.Any() ? _db.Accounts.Max(a => a.AccountId) : 0;
        }

        public Account AddAccount(Account newAccount)
        {
            _db.Accounts.Add(newAccount);

            return newAccount;
        }

        public Account UpdateAccount(Account updatedAccount)
        {
            _db.Accounts.Update(updatedAccount);

            return updatedAccount;
        }

        public Account DeleteAccount(int id)
        {
            var accountToDelete = GetAccountById(id);

            if (accountToDelete != null)
                _db.Accounts.Remove(accountToDelete);

            return accountToDelete;
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}