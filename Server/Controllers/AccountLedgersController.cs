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
    public class AccountLedgersController : ControllerBase
    {
        private readonly IAccountLedgerRepository _accountLedgerRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountLedgersController> _logger;

        public AccountLedgersController(IAccountLedgerRepository accountLedgerRepo, 
                                        IMapper mapper,
                                        ILogger<AccountLedgersController> logger)
        {
            _accountLedgerRepo = accountLedgerRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/AccountLedgers?page=2&itemsPerPage=25
        [HttpGet]
        public async Task<ActionResult<AccountLedgerListDTO>> GetAccountLedgers([FromQuery] int page = 1, [FromQuery] int itemsPerPage = 20)
        {
            try
            {
                if (page < 1 || itemsPerPage < 1)
                    return BadRequest("Request contained one or more invalid paging values.");

                var accountLedgers = await _accountLedgerRepo.GetAccountLedgers()
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToListAsync();

                var accountLedgerCount = await _accountLedgerRepo.GetAccountLedgers().CountAsync();

                var accountLedgerList = _mapper.Map<List<AccountLedger>, List<AccountLedgerDTO>>(accountLedgers);

                var dto = new AccountLedgerListDTO
                {
                    AccountLedgers = accountLedgerList,
                    TotalItems = accountLedgerCount,
                    TotalPages = decimal.ToInt32(Math.Ceiling((decimal)accountLedgerCount / (decimal)itemsPerPage)),
                    CurrentPage = page,
                    ItemsPerPage = itemsPerPage
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve Account Ledgers.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/AccountLedgers/3/2/4
        [HttpGet("{fiscalYearId:int}/{accountId:int}/{ledgerNo:int}")]
        public ActionResult<AccountLedgerDTO> GetAccountLedger(int fiscalYearId, int accountId, int ledgerNo)
        {
            try
            {
                if (fiscalYearId < byte.MinValue || fiscalYearId > byte.MaxValue)
                {
                    return BadRequest($"Fiscal Year ID must be between {byte.MinValue} and {byte.MaxValue}.");
                }

                var accountLedger = _accountLedgerRepo.GetAccountLedger((byte)fiscalYearId, 
                                                                        accountId, 
                                                                        ledgerNo);

                if (accountLedger == null)
                    return NotFound();

                var dto = _mapper.Map<AccountLedgerDTO>(accountLedger);
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve an Account Ledger.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/AccountLedgers/Lookup?fiscalYearId=3&accountId=2&ledgerNo=4
        [HttpGet("Lookup")]
        public async Task<ActionResult<AccountLedgerListDTO>> LookupAccountLedger(
            [FromQuery] int? fiscalYearId,
            [FromQuery] int? accountId,
            [FromQuery] int? ledgerNo)
        {
            try
            {
                if (fiscalYearId.HasValue && (fiscalYearId.Value < byte.MinValue || fiscalYearId.Value > byte.MaxValue))
                {
                    return BadRequest($"Fiscal Year ID must be between {byte.MinValue} and {byte.MaxValue}.");
                }

                var accountLedgerQuery = _accountLedgerRepo.GetAccountLedgers();

                if (fiscalYearId.HasValue)
                    accountLedgerQuery = accountLedgerQuery.Where(l => l.FiscalYearId == fiscalYearId.Value);
                if (accountId.HasValue)
                    accountLedgerQuery = accountLedgerQuery.Where(l => l.AccountId == accountId.Value);
                if (ledgerNo.HasValue)
                    accountLedgerQuery = accountLedgerQuery.Where(l => l.LedgerNo == ledgerNo.Value);

                var accountLedgers = await accountLedgerQuery
                    .OrderBy(l => l.FiscalYearId)
                    .ThenBy(l => l.AccountId)
                    .ThenBy(l => l.LedgerNo)
                    .ToListAsync();

                var accountLedgerList = _mapper.Map<List<AccountLedger>, List<AccountLedgerDTO>>(accountLedgers);

                var accountLedgerListCount = accountLedgerList.Count;

                var dto = new AccountLedgerListDTO
                {
                    AccountLedgers = accountLedgerList,
                    TotalItems = accountLedgerListCount,
                    TotalPages = 1,
                    CurrentPage = 1,
                    ItemsPerPage = accountLedgerListCount
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to lookup an Account Ledger.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // POST: api/AccountLedgers
        [HttpPost]
        public ActionResult<AccountLedgerDTO> Post([FromBody] AccountLedgerDTO accountLedgerDto)
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
                    var accountLedger = _mapper.Map<AccountLedger>(accountLedgerDto);

                    _accountLedgerRepo.AddAccountLedger(accountLedger);
                    _accountLedgerRepo.Save();

                    return Created($"/api/AccountLedgers/{accountLedger.FiscalYearId}-{accountLedger.AccountId}-{accountLedger.LedgerNo}", 
                        accountLedgerDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to add an Account Ledger.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // PUT: api/AccountLedgers
        [HttpPut]
        public ActionResult<AccountLedgerDTO> Put([FromBody] AccountLedgerDTO accountLedgerDto)
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
                    var accountLedger = _mapper.Map<AccountLedger>(accountLedgerDto);

                    _accountLedgerRepo.UpdateAccountLedger(accountLedger);
                    _accountLedgerRepo.Save();

                    return Ok(accountLedgerDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to update an Account.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // DELETE: api/AccountLedgers/3/2/4
        [HttpDelete("{fiscalYearId:int}/{accountId:int}/{ledgerNo:int}")]
        public ActionResult<AccountLedgerDTO> Delete(int fiscalYearId, int accountId, int ledgerNo)
        {
            if (fiscalYearId < byte.MinValue || fiscalYearId > byte.MaxValue)
            {
                return BadRequest($"Fiscal Year ID must be between {byte.MinValue} and {byte.MaxValue}.");
            }

            try
            {
                var accountLedger = _accountLedgerRepo.GetAccountLedger((byte) fiscalYearId, accountId, ledgerNo);

                if (accountLedger == null)
                    return NotFound();

                _accountLedgerRepo.DeleteAccountLedger(accountLedger.FiscalYearId,
                    accountLedger.AccountId,
                    accountLedger.LedgerNo);
                _accountLedgerRepo.Save();

                return Ok("Account Ledger deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to delete a Fiscal Year.\nError: " + e.Message);
                return BadRequest();
            }
        }
    }
}
