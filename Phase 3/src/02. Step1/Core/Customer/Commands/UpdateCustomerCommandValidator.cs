namespace Core.Customer.Commands;

using FluentValidation;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        this.RuleFor(r => r.Data)
            .NotNull();

        this.RuleFor(r => r.Id)
            .NotEmpty();

        this.RuleFor(r => r.Data.Name)
            .MaximumLength(100);

        this.RuleFor(r => r.Data.Cellphone)
            .MaximumLength(50)
            .Matches(@"^[0-9]*$");

        this.RuleFor(r => r.Data.Email)
            .MaximumLength(500)
            .EmailAddress();

        this.RuleFor(r => r.Data.Address)
            .NotEmpty()
			.MaximumLength(500);
    }
}
