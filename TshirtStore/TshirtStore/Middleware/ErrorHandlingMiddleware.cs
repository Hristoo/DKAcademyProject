using System.Net;
using Newtonsoft.Json;

namespace TshirtStore.Middleware
{
    public class ErrorHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "Application/json";

                switch (error)
                {
                    case AppExeption e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case CustomExeptionHandler ce:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonConvert.SerializeObject(new { message = error.Message });
                _logger.LogError(result);
                await response.WriteAsync(result);
            }
        }

    }
}
