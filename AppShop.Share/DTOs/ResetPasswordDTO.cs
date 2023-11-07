using System.ComponentModel.DataAnnotations;

namespace AppShop.Share.DTOs
{

    public class ResetPasswordDTO
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "You must need to enter a valid email")]
        [Required(ErrorMessage = "The field {0} is required")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characters.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "the new password and confirmation don't match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Comfirm password.")]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characters.")]
        public string ConfirmPassword { get; set; } = null!;

        public string Token { get; set; } = null!;
    }

}

