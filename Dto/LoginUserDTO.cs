using System.ComponentModel.DataAnnotations;

namespace UniversityAPP.Dto
{
    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
