using AppShop.API.Data;
using AppShop.Share.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API.Controllers
{
    [ApiController]
	[Route("/api/countries")]
	public class CountiresController : ControllerBase
	{
        private readonly DataContext _context;

        public CountiresController(DataContext context)
		{
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Countries.ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {

            var country = await _context.Countries.FirstAsync(x => x.Id == id).ConfigureAwait(false);

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
                    return BadRequest($"The country {country.Name} already exist.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Country country)
        {
            try
            {
                _context.Countries.Update(country);
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
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
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

