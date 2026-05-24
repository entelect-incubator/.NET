namespace Api.Handlers;

using System.Net;
using Microsoft.AspNetCore.Diagnostics;

/// <summary>
/// Global exception handler using the .NET 8+ IExceptionHandler pattern.
/// Provides centralized, dependency-injection-friendly exception handling for all unhandled exceptions.
/// </summary>
/// <remarks>
/// This handler is registered in the DI container and executed for any unhandled exceptions.
/// It provides a cleaner alternative to traditional middleware-based exception handling.
/// Multiple handlers can be registered and will execute in registration order.
/// </remarks>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
	private readonly ILogger<GlobalExceptionHandler> logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for exception logging</param>
	public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
	{
		this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	/// <summary>
	/// Attempts to handle an exception asynchronously.
	/// Logs the exception and returns a standardized error response.
	/// </summary>
	/// <param name="httpContext">The current HTTP context</param>
	/// <param name="exception">The exception that was thrown</param>
	/// <param name="cancellationToken">Cancellation token for the async operation</param>
	/// <returns>True if the exception was handled; false otherwise</returns>
	/// <remarks>
	/// This method is called by the ASP.NET Core runtime for any unhandled exceptions.
	/// It logs the error and writes a standardized JSON error response to the client.
	/// Supports cancellation via the cancellation token.
	/// </remarks>
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		// Log the exception with full context
		this.logger.LogError(
			exception,
			"Unhandled exception: {ExceptionType} - {Message}",
			exception.GetType().Name,
			exception.Message);

		// Set response status and content type
		httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		httpContext.Response.ContentType = "application/json";

		// Create error response object
		var errorResponse = new
		{
			statusCode = HttpStatusCode.InternalServerError,
			message = "An internal server error occurred. Please try again later.",
			errorId = httpContext.TraceIdentifier,
			exceptionType = exception.GetType().Name,
			details = httpContext.RequestServices
				.GetService<IWebHostEnvironment>()?
				.IsDevelopment() == true ? exception.Message : null
		};

		// Write response to client
		await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

		// Return true to indicate this exception was handled
		return true;
	}
}
