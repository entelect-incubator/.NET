namespace Pezza.Api.Middleware
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next) => this.next = next;

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log issues and handle exception response
            if (exception.GetType() == typeof(ValidationException))
            {
                var code = HttpStatusCode.BadRequest;
                var errors = ((ValidationException)exception).Errors;
                var result = JsonConvert.SerializeObject(errors.Select(s =>
                {
                    return new
                    {
                        Field = s.PropertyName.Replace("Data.", string.Empty),
                        Error = s.ErrorMessage.Replace("Data.", string.Empty)
                    };
                }));
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);
            }
            else
            {
                var code = HttpStatusCode.InternalServerError;
                var result = JsonConvert.SerializeObject(new { isSuccess = false, error = exception.Message });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);
            }
        }
    }
}
