namespace Core.Notify.Commands;

using FluentValidation;

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
