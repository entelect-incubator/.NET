namespace Core.Behaviours;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

public class UnhandledExceptionBehaviour(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			if (httpContext.Request.Method == HttpMethods.Post ||
			httpContext.Request.Method == HttpMethods.Put ||
			httpContext.Request.Method == HttpMethods.Patch)
			{
				var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();

				// Check if body is empty or contains empty JSON "{}"
				if (string.IsNullOrWhiteSpace(requestBody) || requestBody.Trim() == "{}")
				{
					httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
					await httpContext.Response.WriteAsync("Request body cannot be empty or an empty JSON object.");
					return;
				}

				var memoryStream = new MemoryStream();
				var writer = new StreamWriter(memoryStream);
				await writer.WriteAsync(requestBody);
				await writer.FlushAsync();
				memoryStream.Position = 0;
				httpContext.Request.Body = memoryStream;
			}

			await next(httpContext);
		}
		catch (ValidationException ex)
		{
			await HandleValidationExceptionAsync(httpContext, ex);
		}
		catch (Exception ex)
		{
			await HandleUnhandledExceptionAsync(httpContext, ex);
		}
	}

	private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
	{
		var errors = ((ValidationException)exception).Errors;
		if (errors.Any())
		{
			var failures = errors.Select(x => $"{x.PropertyName.Replace("Data.", "")}:{x.ErrorMessage.Replace("Data ", "")}"
);
			var result = Result.Failure(failures.ToList());
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

	private static Task HandleUnhandledExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;

		var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
		return context.Response.WriteAsync(result);
	}
}