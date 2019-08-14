using System.Collections.Generic;
using System.Linq;
using AwqafBlazor.Shared;

namespace AwqafBlazor.Server.Services
{
    public interface IFiscalYearRepository
    {
        IQueryable<FiscalYear> GetFiscalYears();
        FiscalYear GetFiscalYear(string year);
        FiscalYear GetFiscalYearById(byte id);
        FiscalYear AddFiscalYear(FiscalYear newFiscalYear);
        FiscalYear UpdateFiscalYear(FiscalYear updatedFiscalYear);
        FiscalYear DeleteFiscalYear(byte id);
        byte GetMaxFiscalYearId();
        int Save();
    }
}