using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppShop.Share.Entities
{
    public class Product
	{
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string Name { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [MaxLength(500, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        public string Description { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Inventory")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public float Stock { get; set; }

        public ICollection<ProductCategory>? ProductCategories { get; set; }

        [Display(Name = "Categories")]
        public int ProductCategoriesNumber => ProductCategories == null ? 0 : ProductCategories.Count;

        public ICollection<ProductImage>? ProductImages { get; set; }

        [Display(Name = "Images")]
        public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        [Display(Name = "Image")]
        public string MainImage => ProductImages == null ? string.Empty : ProductImages.FirstOrDefault()!.Image;


    }
}

