namespace Core.Product.Commands;

using FluentValidation;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}
