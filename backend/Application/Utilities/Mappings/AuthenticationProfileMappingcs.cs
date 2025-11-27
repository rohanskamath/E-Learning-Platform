using AutoMapper;
using backend.Application.DTOs.AuthenticationDTOs;
using backend.Domain.Entities;

namespace backend.Application.Utilities.Mappings
{
    public class AuthenticationProfileMappingcs:Profile
    {
        public AuthenticationProfileMappingcs()
        {
             CreateMap<Student, StudentDto>().ReverseMap();
             CreateMap<Teacher, TeacherDto>().ReverseMap();
        }
    }
}
