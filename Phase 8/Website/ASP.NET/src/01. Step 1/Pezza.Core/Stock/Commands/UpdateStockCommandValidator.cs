namespace Core.Customer.Commands
{
    using FluentValidation;
    using Core.Stock.Commands;

    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            this.RuleFor(r => r.Data)
                .NotNull();

            this.RuleFor(r => r.Data.Id)
                .NotEmpty();

            this.RuleFor(r => r.Data.Name)
                 .MaximumLength(100)
                 .NotEmpty()
                 .When(x => x.Data?.Name != null);

            this.RuleFor(r => r.Data.UnitOfMeasure)
                .MaximumLength(20)
                .NotEmpty()
                .When(x => x.Data?.UnitOfMeasure != null);

            this.RuleFor(r => r.Data.ValueOfMeasure)
                .NotEmpty()
                .When(x => x.Data?.ValueOfMeasure != null);

            this.RuleFor(r => r.Data.Quantity)
                .NotEmpty()
                .When(x => x.Data?.Quantity != null);

            this.RuleFor(r => r.Data.ExpiryDate)
                .NotEmpty()
                .When(x => x.Data?.ExpiryDate != null);

            this.RuleFor(r => r.Data.Comment)
                .NotEmpty()
                .When(x => x.Data?.Comment != null);
        }
    }
}
