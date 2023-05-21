namespace Core.Customer.Commands;

using Core.Pizza.Commands;
using FluentValidation;

public class UpdatePizzaCommandValidator : AbstractValidator<UpdatePizzaCommand>
{
    public UpdatePizzaCommandValidator()
    {
        this.RuleFor(r => r.Data)
            .NotNull();

        this.RuleFor(r => r.Id)
            .NotEmpty();

        this.RuleFor(r => r.Data.Name)
            .MaximumLength(100);

        this.RuleFor(r => r.Data.Description)
            .MaximumLength(500);

		this.RuleFor(r => r.Data.Price)
			.PrecisionScale(4, 2, false);

	}
}
