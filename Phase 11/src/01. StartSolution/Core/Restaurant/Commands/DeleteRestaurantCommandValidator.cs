namespace Core.Restaurant.Commands;

using FluentValidation;

public sealed class DeleteRestaurantCommandValidator : AbstractValidator<DeleteRestaurantCommand>
{
    public DeleteRestaurantCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
