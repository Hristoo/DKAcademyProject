namespace TshirtStore.Middleware
{
    public class CustomMiddlewareErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomMiddlewareErrorHandler> _logger;

        public CustomMiddlewareErrorHandler(ILogger<CustomMiddlewareErrorHandler> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogError($"HD *************** Error in {context.Request.Method}");
            await _next(context);
        }
    }
}
