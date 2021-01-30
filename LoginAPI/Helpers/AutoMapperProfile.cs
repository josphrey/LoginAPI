using AutoMapper;
using LoginAPI.Entities;
using LoginAPI.Model.Users;

namespace LoginAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Users, UserModel>();
            CreateMap<UserModel, Users>();
            CreateMap<RegisterUserModel, Users>();
            CreateMap<UpdateUserModel, Users>();
        }
    }
}
