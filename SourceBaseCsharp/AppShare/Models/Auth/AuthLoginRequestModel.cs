using System.ComponentModel.DataAnnotations;

namespace AppShare.Models.Auth
{
    public class AuthLoginRequestModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
