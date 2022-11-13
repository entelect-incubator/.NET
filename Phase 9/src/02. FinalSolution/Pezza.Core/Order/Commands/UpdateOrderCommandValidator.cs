namespace Pezza.Core.Customer.Commands;

using FluentValidation;
using Pezza.Core.Order.Commands;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        this.RuleFor(r => r.Data)
            .NotNull();

        this.RuleFor(r => r.Data.Id)
            .NotEmpty();

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
            .NotEmpty()
            .When(x => x.Data?.Amount != null);

        this.RuleFor(r => r.Data.OrderItems)
            .NotEmpty()
            .When(x => x.Data?.OrderItems != null);

        this.RuleFor(r => r.Data.Completed)
            .NotEmpty()
            .When(x => x.Data?.Completed != null);
    }
}
