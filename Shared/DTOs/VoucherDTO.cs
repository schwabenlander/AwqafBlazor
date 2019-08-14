using System;
using System.ComponentModel.DataAnnotations;

namespace AwqafBlazor.Shared.DTOs
{
    public class VoucherDTO
    {
        [Required, Display(Name = "Voucher ID"), Range(1, int.MaxValue)]
        public int VoucherId { get; set; }

        [DataType(DataType.Date), Display(Name = "Voucher Date")]
        public DateTime? VoucherDate { get; set; }

        [Display(Name = "Voucher Status ID"), Range(0, byte.MaxValue)]
        public byte? VoucherStatusId { get; set; }

        [DataType(DataType.Currency), Display(Name = "Voucher Total")]
        public decimal? VoucherTotal { get; set; }

        [Required, Display(Name = "Fiscal Year ID"), Range(1, byte.MaxValue)]
        public byte FiscalYearId { get; set; }

        [Required, Display(Name = "Account ID"), Range(1, int.MaxValue)]
        public int AccountId { get; set; }

        [Required, Display(Name = "Ledger Number"), Range(1, int.MaxValue)]
        public int LedgerNo { get; set; }

        [Display(Name = "Remarks"), StringLength(1000)]
        public string Remarks { get; set; }

        [Display(Name = "User ID"), Range(1, int.MaxValue)]
        public int? UserId { get; set; }
    }
}