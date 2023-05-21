namespace Core.Customer.Commands;

using Core.Pizza.Commands;
using FluentValidation;

public class DeletePizzaCommandValidator : AbstractValidator<DeletePizzaCommand>
{
    public DeletePizzaCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
