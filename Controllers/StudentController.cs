using AutoMapper;
using DomainServices.Entities;
using DomainServices.Interface;
using DomainServices.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAPP.Dto;
using UniversityAPP.Exeption;
using UniversityAPP.Mapping;

namespace UniversityAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IStudentManager _studentManager;
        private readonly IMapper mapper = MyMapper.InitializeAutoMapper();

        public StudentController(IStudentRepository repository, IStudentManager studentManager)
        {
            _repository = repository;
            _studentManager = studentManager;
        }

        [HttpGet]
        public async Task<PaginationResult<StudentDto>> GetAll([FromQuery] GetAllStudentsFilter studentsFilter)
        {
            var items = await _repository.GetAllStudents(studentsFilter.Name, studentsFilter.Address, studentsFilter.Skip, studentsFilter.PageSize);


            var EntityDTO = mapper.Map<List<StudentDto>>(items);


            return new PaginationResult<StudentDto>
            {
                Items = EntityDTO,
                TotalCount = await _repository.GetCountAsync(studentsFilter.Name, studentsFilter.Address)
            };
        }

        [HttpGet("GetByID")]
        public async Task<StudentDto> GetByID(int id)
        {
            var res = await _repository.GetByIDAsync(id);
            if (res == null)
                throw new InvalidException("ID Not Found");

            return mapper.Map<StudentDto>(res);
        }

        [HttpGet("GetByName")]
        public async Task<StudentDto> GetByName(string Name)
        {
            var res = await _repository.GetByNameAsync(Name);
            if (res == null)
                throw new InvalidException("Name Not Found");

            return mapper.Map<StudentDto>(res);
        }

        [HttpPost]
        public IActionResult AddStudent(StudentDto student)
        {
            if (ModelState.IsValid)
            {
                if (student is null)
                    return BadRequest("No Data");

                var mapper = MyMapper.InitializeAutoMapper();
                var std = mapper.Map<Student>(student);

                _repository.Add(std);
                return Ok("Data Saved");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _studentManager.Delete(id);
        }
    }
}
