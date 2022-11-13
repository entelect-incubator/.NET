namespace Pezza.Core.Customer.Commands;

using FluentValidation;
using Pezza.Common.Validators;
using Pezza.Core.Restaurant.Commands;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        this.RuleFor(r => r.Data)
            .NotNull();

        this.RuleFor(r => r.Data.Id)
            .NotEmpty();

        this.RuleFor(r => r.Data.Name)
             .MaximumLength(100)
             .NotEmpty()
             .When(x => x.Data?.Name != null);

        this.RuleFor(r => r.Data.Description)
            .MaximumLength(1000)
            .NotEmpty()
            .When(x => x.Data?.Description != null);

        this.RuleFor(r => r.Data.PictureUrl)
            .MaximumLength(1000)
            .NotEmpty()
            .When(x => x.Data?.PictureUrl != null);

        this.RuleFor(r => r.Data.IsActive)
            .NotEmpty()
            .When(x => x.Data?.IsActive != null);

        this.RuleFor(r => r.Data.Address)
            .SetValidator(new AddressUpdateValidator())
            .When(x => x.Data.Address != null);
    }
}
