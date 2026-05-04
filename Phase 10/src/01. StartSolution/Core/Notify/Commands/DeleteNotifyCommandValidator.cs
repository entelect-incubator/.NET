namespace Core.Notify.Commands;

using FluentValidation;

public sealed class DeleteNotifyCommandValidator : AbstractValidator<DeleteNotifyCommand>
{
    public DeleteNotifyCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
