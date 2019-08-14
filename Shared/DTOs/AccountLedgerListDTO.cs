using System.Collections.Generic;

namespace AwqafBlazor.Shared.DTOs
{
    public class AccountLedgerListDTO : PagingInfo
    {
        public List<AccountLedgerDTO> AccountLedgers { get; set; }
    }
}