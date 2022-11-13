namespace Pezza.Core.Customer.Commands;

using FluentValidation;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
