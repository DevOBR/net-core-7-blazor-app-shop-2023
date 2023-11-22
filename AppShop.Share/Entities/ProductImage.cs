using System.ComponentModel.DataAnnotations;

namespace AppShop.Share.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; } = null!;

        public int ProductId { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; } = null!;
    }

}

