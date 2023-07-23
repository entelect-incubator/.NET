namespace Core.Customer.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Common.DTO;
using Common.Entities;
using Common.Models;
using Core.Helpers;
using DataAccess;

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

        return await CoreHelper<CustomerDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating a customer");
    }
}
