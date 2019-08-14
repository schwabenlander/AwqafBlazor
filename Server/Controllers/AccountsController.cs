using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AwqafBlazor.Server.Services;
using AwqafBlazor.Shared;
using AwqafBlazor.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AwqafBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountRepository accountRepo, 
                                  IMapper mapper,
                                  ILogger<AccountsController> logger)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Accounts?page=2&itemsPerPage=25
        [HttpGet]
        public async Task<ActionResult<AccountListDTO>> GetAccounts([FromQuery] int page = 1, [FromQuery] int itemsPerPage = 20)
        {
            try
            {
                if (page < 1 || itemsPerPage < 1)
                    return BadRequest("Request contained one or more invalid paging values.");

                var accounts = await _accountRepo.GetAccounts()
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .OrderBy(a => a.AccountId)
                    .ToListAsync();

                var accountCount = await _accountRepo.GetAccounts().CountAsync();

                var accountList = _mapper.Map<List<Account>, List<AccountDTO>>(accounts);

                var dto = new AccountListDTO
                {
                    Accounts = accountList,
                    TotalItems = accountCount,
                    TotalPages = decimal.ToInt32(Math.Ceiling((decimal)accountCount / (decimal)itemsPerPage)),
                    CurrentPage = page,
                    ItemsPerPage = itemsPerPage
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve Accounts.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/Accounts/5
        [HttpGet("{id:int}")]
        public ActionResult<AccountDTO> GetAccount(int id)
        {
            try
            {
                var account = _accountRepo.GetAccountById(id);

                if (account == null)
                    return NotFound();

                var dto = _mapper.Map<AccountDTO>(account);
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve an Account.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/Accounts/Lookup?l1=6&l2=9&l3=9&l4=5
        [HttpGet("Lookup")]
        public async Task<ActionResult<AccountListDTO>> LookupAccount(
                [FromQuery] short? l1,
                [FromQuery] short? l2,
                [FromQuery] short? l3,
                [FromQuery] short? l4)
        {
            try
            {
                var accountQuery = _accountRepo.GetAccounts();

                if (l1.HasValue)
                    accountQuery = accountQuery.Where(a => a.Level1 == l1.Value);
                if (l2.HasValue)
                    accountQuery = accountQuery.Where(a => a.Level2 == l2.Value);
                if (l3.HasValue)
                    accountQuery = accountQuery.Where(a => a.Level3 == l3.Value);
                if (l4.HasValue)
                    accountQuery = accountQuery.Where(a => a.Level4 == l4.Value);

                var accounts = await accountQuery
                    .OrderBy(a => a.AccountId)
                    .ToListAsync();

                var accountList = _mapper.Map<List<Account>, List<AccountDTO>>(accounts);

                var accountListCount = accountList.Count;

                var dto = new AccountListDTO
                {
                    Accounts = accountList,
                    TotalItems = accountListCount,
                    TotalPages = 1,
                    CurrentPage = 1,
                    ItemsPerPage = accountListCount
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred during an Account lookup.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // POST: api/Accounts
        [HttpPost]
        public ActionResult<AccountDTO> Post([FromBody] AccountDTO accountDto)
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
                    var account = _mapper.Map<Account>(accountDto);

                    _accountRepo.AddAccount(account);
                    _accountRepo.Save();

                    return Created($"/api/Accounts/{account.AccountId}", accountDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to add an Account.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // PUT: api/Accounts
        [HttpPut]
        public ActionResult<AccountDTO> Put([FromBody] AccountDTO accountDto)
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
                    var account = _mapper.Map<Account>(accountDto);

                    _accountRepo.UpdateAccount(account);
                    _accountRepo.Save();

                    return Ok(accountDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to update an Account.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id:int}")]
        public ActionResult<AccountDTO> Delete(int id)
        {
            if (id < byte.MinValue || id > byte.MaxValue)
            {
                return BadRequest($"ID must be between {byte.MinValue} and {byte.MaxValue}.");
            }

            try
            {
                var account = _accountRepo.GetAccountById(id);

                if (account == null)
                    return NotFound();

                _accountRepo.DeleteAccount(id);
                _accountRepo.Save();

                return Ok("Account deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to delete an Account.\nError: " + e.Message);
                return BadRequest();
            }
        }
    }
}