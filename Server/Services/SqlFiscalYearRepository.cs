using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AwqafBlazor.Server.Services
{
    public class SqlFiscalYearRepository : IFiscalYearRepository
    {
        private readonly AwqafDbContext _db;

        public SqlFiscalYearRepository(AwqafDbContext db)
        {
            _db = db;
        }

        public IQueryable<FiscalYear> GetFiscalYears()
        {
            return _db.FiscalYears;
        }

        public FiscalYear GetFiscalYear(string year)
        {
            return _db.FiscalYears.SingleOrDefault(f => f.YearDescription == year);
        }

        public FiscalYear GetFiscalYearById(byte id)
        {
            return _db.FiscalYears.Find(id);
        }

        public FiscalYear AddFiscalYear(FiscalYear newFiscalYear)
        {
            _db.FiscalYears.Add(newFiscalYear);

            return newFiscalYear;
        }

        public FiscalYear UpdateFiscalYear(FiscalYear updatedFiscalYear)
        {
            _db.FiscalYears.Update(updatedFiscalYear);

            return updatedFiscalYear;
        }

        public FiscalYear DeleteFiscalYear(byte id)
        {
            var fiscalYearToDelete = GetFiscalYearById(id);

            if (fiscalYearToDelete != null)
                _db.FiscalYears.Remove(fiscalYearToDelete);

            return fiscalYearToDelete;
        }

        public byte GetMaxFiscalYearId()
        {
            return _db.FiscalYears.Any() ? _db.FiscalYears.Max(f => f.FiscalYearId) : (byte)0;
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}