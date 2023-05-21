namespace Core.Customer.Commands;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
	public DeleteCustomerCommandValidator()
	{
		this.RuleFor(r => r.Id)
			.NotEmpty();
	}
}