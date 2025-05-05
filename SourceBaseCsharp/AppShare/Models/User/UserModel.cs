namespace AppShare.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Tài khoản khách";
        public string Email { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public UserEnum.RoleOption Role { get; set; } = UserEnum.RoleOption.Guest;
        public UserEnum.StatusOption Status { get; set; } = UserEnum.StatusOption.Active;

        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
    }
}
