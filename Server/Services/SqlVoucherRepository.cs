using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;

namespace AwqafBlazor.Server.Services
{
    public class SqlVoucherRepository : IVoucherRepository
    {
        private readonly AwqafDbContext _db;

        public SqlVoucherRepository(AwqafDbContext db)
        {
            _db = db;
        }

        public IQueryable<Voucher> GetVouchers()
        {
            return _db.Vouchers;
        }

        public Voucher GetVoucherById(int id)
        {
            return _db.Vouchers.Find(id);
        }

        public int GetMaxVoucherId()
        {
            return _db.Vouchers.Any() ? _db.Vouchers.Max(v => v.VoucherId) : 0;
        }

        public Voucher AddVoucher(Voucher newVoucher)
        {
            _db.Vouchers.Add(newVoucher);

            return newVoucher;
        }

        public Voucher UpdateVoucher(Voucher updatedVoucher)
        {
            _db.Vouchers.Update(updatedVoucher);

            return updatedVoucher;
        }

        public Voucher DeleteVoucher(int id)
        {
            var voucherToDelete = GetVoucherById(id);

            if (voucherToDelete != null)
                _db.Vouchers.Remove(voucherToDelete);

            return voucherToDelete;
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}