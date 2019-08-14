using System.Collections.Generic;

namespace AwqafBlazor.Shared.DTOs
{
    public class AccountListDTO : PagingInfo
    {
        public List<AccountDTO> Accounts { get; set; }
    }
}