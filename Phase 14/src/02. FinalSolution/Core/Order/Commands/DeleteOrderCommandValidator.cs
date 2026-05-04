namespace Core.Order.Commands;

using FluentValidation;

public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
