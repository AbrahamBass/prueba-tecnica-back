using Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace PruebaTecnica.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = exception.Message;

            if (exception is ValidationException validationEx)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = string.Join(" ", validationEx.Errors.Select(e => e.ErrorMessage));
            }
            else
            {
                statusCode = exception switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    BadRequestException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };
            }

            var response = new
            {
                StatusCode = statusCode,
                Message = message, 
                Detailed = exception.InnerException?.Message
            };

            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
