using AutoMapper;
using Common.DTO;
using KeepHealth.Domain.Identity;

namespace KeepHealth.Service
{
    public class KeepHealthProfile : Profile
    {
        public KeepHealthProfile()
        {
            CreateMap<User, UserDTO>(MemberList.None).ReverseMap();
            CreateMap<User, UserLoginDTO>(MemberList.None).ReverseMap();
        }
    }
}