using System.Text.Json;
using Serilog;

namespace API
{
    public sealed class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unhandled API error for {Method} {Path}", context.Request.Method, context.Request.Path);
                if (context.Response.HasStarted)
                    throw;

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "L?i h? th?ng." }));
            }
        }
    }
}
