namespace UniversityAPP
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {

                _logger.LogInformation($"Request: {httpContext.Request.Method} {httpContext.Request.Path}");

                await _next(httpContext);

                // Log response details
                _logger.LogInformation($"Response: {httpContext.Response.StatusCode}");
            }
            catch (Exception ex)
            {

                _logger.LogError(message: $"Response: {httpContext.Response.StatusCode}{Environment.NewLine}{ex.Message}"); //Body: {responseBody}
                throw;
            }
        }
    }

    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
