namespace Core.Behaviours;

using FluentValidation;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> validators = validators;

	public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
	{
		if (this.validators.Any())
		{
			var context = new ValidationContext<TRequest>(request);

			var validationResults = await Task.WhenAll(this.validators.Select(v => v.ValidateAsync(context, cancellationToken)));
			var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null);

			if (failures.Any())
			{
				throw new ValidationException(failures);
			}
		}

		return await next();
	}
}
