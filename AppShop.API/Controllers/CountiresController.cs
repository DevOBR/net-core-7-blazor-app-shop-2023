using AppShop.API.Data;
using AppShop.API.Helper;
using AppShop.Share.DTOs;
using AppShop.Share.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/countries")]
	public class CountiresController : ControllerBase
	{
        private readonly DataContext _context;

        public CountiresController(DataContext context)
		{
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            return Ok(await _context.Countries.ToListAsync().ConfigureAwait(false));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Countries
                .Include(x => x.States)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }


            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
                .ConfigureAwait(false));
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Countries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }


        [HttpGet("full")]
        public async Task<IActionResult> GetFullAsync()
        {
            return Ok(await _context.Countries
                .Include(x => x.States!)
                .ThenInclude(x => x.Cities)
                .ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {

            var country = await _context.Countries.Include(x => x.States!)
                .ThenInclude(x => x.Cities)
                .FirstAsync(x => x.Id == id).ConfigureAwait(false);

            if (country is null)
                return NotFound();


            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Country country)
        {
            try
            {
                await _context.Countries.AddAsync(country).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The {nameof(Country)} {country.Name} already exist.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Country country)
        {
            try
            {
                var countryResult = await _context.Countries.FirstAsync(x => x.Id == country.Id).ConfigureAwait(false);

                if (country is null)
                    return NotFound();

                countryResult.Name = country.Name;

                _context.Countries.Update(countryResult);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The country {country.Name} already exist.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {

            var country = await _context.Countries.FirstAsync(x => x.Id == id).ConfigureAwait(false);

            if (country is null)
                return NotFound();

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }
    }
}

