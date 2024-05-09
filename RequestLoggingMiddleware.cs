using System.Text;

namespace UniversityAPP
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {

                // Log request details
                var requestBody = await ReadRequestBodyAsync(context.Request);
                _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} Body: {requestBody}");

                // Continue processing the request
                await next(context);

                // Log response details
                var responseBody = await ReadResponseBodyAsync(context.Response);
                _logger.LogInformation($"Response: {context.Response.StatusCode} Body: {responseBody}");
            }
            catch (Exception ex)
            {

                //_logger.LogError($"Response: {context.Response.StatusCode} Body: {responseBody}");
                throw;
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {  
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            var originalBodyStream = response.Body;
            using var memoryStream = new MemoryStream();
            response.Body = memoryStream;

            await response.Body.CopyToAsync(originalBodyStream);
            response.Body = originalBodyStream;

            memoryStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(memoryStream, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }
    }
}
