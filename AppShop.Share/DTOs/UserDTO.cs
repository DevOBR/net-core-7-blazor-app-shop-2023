using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using AppShop.Share.Entities;

namespace AppShop.Share.DTOs
{
    public class UserDTO : User
	{
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "The gield {0} is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must hav between {2} and {1} characteres.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "The password and confirmation are not equal.")]
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characteres.")]
        public string PasswordConfirm { get; set; } = null!;
    }
}

