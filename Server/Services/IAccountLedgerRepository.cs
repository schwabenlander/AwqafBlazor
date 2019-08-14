using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;

namespace AwqafBlazor.Server.Services
{
    public interface IAccountLedgerRepository
    {
        IQueryable<AccountLedger> GetAccountLedgers();
        AccountLedger GetAccountLedger(byte fiscalYearId, int accountId, int ledgerNo);
        AccountLedger AddAccountLedger(AccountLedger newAccountLedger);
        AccountLedger UpdateAccountLedger(AccountLedger updatedAccountLedger);
        AccountLedger DeleteAccountLedger(byte fiscalYearId, int accountId, int ledgerNo);
        int Save();
    }
}