using System.Collections.Generic;

namespace AwqafBlazor.Shared.DTOs
{
    public class VoucherListDTO : PagingInfo
    {
        public List<VoucherDTO> Vouchers { get; set; }
    }
}