namespace Pezza.Core.Product.Commands;

using FluentValidation;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
