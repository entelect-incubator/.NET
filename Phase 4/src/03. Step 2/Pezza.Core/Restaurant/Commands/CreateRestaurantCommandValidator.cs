namespace Core.Restaurant.Commands;

using FluentValidation;
using Common.Behaviours.Validators;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        this.RuleFor(r => r.Data.Name)
            .MaximumLength(100)
            .NotEmpty();

        this.RuleFor(r => r.Data.Description)
            .MaximumLength(1000)
            .NotEmpty()
            .When(x => x.Data?.Description != null);

        this.RuleFor(r => r.Data.PictureUrl)
            .MaximumLength(1000)
            .NotEmpty()
            .When(x => x.Data?.PictureUrl != null);

        this.RuleFor(r => r.Data.Address)
            .NotNull()
            .SetValidator(new AddressValidator());
    }
}
