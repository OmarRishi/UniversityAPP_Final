namespace UniversityAPP.Utilities
{
    public class InvalidException : Exception
    {

        public InvalidException(ILogger logger)
        {

        }
        public InvalidException(ILogger logger, string message) : base(message)
        {
            logger.LogError(message);
        }
        public InvalidException(ILogger logger, string message, Exception Inner) : base(message, Inner)
        {
            logger.LogError(message);
        }
    }

}
