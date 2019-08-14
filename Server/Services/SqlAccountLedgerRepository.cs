using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;
using Microsoft.EntityFrameworkCore;

namespace AwqafBlazor.Server.Services
{
    public class SqlAccountLedgerRepository : IAccountLedgerRepository
    {
        private readonly AwqafDbContext _db;

        public SqlAccountLedgerRepository(AwqafDbContext db)
        {
            _db = db;
        }

        public IQueryable<AccountLedger> GetAccountLedgers()
        {
            return _db.AccountsLedgers
                .Include(a => a.Account)
                .Include(a => a.FiscalYear);
        }

        public AccountLedger GetAccountLedger(byte fiscalYearId, int accountId, int ledgerNo)
        {
            return _db.AccountsLedgers
                .Include(a => a.Account)
                .Include(a => a.FiscalYear)
                .SingleOrDefault(a => a.FiscalYearId == fiscalYearId &&
                                      a.AccountId == accountId &&
                                      a.LedgerNo == ledgerNo);
        }

        public AccountLedger AddAccountLedger(AccountLedger newAccountLedger)
        {
            _db.AccountsLedgers.Add(newAccountLedger);

            return newAccountLedger;
        }

        public AccountLedger UpdateAccountLedger(AccountLedger updatedAccountLedger)
        {
            _db.AccountsLedgers.Update(updatedAccountLedger);

            return updatedAccountLedger;
        }

        public AccountLedger DeleteAccountLedger(byte fiscalYearId, int accountId, int ledgerNo)
        {
            var accountLedgerToDelete = GetAccountLedger(fiscalYearId, accountId, ledgerNo);

            if (accountLedgerToDelete != null)
                _db.AccountsLedgers.Remove(accountLedgerToDelete);

            return accountLedgerToDelete;
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}