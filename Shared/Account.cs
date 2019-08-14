using System;
using System.Collections.Generic;

namespace AwqafBlazor.Shared
{
    public partial class Account
    {
        public Account()
        {
            AccountLedgers = new HashSet<AccountLedger>();
        }

        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public short Level1 { get; set; }
        public short Level2 { get; set; }
        public short Level3 { get; set; }
        public short Level4 { get; set; }
        //public DateTime? SystemDate { get; set; }
        public int? UserId { get; set; }
        public string Remarks { get; set; }

        public virtual ICollection<AccountLedger> AccountLedgers { get; set; }
    }
}
