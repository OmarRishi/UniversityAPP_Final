namespace UniversityAPP.Dto
{
    public class PaginationResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}
