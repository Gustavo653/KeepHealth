using AutoMapper;
using Common.DTO;
using KeepNotes.Domain.Identity;

namespace KeepNotes.Service
{
    public class KeepNotesProfile : Profile
    {
        public KeepNotesProfile()
        {
            CreateMap<User, UserDTO>(MemberList.None).ReverseMap();
            CreateMap<User, UserLoginDto>(MemberList.None).ReverseMap();
        }
    }
}