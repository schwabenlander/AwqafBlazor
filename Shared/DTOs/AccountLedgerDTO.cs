using System.ComponentModel.DataAnnotations;

namespace AwqafBlazor.Shared.DTOs
{
    public class AccountLedgerDTO
    {
        [Required, Display(Name = "Fiscal Year ID"), Range(1, byte.MaxValue)]
        public byte FiscalYearId { get; set; }

        [Required, Display(Name = "Account ID"), Range(1, int.MaxValue)]
        public int AccountId { get; set; }

        [Required, Display(Name = "Ledger Number"), Range(1, int.MaxValue)]
        public int LedgerNo { get; set; }

        [Required, Display(Name = "Ledger"), StringLength(250)]
        public string Ledger { get; set; }

        [Display(Name = "Remarks"), StringLength(300)]
        public string Remarks { get; set; }

        [Display(Name = "User ID"), Range(1, int.MaxValue)]
        public int? UserId { get; set; }
    }
}