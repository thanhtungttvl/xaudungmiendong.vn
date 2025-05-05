using System.ComponentModel.DataAnnotations;

namespace AppShare.Models.Auth
{
    public class AuthRegisterRequestModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
