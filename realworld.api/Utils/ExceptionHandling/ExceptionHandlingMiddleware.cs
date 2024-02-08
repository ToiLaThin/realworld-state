using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Realworld.Api.Utils.ExceptionHandling {
    public class ExceptionHandlingMiddleware {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            } 
            catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception) {
            string? result = null;
            switch (exception) {
                case ConduitException conduitEx:
                    context.Response.StatusCode = (int)conduitEx.StatusCode;
                    result = JsonSerializer.Serialize(new { errors = conduitEx.Errors });
                    break;
                case Exception ex:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { errors = new { message = ex.Message } });
                    break;
                default:
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(result ?? "{}");
        }
    }
}