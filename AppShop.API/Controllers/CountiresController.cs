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
            return Ok(await _context.Countries.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Country country)
        {
            await _context.Countries.AddAsync(country).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Ok(country);
        }
    }
}

