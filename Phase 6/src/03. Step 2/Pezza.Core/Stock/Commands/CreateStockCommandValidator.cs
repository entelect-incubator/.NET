namespace Pezza.Core.Stock.Commands;

using FluentValidation;

public class CreateStockCommandValidator : AbstractValidator<CreateStockCommand>
{
    public CreateStockCommandValidator()
    {
        this.RuleFor(r => r.Data.Name)
            .MaximumLength(100)
            .NotEmpty();

        this.RuleFor(r => r.Data.UnitOfMeasure)
            .MaximumLength(20)
            .NotEmpty()
            .When(x => x.Data?.UnitOfMeasure != null);

        this.RuleFor(r => r.Data.ValueOfMeasure)
            .NotEmpty()
            .When(x => x.Data?.ValueOfMeasure != null);

        this.RuleFor(r => r.Data.Quantity)
            .NotEmpty();

        this.RuleFor(r => r.Data.ExpiryDate)
            .NotEmpty()
            .When(x => x.Data?.ExpiryDate != null);

        this.RuleFor(r => r.Data.Comment)
            .NotEmpty()
            .When(x => x.Data?.Comment != null);
    }
}
