namespace Common.Behaviour;

using System.Net;
using System.Text.Json;
using Common.Models;
using Microsoft.AspNetCore.Http;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
	private readonly RequestDelegate next = next;

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await this.next(context);
		}
		catch (Exception exception)
		{
			await HandleExceptionAsync(context, exception);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		var result = Result.Failure(exception.Message);
		var resultJson = JsonSerializer.Serialize(result);

		return context.Response.WriteAsync(resultJson);
	}
}
