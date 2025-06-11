namespace App.Qtech.Web.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var sanitizedPath = context.Request.Path.Value?.Replace("\n", "").Replace("\r", "");
            _logger.LogInformation("Handling request: {Method} {Path}",
                context.Request.Method, sanitizedPath);

            await _next(context);

            _logger.LogInformation("Finished handling request: {Method} {Path}",
                context.Request.Method, context.Request.Path);
        }
    }
}
