using AppShop.Share.Enums;

namespace AppShop.Share.DTOs
{
    public class SaleDTO
    {
        public int Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string Remarks { get; set; } = string.Empty;
    }

}

