namespace Core.Customer.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Entities;
using Core.Helpers;
using DataAccess;

public sealed class CreateCustomerCommand : ICommand<Result<CustomerDTO>>
{
    public CustomerDTO Data { get; set; }
}

public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Result<CustomerDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public CreateCustomerCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<CustomerDTO>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<Customer>(request.Data);
        this.databaseContext.Customers.Add(entity);

        return await CoreHelper<CustomerDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating a customer");
    }
}
