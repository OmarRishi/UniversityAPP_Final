using AutoMapper;
using DomainServices.Entities;
using UniversityAPP.Dto;

namespace UniversityAPP.Mapping
{
    public static class MyMapper
    {
        public static IMapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, StudentDto>();
                cfg.CreateMap<StudentDto, Student>();

                cfg.CreateMap<StudentInput, Student>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.DeleteTime, opt => opt.Ignore())
                 .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
            });
            var mapper = config.CreateMapper();
            return mapper;
        }

    }
}
