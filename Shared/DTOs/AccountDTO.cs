using System;
using System.ComponentModel.DataAnnotations;

namespace AwqafBlazor.Shared.DTOs
{
    public class AccountDTO
    {
        [Required, Display(Name = "Account ID"), Range(1, int.MaxValue)]
        public int AccountId { get; set; }

        [Required, Display(Name = "Account"), StringLength(100)]
        public string AccountName { get; set; }

        [Required, Display(Name = "Level 1"), Range(0, short.MaxValue)]
        public short Level1 { get; set; }

        [Required, Display(Name = "Level 2"), Range(0, short.MaxValue)]
        public short Level2 { get; set; }

        [Required, Display(Name = "Level 3"), Range(0, short.MaxValue)]
        public short Level3 { get; set; }

        [Required, Display(Name = "Level 4"), Range(0, short.MaxValue)]
        public short Level4 { get; set; }

        [Display(Name = "Remarks"), StringLength(300), DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "User ID"), Range(1, int.MaxValue)]
        public int? UserId { get; set; }
    }
}