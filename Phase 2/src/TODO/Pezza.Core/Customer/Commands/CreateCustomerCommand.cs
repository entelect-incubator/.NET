namespace Pezza.Core.Customer.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateCustomerCommand : IRequest<Result<Customer>>
    {
        public Customer Customer { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<Customer>>
    {
        private readonly ICustomerDataAccess dataAcess;

        public CreateCustomerCommandHandler(ICustomerDataAccess dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Customer>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.SaveAsync(request.Customer);

            return (outcome != null) ? Result<Customer>.Success(outcome) : Result<Customer>.Failure("Error creating a Customer");
        }
    }
}
