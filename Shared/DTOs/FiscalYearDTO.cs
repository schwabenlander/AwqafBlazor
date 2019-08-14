using System;
using System.ComponentModel.DataAnnotations;

namespace AwqafBlazor.Shared.DTOs
{
    public class FiscalYearDTO
    {
        [Required, Display(Name = "Fiscal Year ID"), Range(1, Byte.MaxValue)]
        public byte FiscalYearId { get; set; }

        [Required, StringLength(4, MinimumLength = 4), Display(Name = "Year")]
        public string YearDescription { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Is Current?")]
        public bool IsCurrent { get; set; }

        [Display(Name = "Is Open?")]
        public bool IsOpen { get; set; }
    }
}