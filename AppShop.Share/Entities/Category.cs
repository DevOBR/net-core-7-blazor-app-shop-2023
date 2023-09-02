using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AppShop.Share.Entities
{
    public class Category
	{
        public int Id { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(100, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        public string Name { get; set; } = null!;
    }
}

