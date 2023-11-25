using AppShop.API.Data;
using AppShop.Share.DTOs;
using AppShop.Share.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/temporalSales")]
    public class TemporalSalesController : ControllerBase
    {
        private readonly DataContext _context;

        public TemporalSalesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post(TemporalSaleDTO temporalSaleDTO)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == temporalSaleDTO.ProductId).ConfigureAwait(false);
            if (product is null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name).ConfigureAwait(false);
            if (user is null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = new()
            {
                Product = product,
                Quantity = temporalSaleDTO.Quantity,
                Remarks = temporalSaleDTO.Remarks,
                User = user
            };

            try
            {
                _context.Add(temporalSale);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(temporalSaleDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.TemporalSales
                .Include(ts => ts.User!)
                .Include(ts => ts.Product!)
                .ThenInclude(p => p.ProductCategories!)
                .ThenInclude(pc => pc.Category)
                .Include(ts => ts.Product!)
                .ThenInclude(p => p.ProductImages)
                .Where(x => x.User!.Email == User.Identity!.Name)
                .ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("count")]
        public async Task<ActionResult> GetCount()
        {
            return Ok(await _context.TemporalSales
                .Where(x => x.User!.Email == User.Identity!.Name)
                .SumAsync(x => x.Quantity).ConfigureAwait(false));
        }
    }
}
