using AppShop.API.Data;
using AppShop.Share.Entities;
using AppShop.Share.Enums;
using AppShop.Share.Reponses;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API 
{
    public class OrdersHelper : IOrdersHelper
    {
        private readonly DataContext _context;

        public OrdersHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Response> ProcessOrderAsync(string email, string remarks)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
            if (user == null) 
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "User not valid"
                };
            }

            var temporalSales = await _context.TemporalSales
                .Include(x => x.Product)
                .Where(x => x.User!.Email == email)
                .ToListAsync().ConfigureAwait(false);

            Response response = await CheckInventoryAsync(temporalSales).ConfigureAwait(false);

            if (!response.IsSuccess)
            {
                return response;
            }

            Sale sale = new()
            {
                Date = DateTime.UtcNow,
                User = user,
                Remarks =remarks,
                SaleDetails = new List<SaleDetail>(),
                OrderStatus = OrderStatus.New
            };

            foreach (var temporalSale in temporalSales)
            {
                sale.SaleDetails.Add(new SaleDetail
                {
                    Product = temporalSale.Product,
                    Quantity = temporalSale.Quantity,
                    Remarks = temporalSale.Remarks,
                });

                Product? product = await _context.Products.FindAsync(temporalSale.Product!.Id).ConfigureAwait(false);
                if (product != null)
                {
                    product.Stock -= temporalSale.Quantity;
                    _context.Products.Update(product);
                }

                _context.TemporalSales.Remove(temporalSale);
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return response;
        }

        private async Task<Response> CheckInventoryAsync(List<TemporalSale> temporalSales)
        {
            Response response = new() { IsSuccess = true };
            foreach (var temporalSale in temporalSales)
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == temporalSale.Product!.Id).ConfigureAwait(false);
                if (product == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"The product {temporalSale.Product!.Name}, is no longer available";
                    return response;
                }

                if (product.Stock < temporalSale.Quantity)
                {
                    response.IsSuccess = false;
                    response.Message = $"Sorry we don't have enough of this product {temporalSale.Product!.Name}, please reduce the amount of products or select another product to continue with the order.";
                    return response;
                }
            }
            return response;
        }
    }

}
