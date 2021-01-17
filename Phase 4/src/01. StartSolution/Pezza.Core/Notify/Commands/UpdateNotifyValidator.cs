namespace Pezza.Core.Customer.Commands
{
    using FluentValidation;
    using Pezza.Core.Notify.Commands;

    public class UpdateNotifyCommandValidator : AbstractValidator<UpdateNotifyCommand>
    {
        public UpdateNotifyCommandValidator()
        {
            this.RuleFor(r => r.Data)
                .NotNull();

            this.RuleFor(r => r.Data.Id)
                .NotEmpty();

            this.RuleFor(r => r.Data.Sent)
                .NotNull();

            this.RuleFor(r => r.Data.Retry)
                .NotNull();
        }
    }
}
