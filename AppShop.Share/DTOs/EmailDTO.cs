using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AppShop.Share.DTOs
{
    public class EmailDTO
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "The field {0} is required.")]
        [EmailAddress(ErrorMessage = "You must enter a valid mail.")]
        public string Email { get; set; } = null!;
    }
}

