using System;
using System.Collections.Generic;

namespace AwqafBlazor.Shared
{
    public partial class AccountLedger
    {
        public AccountLedger()
        {
            Vouchers = new HashSet<Voucher>();
        }

        public byte FiscalYearId { get; set; }
        public int AccountId { get; set; }
        public int LedgerNo { get; set; }
        public string Ledger { get; set; }
        //public DateTime? SystemDate { get; set; }
        public int? UserId { get; set; }
        public string Remarks { get; set; }

        public virtual Account Account { get; set; }
        public virtual FiscalYear FiscalYear { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
