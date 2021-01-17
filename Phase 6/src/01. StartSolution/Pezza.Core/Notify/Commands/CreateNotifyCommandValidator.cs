namespace Pezza.Core.Customer.Commands
{
    using FluentValidation;
    using Pezza.Core.Notify.Commands;

    public class CreateNotifyCommandValidator : AbstractValidator<CreateNotifyCommand>
    {
        public CreateNotifyCommandValidator()
        {
            this.RuleFor(r => r.Data.CustomerId)
                .NotNull();

            this.RuleFor(r => r.Data.Email)
                .MaximumLength(800)
                .EmailAddress()
                .NotEmpty();
        }
    }
}
