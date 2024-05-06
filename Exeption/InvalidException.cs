namespace UniversityAPP.Exeption
{
    public class InvalidException : Exception
    {
        public InvalidException() { }
        public InvalidException(string message) : base(message) { }
        public InvalidException(string message, Exception inner) : base(message, inner) { }

    }
    public class InvalidAgeException : Exception
    {
        public int Age { get; }
        public InvalidAgeException(int age)
        {
            Age = age;
            base.Data.Add("Age", Age);
        }
    }
}
