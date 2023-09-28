using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AppShop.Share.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "The field {0} is required.")]
        [EmailAddress(ErrorMessage = "You must enter a valid mail.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The field {0} is requred.")]
        [MinLength(6, ErrorMessage = "The field {0} must have less than {1} characteres.")]
        public string Password { get; set; } = null!;
    }
}

