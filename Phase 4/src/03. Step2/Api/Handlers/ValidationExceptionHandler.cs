namespace Api.Handlers;

using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

/// <summary>
/// Specialized exception handler for FluentValidation validation failures.
/// Extends the IExceptionHandler pattern to provide validation-specific error formatting.
/// </summary>
/// <remarks>
/// This handler should be registered BEFORE GlobalExceptionHandler so validation errors
/// are formatted with detailed field-level information before falling back to generic handling.
/// 
/// Registration order matters:
/// services.AddExceptionHandler<ValidationExceptionHandler>();
/// services.AddExceptionHandler<GlobalExceptionHandler>();
/// </remarks>
public sealed class ValidationExceptionHandler : IExceptionHandler
{
	private readonly ILogger<ValidationExceptionHandler> logger;

	/// <summary>
	/// Initializes a new instance of the ValidationExceptionHandler class.
	/// </summary>
	/// <param name="logger">Logger instance for validation error logging</param>
	public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
	{
		this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	/// <summary>
	/// Handles FluentValidation ValidationException specifically.
	/// Returns 400 Bad Request with detailed field-level validation errors.
	/// </summary>
	/// <param name="httpContext">The current HTTP context</param>
	/// <param name="exception">The exception that was thrown</param>
	/// <param name="cancellationToken">Cancellation token for the async operation</param>
	/// <returns>True if this is a ValidationException; false otherwise</returns>
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		// Only handle FluentValidation ValidationException
		if (exception is not ValidationException validationException)
		{
			return false;
		}

		// Log validation failure (not as an error, but as expected validation failure)
		if (this.logger.IsEnabled(LogLevel.Warning))
		{
			this.logger.LogWarning(
				"Validation failed for request {TraceId}: {FailureCount} errors",
				httpContext.TraceIdentifier,
				validationException.Errors.Count());
		}

		// Set response status and content type
		httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		httpContext.Response.ContentType = "application/json";

		// Extract field-level validation errors
		var validationErrors = validationException.Errors
			.GroupBy(x => x.PropertyName)
			.ToDictionary(
				g => g.Key,
				g => g.Select(x => x.ErrorMessage).ToArray());

		// Create standardized Problem Details response (RFC 7231)
		var problemDetails = new
		{
			type = "https://api.pezza.com/docs/errors/validation-failed",
			title = "Validation Failed",
			status = HttpStatusCode.BadRequest,
			detail = "One or more validation errors occurred.",
			traceId = httpContext.TraceIdentifier,
			errors = validationErrors
		};

		// Write response to client
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		// Return true to indicate this exception was handled
		return true;
	}
}
