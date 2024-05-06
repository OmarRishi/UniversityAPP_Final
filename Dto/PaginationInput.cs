using System.ComponentModel.DataAnnotations;

namespace UniversityAPP.Dto
{
    public class PaginationInput
    {
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int PageSize { get; set; } = 10;

        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int PageNumber { get; set; } = 1;

        internal int Skip => PageSize * (PageNumber - 1);
    }
}
