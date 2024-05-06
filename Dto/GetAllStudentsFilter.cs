using System.ComponentModel.DataAnnotations;

namespace UniversityAPP.Dto
{
    public class GetAllStudentsFilter : PaginationInput
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }
    }
}