namespace Pezza.Common.Behaviours.Validators;

using FluentValidation;
using Pezza.Common.Models.Base;

public class AddressValidator : AbstractValidator<AddressBase>
{
    public AddressValidator()
    {
        RuleFor(x => x.Address)
        .NotEmpty()
        .MaximumLength(500);

        RuleFor(x => x.City)
        .NotEmpty()
        .MaximumLength(100);

        RuleFor(x => x.Province)
        .NotEmpty()
        .MaximumLength(100);

        RuleFor(x => x.PostalCode)
        .NotEmpty()
        .Must(x => int.TryParse(x, out var val) && val > 0)
        .MaximumLength(8);
    }
}

public class AddressUpdateValidator : AbstractValidator<AddressBase>
{
    public AddressUpdateValidator()
    {
        RuleFor(x => x.Address)
        .MaximumLength(500);

        RuleFor(x => x.City)
        .MaximumLength(100);

        RuleFor(x => x.Province)
        .MaximumLength(100);

        RuleFor(x => x.PostalCode)
        .Must(x => int.TryParse(x, out var val) && val > 0)
        .When(x => !string.IsNullOrWhiteSpace(x.PostalCode))
        .MaximumLength(8);
    }
}
