namespace Core.Customer.Commands;

using FluentValidation;
using Core.Notify.Commands;

public class CreateNotifyCommandValidator : AbstractValidator<CreateNotifyCommand>
{
    public CreateNotifyCommandValidator()
    {
        this.RuleFor(r => r.Data.CustomerId)
            .NotNull();

        this.RuleFor(r => r.Data.Email)
            .NotEmpty();
    }
}
