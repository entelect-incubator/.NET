namespace Common.Helpers;

using FluentValidation;

/// <summary>
/// Helper class for validating commands/queries using FluentValidation.
/// Handlers can use this to validate requests before processing business logic.
/// </summary>
/// <remarks>
/// This replaces the old MediatR IPipelineBehavior approach with a more explicit,
/// handler-level validation pattern. Handlers call Validate() and exceptions are
/// caught by ValidationExceptionHandler in the exception handling pipeline.
/// </remarks>
public static class ValidationHelper
{
	/// <summary>
	/// Validates a request using provided validators.
	/// Throws ValidationException if validation fails.
	/// </summary>
	/// <typeparam name="TRequest">The type of request being validated</typeparam>
	/// <param name="request">The request to validate</param>
	/// <param name="validators">Collection of validators for this request type</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public static async Task ValidateAsync<TRequest>(
		TRequest request,
		IEnumerable<IValidator<TRequest>> validators,
		CancellationToken cancellationToken = default)
	{
		if (validators == null || !validators.Any())
		{
			return;
		}

		var context = new ValidationContext<TRequest>(request);
		var validationResults = await Task.WhenAll(
			validators.Select(v => v.ValidateAsync(context, cancellationToken)));

		var failures = validationResults
			.SelectMany(r => r.Errors)
			.Where(f => f != null)
			.ToList();

		if (failures.Any())
		{
			throw new ValidationException(failures);
		}
	}
}
