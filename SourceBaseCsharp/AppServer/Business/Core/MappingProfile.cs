using AppShare.Models.User;
using AutoMapper;
using AppServer.Business.Entities;

namespace AppServer.Business.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserModel>().ReverseMap();
        }
    }
}
