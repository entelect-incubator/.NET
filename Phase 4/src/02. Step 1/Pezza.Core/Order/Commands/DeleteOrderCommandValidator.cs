namespace Pezza.Core.Order.Commands
{
    using FluentValidation;

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            this.RuleFor(r => r.Id)
                .NotEmpty();
        }
    }
}
