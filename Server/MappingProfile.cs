using AutoMapper;
using AwqafBlazor.Shared;
using AwqafBlazor.Shared.DTOs;

namespace AwqafBlazor.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FiscalYear, FiscalYearDTO>().ReverseMap();

            CreateMap<Account, AccountDTO>().ReverseMap();

            CreateMap<AccountLedger, AccountLedgerDTO>().ReverseMap();

            CreateMap<Voucher, VoucherDTO>().ReverseMap();
        }
    }
}