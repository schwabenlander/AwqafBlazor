using System.Collections.Generic;

namespace AwqafBlazor.Shared.DTOs
{
    public class FiscalYearListDTO : PagingInfo
    {
        public List<FiscalYearDTO> FiscalYears { get; set; }
    }
}