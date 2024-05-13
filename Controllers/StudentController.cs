using AutoMapper;
using DomainServices.Entities;
using DomainServices.Interface;
using DomainServices.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityAPP.Dto;
using UniversityAPP.Mapping;
using UniversityAPP.Utilities;

namespace UniversityAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IStudentManager _studentManager;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepository repository,
                                 IStudentManager studentManager,
                                 IMapper mapper,
                                 ILogger<StudentController> logger)
        {
            _repository = repository;
            _studentManager = studentManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<PaginationResult<StudentDto>> GetAll([FromQuery] GetAllStudentsFilter studentsFilter)
        {
            var items = await _repository.GetAllStudents(studentsFilter.Name, studentsFilter.Address, studentsFilter.Skip, studentsFilter.PageSize);

            var EntityDTO = _mapper.Map<List<StudentDto>>(items);

            return new PaginationResult<StudentDto>
            {
                Items = EntityDTO,
                TotalCount = await _repository.GetCountAsync(studentsFilter.Name, studentsFilter.Address)
            };
        }

        [HttpGet("GetByID")]
        public async Task<StudentDto> GetByID(int ID)
        {
            var res = await _repository.GetByIDAsync(ID);
            if (res == null)
                throw new InvalidException("ID Not Found");

            return _mapper.Map<StudentDto>(res);
        }

        [HttpGet("GetByName")]
        public async Task<StudentDto> GetByName(string Name)
        {
            var res = await _repository.GetByNameAsync(Name);
            if (res == null)
                throw new InvalidException("Name Not Found");

            return _mapper.Map<StudentDto>(res);
        }

        [HttpPost]
        public async Task Add(StudentInput student)
        {
            if (student is null)
                throw new InvalidException("No Data");

            var std = _mapper.Map<StudentInput, Student>(student);
            await _studentManager.AddStudent(std);
        }

        [HttpPut]
        public async Task Update(int ID, StudentInput student)
        {
            if (student is null)
                throw new InvalidException(_logger, "No Data");

            var Old = await _repository.GetByIDAsync(ID);

            if (Old == null)
                throw new InvalidException("ID Not Found");

            var std = _mapper.Map(student, Old);

            await _studentManager.UpdateStudent(std);
        }

        [HttpDelete]
        public void Delete(int ID)
        {
            var student = _repository.GetByID(ID);
            if (student == null)
                throw new InvalidException("ID Not Found");

            _repository.Remove(student);
        }
    }
}
