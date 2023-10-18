using System.ComponentModel.DataAnnotations;

namespace AppShop.Share.DTOs
{
    public class ChangePasswordDTO
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characteres.")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string CurrentPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characteres.")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string NewPassword { get; set; } = null!;

        [Compare("NewPassword", ErrorMessage = "The new password and the confrimation are no equals.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characteres.")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string Confirm { get; set; } = null!;
    }

}

