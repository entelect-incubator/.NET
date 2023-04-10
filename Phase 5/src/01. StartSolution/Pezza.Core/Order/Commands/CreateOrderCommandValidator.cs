namespace Core.Order.Commands;

using FluentValidation;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        this.RuleFor(r => r.Data.CustomerId)
            .NotNull()
            .When(x => x.Data?.CustomerId != null);

        this.RuleFor(r => r.Data.Customer)
            .NotNull()
            .When(x => x.Data?.Customer != null);

        this.RuleFor(r => r.Data.RestaurantId)
            .NotNull()
            .When(x => x.Data?.RestaurantId != null);

        this.RuleFor(r => r.Data.Amount)
            .NotEmpty();

        this.RuleFor(r => r.Data.OrderItems)
            .NotEmpty();
    }
}
