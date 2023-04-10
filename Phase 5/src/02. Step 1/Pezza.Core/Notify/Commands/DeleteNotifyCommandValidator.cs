namespace Core.Notify.Commands;

using FluentValidation;

public class DeleteNotifyCommandValidator : AbstractValidator<DeleteNotifyCommand>
{
    public DeleteNotifyCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
