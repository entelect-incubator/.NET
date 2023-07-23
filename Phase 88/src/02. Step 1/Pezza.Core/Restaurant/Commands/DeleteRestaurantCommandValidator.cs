namespace Core.Restaurant.Commands;

using FluentValidation;

public class DeleteRestaurantCommandValidator : AbstractValidator<DeleteRestaurantCommand>
{
    public DeleteRestaurantCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
