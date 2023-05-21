namespace Common.Behaviour;

using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
	private readonly ILogger<TRequest> logger;

	public UnhandledExceptionBehaviour(ILogger<TRequest> logger) => this.logger = logger;
	

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		try
		{
			return await next();
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		// Log issues and handle exception response
		if (exception.GetType() != typeof(ValidationException))
		{
			var code = HttpStatusCode.InternalServerError;
			var result = JsonConvert.SerializeObject(new { isSuccess = false, error = exception.Message });
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;

			return context.Response.WriteAsync(result);
		}
		else
		{
			var errors = ((ValidationException)exception).Errors;
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
	}
}
