namespace Pezza.Core.Customer.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;
    using Pezza.Core.Helpers;
    using Pezza.DataAccess;

    public class CreateCustomerCommand : IRequest<Result<CustomerDTO>>
    {
        public CustomerDTO Data { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public CreateCustomerCommandHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<Result<CustomerDTO>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = this.mapper.Map<Customer>(request.Data);
            this.databaseContext.Customers.Add(entity);
            var outcome = await this.databaseContext.SaveChangesAsync(cancellationToken);

            return CoreHelper<CustomerDTO>.Outcome(this.mapper, outcome, entity, "Error creating a Customer");
        }
    }
}
