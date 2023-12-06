using System.ComponentModel.DataAnnotations;

namespace AppShop.Share.Entities 
{
    public class SaleDetail
    {
        public int Id { get; set; }

        public Sale? Sale { get; set; }

        public int SaleId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remakrs")]
        public string? Remarks { get; set; }

        public Product? Product { get; set; }

        public int ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Value")]
        public decimal Value => Product == null ? 0 : (decimal)Quantity * Product.Price;
    }
}

