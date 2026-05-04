namespace Core.Customer.Commands;

using FluentValidation;

public sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
