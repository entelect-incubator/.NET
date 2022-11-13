namespace Pezza.Core.Customer.Commands;

using FluentValidation;
using Pezza.Core.Product.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        this.RuleFor(r => r.Data.Name)
            .MaximumLength(150)
            .NotEmpty();

        this.RuleFor(r => r.Data.Description)
            .MaximumLength(1000)
            .NotEmpty()
            .When(x => x.Data?.Description != null);

        this.RuleFor(r => r.Data.PictureUrl)
            .MaximumLength(1000)
            .NotEmpty()
            .When(x => x.Data?.PictureUrl != null);

        this.RuleFor(r => r.Data.Price)
           .NotEmpty();

        this.RuleFor(r => r.Data.Special)
           .NotEmpty();

        this.RuleFor(r => r.Data.OfferEndDate)
           .NotEmpty()
           .When(x => x.Data?.OfferEndDate != null);

        this.RuleFor(r => r.Data.OfferPrice)
           .NotEmpty()
           .When(x => x.Data?.OfferPrice != null);
    }
}
