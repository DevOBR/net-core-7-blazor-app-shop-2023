using System;
using AppShop.API.Data;
using AppShop.Share.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API.Controllers
{
    [ApiController]
    [Route("/api/cities")]
    public class CitiesController : ControllerBase
	{
        private readonly DataContext _context;

        public CitiesController(DataContext context)
		{
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFullAsync()
        {
            return Ok(await _context.Cities.ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {

            var city = await _context.Cities.FirstAsync(x => x.Id == id).ConfigureAwait(false);

            if (city is null)
                return NotFound();


            return Ok(city);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] City city)
        {
            try
            {
                await _context.Cities.AddAsync(city).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The {nameof(City)} {city.Name} already exist.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] City city)
        {
            try
            {
                var cityResult = await _context.Cities.FirstAsync(x => x.Id == city.Id).ConfigureAwait(false);

                if (city is null)
                    return NotFound();

                cityResult.Name = city.Name;

                _context.Cities.Update(cityResult);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The {nameof(City)} {city.Name} already exist.");
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

            var city = await _context.Cities.FirstAsync(x => x.Id == id).ConfigureAwait(false);

            if (city is null)
                return NotFound();

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }
    }
}

