namespace Core.Customer.Commands;

public sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
	public DeleteCustomerCommandValidator()
	{
		this.RuleFor(r => r.Id)
			.NotEmpty();
	}
}