using AutoMapper;
using Common.DTO;
using KeepNotes.Domain.Identity;

namespace KeepNotes.Service
{
    public class SOProfile : Profile
    {
        public SOProfile()
        {
            CreateMap<User, UserDTO>(MemberList.None).ReverseMap();
            CreateMap<User, UserLoginDto>(MemberList.None).ReverseMap();
        }
    }
}