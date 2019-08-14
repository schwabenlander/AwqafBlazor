using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AwqafBlazor.Server.Services;
using AwqafBlazor.Shared;
using AwqafBlazor.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AwqafBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherRepository _voucherRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<FiscalYearsController> _logger;

        public VouchersController(IVoucherRepository voucherRepo,
                                  IMapper mapper,
                                  ILogger<FiscalYearsController> logger)
        {
            _voucherRepo = voucherRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Vouchers?page=2&itemsPerPage=25
        [HttpGet]
        public async Task<ActionResult<VoucherListDTO>> GetVouchers([FromQuery] int page = 1, [FromQuery] int itemsPerPage = 20)
        {
            try
            {
                if (page < 1 || itemsPerPage < 1)
                    return BadRequest("Request contained one or more invalid paging values.");

                var vouchers = await _voucherRepo.GetVouchers()
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .OrderBy(f => f.VoucherId)
                    .ToListAsync();

                var voucherCount = await _voucherRepo.GetVouchers().CountAsync();

                var voucherList = _mapper.Map<List<Voucher>, List<VoucherDTO>>(vouchers);
                
                var dto = new VoucherListDTO
                {
                    Vouchers = voucherList,
                    TotalItems = voucherCount,
                    TotalPages = decimal.ToInt32(Math.Ceiling((decimal)voucherCount / (decimal)itemsPerPage)),
                    CurrentPage = page,
                    ItemsPerPage = itemsPerPage
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve Vouchers.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/Vouchers/5
        [HttpGet("{id:int}")]
        public ActionResult<VoucherDTO> GetVoucher(int id)
        {
            try
            {
                var voucher = _voucherRepo.GetVoucherById(id);

                if (voucher == null)
                    return NotFound();

                var dto = _mapper.Map<VoucherDTO>(voucher);
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve a Voucher.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/Vouchers/Lookup?fiscalYearId=3&accountId=1&ledgerNo=2
        [HttpGet("Lookup")]
        public async Task<ActionResult<VoucherListDTO>> LookupVoucher(
            [FromQuery] int? fiscalYearId,
            [FromQuery] int? accountId,
            [FromQuery] int? ledgerNo)
        {
            try
            {
                var voucherQuery = _voucherRepo.GetVouchers();

                if (fiscalYearId.HasValue)
                    voucherQuery = voucherQuery.Where(v => v.FiscalYearId == fiscalYearId.Value);
                if (accountId.HasValue)
                    voucherQuery = voucherQuery.Where(v => v.AccountId == accountId.Value);
                if (ledgerNo.HasValue)
                    voucherQuery = voucherQuery.Where(v => v.LedgerNo == ledgerNo.Value);

                var vouchers = await voucherQuery
                    .OrderBy(v => v.VoucherId)
                    .ToListAsync();

                var voucherList = _mapper.Map<List<Voucher>, List<VoucherDTO>>(vouchers);

                var voucherListCount = voucherList.Count;

                var dto = new VoucherListDTO
                {
                    Vouchers = voucherList,
                    TotalItems = voucherListCount,
                    TotalPages = 1,
                    CurrentPage = 1,
                    ItemsPerPage = voucherListCount
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred during a Voucher lookup.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // POST: api/Vouchers
        [HttpPost]
        public ActionResult<VoucherDTO> Post([FromBody] VoucherDTO voucherDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state.");
                    return BadRequest();
                }
                else
                {
                    var voucher = _mapper.Map<Voucher>(voucherDto);

                    _voucherRepo.AddVoucher(voucher);
                    _voucherRepo.Save();

                    return Created($"/api/Vouchers/{voucher.VoucherId}", voucherDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to add a Voucher.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // PUT: api/Vouchers/5
        [HttpPut]
        public ActionResult<VoucherDTO> Put([FromBody] VoucherDTO voucherDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state.");
                    return BadRequest();
                }
                else
                {
                    var voucher = _mapper.Map<Voucher>(voucherDto);

                    _voucherRepo.UpdateVoucher(voucher);
                    _voucherRepo.Save();

                    return Ok(voucherDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to update a Voucher.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:int}")]
        public ActionResult<VoucherDTO> Delete(int id)
        {
            try
            {
                var voucher = _voucherRepo.GetVoucherById(id);

                if (voucher == null)
                    return NotFound();

                _voucherRepo.DeleteVoucher(id);
                _voucherRepo.Save();

                return Ok("Voucher deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to delete a Voucher.\nError: " + e.Message);
                return BadRequest();
            }
        }
    }
}
