using AutoMapper;
using DomainServices.Entities;
using UniversityAPP.Dto;

namespace UniversityAPP.Mapping
{
    public static class MyMapper//<TSource, TDestination>
    {
        public static IMapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, StudentDto>();
                cfg.CreateMap<StudentDto, Student>();
            });
            //var mapper = new Mapper(config);
            var mapper = config.CreateMapper();
            return mapper;
        }

    }
}
