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
    public class FiscalYearsController : ControllerBase
    {
        private readonly IFiscalYearRepository _fiscalYearRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<FiscalYearsController> _logger;

        public FiscalYearsController(IFiscalYearRepository fiscalYearRepo, 
                                     IMapper mapper, 
                                     ILogger<FiscalYearsController> logger)
        {
            _fiscalYearRepo = fiscalYearRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/FiscalYears?page=2&itemsPerPage=25
        [HttpGet]
        public async Task<ActionResult<FiscalYearListDTO>> GetFiscalYears([FromQuery] int page = 1, [FromQuery] int itemsPerPage = 20)
        {
            try
            {
                if (page < 1 || itemsPerPage < 1)
                    return BadRequest("Request contained one or more invalid paging values.");

                var fiscalYears = await _fiscalYearRepo.GetFiscalYears()
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .OrderBy(f => f.YearDescription)
                    .ToListAsync();

                var fiscalYearCount = await _fiscalYearRepo.GetFiscalYears().CountAsync();

                var fiscalYearList = _mapper.Map<List<FiscalYear>, List<FiscalYearDTO>>(fiscalYears);

                var dto = new FiscalYearListDTO
                {
                    FiscalYears = fiscalYearList,
                    TotalItems = fiscalYearCount,
                    TotalPages = decimal.ToInt32(Math.Ceiling((decimal)fiscalYearCount / (decimal)itemsPerPage)),
                    CurrentPage = page,
                    ItemsPerPage = itemsPerPage
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve Fiscal Years.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/FiscalYears/5
        [HttpGet("{id:int}")]
        public ActionResult<FiscalYearDTO> GetFiscalYear(int id)
        {
            try
            {
                if (id < byte.MinValue || id > byte.MaxValue)
                {
                    return BadRequest($"ID must be between {byte.MinValue} and {byte.MaxValue}.");
                }

                var fiscalYear = _fiscalYearRepo.GetFiscalYearById((byte)id);

                if (fiscalYear == null)
                    return NotFound();

                var fiscalYearDto = _mapper.Map<FiscalYearDTO>(fiscalYear);
                return fiscalYearDto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to retrieve a Fiscal Year.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // GET: api/FiscalYears/Lookup?year=2018
        [HttpGet("Lookup")]
        public ActionResult<FiscalYearDTO> LookupFiscalYear([FromQuery] int year)
        {
            try
            {
                var fiscalYear = _fiscalYearRepo.GetFiscalYear(year.ToString());

                if (fiscalYear == null)
                    return NotFound();

                var dto = _mapper.Map<FiscalYearDTO>(fiscalYear);

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while looking up a Fiscal Year.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // POST: api/FiscalYears
        [HttpPost]
        public ActionResult<FiscalYearDTO> Post([FromBody] FiscalYearDTO fiscalYearDto)
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
                    var fiscalYear = _mapper.Map<FiscalYear>(fiscalYearDto);

                    _fiscalYearRepo.AddFiscalYear(fiscalYear);
                    _fiscalYearRepo.Save();

                    return Created($"/api/FiscalYears/{fiscalYear.FiscalYearId}", fiscalYearDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to add a Fiscal Year.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // PUT: api/FiscalYears
        [HttpPut]
        public ActionResult<FiscalYearDTO> Put([FromBody] FiscalYearDTO fiscalYearDto)
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
                    var fiscalYear = _mapper.Map<FiscalYear>(fiscalYearDto);

                    _fiscalYearRepo.UpdateFiscalYear(fiscalYear);
                    _fiscalYearRepo.Save();

                    return Ok(fiscalYearDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to update a Fiscal Year.\nError: " + e.Message);
                return BadRequest();
            }
        }

        // DELETE: api/FiscalYears/5
        [HttpDelete("{id:int}")]
        public ActionResult<FiscalYearDTO> Delete(int id)
        {
            if (id < byte.MinValue || id > byte.MaxValue)
            {
                return BadRequest($"ID must be between {byte.MinValue} and {byte.MaxValue}.");
            }

            try
            {
                var fiscalYear = _fiscalYearRepo.GetFiscalYearById((byte)id);

                if (fiscalYear == null)
                    return NotFound();

                _fiscalYearRepo.DeleteFiscalYear((byte)id);
                _fiscalYearRepo.Save();

                return Ok("Fiscal Year deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred while attempting to delete a Fiscal Year.\nError: " + e.Message);
                return BadRequest();
            }
        }
    }
}