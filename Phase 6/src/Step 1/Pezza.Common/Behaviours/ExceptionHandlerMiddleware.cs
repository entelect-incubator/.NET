namespace Pezza.Common.Behaviours
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Pezza.Common.Models;

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
                Logging.Logging.LogException(ex);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception.GetType() == typeof(ValidationException))
            {
                var errors = ((ValidationException)exception).Errors;
                if (errors.Any())
                {
                    var failures = errors.Select(x =>
                    {
                        return new
                        {
                            Property = x.PropertyName.Replace("Data.", string.Empty),
                            Error = x.ErrorMessage.Replace("Data ", string.Empty)
                        };
                    });
                    var result = Result.Failure(failures.ToList<object>());
                    var code = HttpStatusCode.BadRequest;
                    var resultJson = JsonConvert.SerializeObject(result);

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;

                    return context.Response.WriteAsync(resultJson);
                }
                else
                {
                    var code = HttpStatusCode.BadRequest;
                    var result = Result.Failure(exception?.Message);
                    var resultJson = JsonConvert.SerializeObject(result);

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;

                    return context.Response.WriteAsync(resultJson);
                }
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