namespace UniversityAPP.Dto
{
    public class JwtData
    {
        public string? ValidIssuer { get; set; }
        public string? ValidAudiance { get; set; }

        public string? SecretyKey { get; set; }
    }
}
