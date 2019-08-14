using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;

namespace AwqafBlazor.Server.Services
{
    public interface IVoucherRepository
    {
        IQueryable<Voucher> GetVouchers();
        Voucher GetVoucherById(int id);
        int GetMaxVoucherId();
        Voucher AddVoucher(Voucher newVoucher);
        Voucher UpdateVoucher(Voucher updatedVoucher);
        Voucher DeleteVoucher(int id);
        int Save();
    }
}