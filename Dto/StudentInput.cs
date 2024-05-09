using System.ComponentModel.DataAnnotations;

namespace UniversityAPP.Dto
{
    public class StudentInput
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Address { get; set; }
        public string? Mail { get; set; }
        public DateTime BirthDate { get; set; }
        public int Grade { get; set; }
        public string? Phone { get; set; }
    }
}