namespace UniversityAPP.Utilities
{
    public class InvalidException : Exception
    {
        public InvalidException() { }
        public InvalidException(string message) : base(message) { }
        public InvalidException(string message, Exception Inner) : base(message, Inner) { }
    }

}
