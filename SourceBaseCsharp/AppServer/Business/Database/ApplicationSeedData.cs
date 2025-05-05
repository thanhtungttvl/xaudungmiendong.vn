using AppServer.Business.Entities;
using AppShare.Core;

namespace AppServer.Business.Database
{
    public class ApplicationSeedData
    {
        public static readonly List<UserEntity> Users = new()
        {
            new UserEntity { 
                Id = Guid.NewGuid(),
                Avatar = "images/user_man.svg",
                Name = "Tùng Đẹp Trai",
                Email = "thanhtungttvl@gmail.com",
                Password = "123123",
                Role = UserEnum.RoleOption.User,
                Status = UserEnum.StatusOption.Active,
                Birthday = new DateTime(1991, 3,15),
                Facebook = "https://www.facebook.com/thanhtungttvl",
                Phone = "0353 522 796",
                About = "TTVL"
            },
            new UserEntity {
                Id = Guid.NewGuid(),
                Avatar = AppExtensions.Randomize(new List<string> { "images/user_girl.svg", "images/user_man.svg" }),
                Name = "Tèo Téo teo",
                Email = "teo@gmail.com",
                Password = "123123",
                Role = UserEnum.RoleOption.User,
            },
            new UserEntity {
                Id = Guid.NewGuid(),
                Avatar = AppExtensions.Randomize(new List<string> { "images/user_girl.svg", "images/user_man.svg" }),
                Name = "Bành Thị Heo",
                Email = "heo@gmail.com",
                Password = "123123",
                Role = UserEnum.RoleOption.User,
            },
            new UserEntity {
                Id = Guid.NewGuid(),
                Avatar = AppExtensions.Randomize(new List<string> { "images/user_girl.svg", "images/user_man.svg" }),
                Name = "Trần Trùi Trụi",
                Email = "trui@gmail.com",
                Password = "123123",
                Role = UserEnum.RoleOption.User,
            },
            new UserEntity {
                Id = Guid.NewGuid(),
                Avatar = AppExtensions.Randomize(new List<string> { "images/user_girl.svg", "images/user_man.svg" }),
                Name = "Tao là test chơi thôi",
                Email = "user@example.com",
                Password = "string",
                Role = UserEnum.RoleOption.User,
            },
        };
    }
}
