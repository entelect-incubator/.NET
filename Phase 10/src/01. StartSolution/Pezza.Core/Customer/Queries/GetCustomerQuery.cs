namespace Core.Customer.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetCustomerQuery : ICommand<Result<CustomerDTO>>
{
    public int Id { get; set; }
}

public sealed class GetCustomerQueryHandler : ICommandHandler<GetCustomerQuery, Result<CustomerDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetCustomerQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<CustomerDTO>(await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<CustomerDTO>.Success(result);
    }
}

