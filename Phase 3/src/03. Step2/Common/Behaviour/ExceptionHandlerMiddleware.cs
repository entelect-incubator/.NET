namespace Common.Behaviour;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

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
		if (exception.GetType() == typeof(FluentValidation.ValidationException))
		{
			var errors = ((FluentValidation.ValidationException)exception).Errors;
			if (errors.Any())
			{
				var failures = errors.Select(x =>
				{
					return new
					{
						Property = x.PropertyName.Replace("Data.", ""),
						Error = x.ErrorMessage.Replace("Data ", "")
					};
				});
				var result = Result.Failure(failures.ToList<object>());
				var code = HttpStatusCode.BadRequest;
				var resultJson = JsonSerializer.Serialize(result);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)code;

				return context.Response.WriteAsync(resultJson);
			}
			else
			{
				var code = HttpStatusCode.BadRequest;
				var result = Result.Failure(exception?.Message);
				var resultJson = JsonSerializer.Serialize(result);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)code;

				return context.Response.WriteAsync(resultJson);
			}
		}
		else
		{
			var code = HttpStatusCode.InternalServerError;
			var result = JsonSerializer.Serialize(new { isSuccess = false, error = exception.Message });
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;

			return context.Response.WriteAsync(result);
		}
	}
}
