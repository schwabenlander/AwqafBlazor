using System;
using System.Collections.Generic;

namespace AwqafBlazor.Shared
{
    public partial class FiscalYear
    {
        public FiscalYear()
        {
            AccountLedgers = new HashSet<AccountLedger>();
        }

        public byte FiscalYearId { get; set; }
        public string YearDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte? IsCurrent { get; set; }
        public byte? IsOpen { get; set; }

        public virtual ICollection<AccountLedger> AccountLedgers { get; set; }
    }
}
