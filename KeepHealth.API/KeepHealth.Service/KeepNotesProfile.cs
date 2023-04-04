using AutoMapper;
using Common.DTO;
using KeepHealth.Domain;
using KeepHealth.Domain.Identity;

namespace KeepHealth.Service
{
    public class KeepHealthProfile : Profile
    {
        public KeepHealthProfile()
        {
            CreateMap<User, UserDTO>(MemberList.None).ReverseMap();
            CreateMap<User, UserLoginDTO>(MemberList.None).ReverseMap();
            CreateMap<MedicalCondition, CreateMedicalDTO>(MemberList.None).ReverseMap();
            CreateMap<MedicalSpeciality, CreateMedicalDTO>(MemberList.None).ReverseMap();
        }
    }
}