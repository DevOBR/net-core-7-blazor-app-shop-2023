using System;
using AppShop.API.Data;
using AppShop.API.Helper;
using AppShop.Share.DTOs;
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
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Cities
                .Where(x => x.State!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }


        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Cities
                .Where(x => x.State!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
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

