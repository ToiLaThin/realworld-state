using System.Net;

namespace Realworld.Api.Utils.ExceptionHandling
{
    public class ConduitException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public object Errors { get; set; }
        public ConduitException(HttpStatusCode statusCode, object errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}