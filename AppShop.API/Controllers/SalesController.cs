using AppShop.API.Data;
using AppShop.API.Helper;
using AppShop.Share.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppShop.Share.Enums;
using AppShop.Share.Entities;

namespace AppShop.API 
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly IOrdersHelper _ordersHelper;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SalesController(IOrdersHelper ordersHelper, DataContext context, IUserHelper userHelper)
        {
            _ordersHelper = ordersHelper;
            _context = context;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<ActionResult> Post(SaleDTO saleDTO)
        {
            var response = await _ordersHelper.ProcessOrderAsync(User.Identity!.Name!, saleDTO.Remarks).ConfigureAwait(false);
            if (response.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name).ConfigureAwait(false);
            if (user is null) 
            {
                return BadRequest("User not valid.");
            }

            var queryable = _context.Sales
                .Include(s => s.User!)
                .Include(s => s.SaleDetails!)
                .ThenInclude(sd => sd.Product)
                .AsQueryable();

            var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Admin.ToString()).ConfigureAwait(false);
            if (!isAdmin)
            {
                queryable = queryable.Where(s => s.User!.Email == User.Identity!.Name);
            }

            return Ok(await queryable
                .OrderByDescending(x => x.Date)
                .Paginate(pagination)
                .ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name).ConfigureAwait(false);
            if (user is null)
            {
                return BadRequest("User not valid.");
            }

            var queryable = _context.Sales
                .AsQueryable();

            var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Admin.ToString()).ConfigureAwait(false);
            if (!isAdmin)
            {
                queryable = queryable.Where(s => s.User!.Email == User.Identity!.Name);
            }

            double count = await queryable.CountAsync().ConfigureAwait(false);
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.User!)
                .ThenInclude(u => u.City!)
                .ThenInclude(c => c.State!)
                .ThenInclude(s => s.Country)
                .Include(s => s.SaleDetails!)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        [HttpPut]
        public async Task<ActionResult> Put(SaleDTO saleDTO)
        {
            var user = await _userHelper.GetUserAsync(User.Identity!.Name!).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound();
            }

            var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Admin.ToString()).ConfigureAwait(false);
            if (!isAdmin)
            {
                return BadRequest("Only Admins are allowed.");
            }

            var sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .FirstOrDefaultAsync(s => s.Id == saleDTO.Id).ConfigureAwait(false);
            if (sale == null)
            {
                return NotFound();
            }

            if (saleDTO.OrderStatus == OrderStatus.Cancelled)
            {
                await ReturnStockAsync(sale).ConfigureAwait(false);
            }

            sale.OrderStatus = saleDTO.OrderStatus;
            _context.Update(sale);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Ok(sale);
        }

        private async Task ReturnStockAsync(Sale sale)
        {
            foreach (var saleDetail in sale.SaleDetails!)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == saleDetail.ProductId).ConfigureAwait(false);
                if (product != null)
                {
                    product.Stock += saleDetail.Quantity;
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

    }

}


