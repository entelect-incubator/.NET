namespace Common.Behaviours;

using ValidationException = FluentValidation.ValidationException;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		=> this.validators = validators;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
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