namespace Common.Behaviour;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
	public async Task Invoke(HttpContext context /* other dependencies */)
	{
		try
		{
			await next(context);
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
				var failures = errors
					.GroupBy(e => e.PropertyName.Replace("Data.", string.Empty))
					.ToDictionary(
						g => g.Key,
						g => g.Select(e
							=> e.ErrorMessage.Replace("Data ", string.Empty)).ToList());
				var result = Result.ValidationFailure(failures);
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
