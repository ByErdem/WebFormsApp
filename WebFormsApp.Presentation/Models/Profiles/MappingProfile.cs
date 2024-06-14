using AutoMapper;
using System.Collections.Generic;
using WebFormsApp.Data;
using WebFormsApp.Entity.Dtos;

namespace WebFormsApp.Presentation.Models.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Students, StudentDto>().ReverseMap();
            CreateMap<StudentDto, Students>().ReverseMap();

            CreateMap<List<Students>, List<StudentDto>>().ReverseMap();
            CreateMap<List<StudentDto>, List<Students>>().ReverseMap();
        }
    }
}