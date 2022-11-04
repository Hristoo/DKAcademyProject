using System.Net;

namespace ThirtStore.Models.Models.Responses
{
    public class BaseResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public string Message { get; set; }
    }
}
