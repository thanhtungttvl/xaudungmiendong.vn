using System.ComponentModel.DataAnnotations;

namespace AppShare.Models.Auth
{
    public class AuthResetPasswordRequestModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
