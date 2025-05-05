using AppShare.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppServer.Business.Entities
{
    [Table("users")]
    public class UserEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Avatar { get; set; } = AppExtensions.Randomize(new List<string> { "images/user_girl.svg", "images/user_man.svg" });
        public string? Facebook { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; } = null;
        public string? About { get; set; } = string.Empty;
        public UserEnum.RoleOption Role { get; set; } = UserEnum.RoleOption.Guest;
        public UserEnum.StatusOption Status { get; set; } = UserEnum.StatusOption.Active;
    }
}
