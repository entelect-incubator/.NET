namespace Core.Customer.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetCustomerQuery : IQuery<Result<CustomerDTO>>
{
    public int Id { get; set; }
}

public sealed class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, Result<CustomerDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IObjectMapper mapper;

    public GetCustomerQueryHandler(DatabaseContext databaseContext, IObjectMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<CustomerDTO>(await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<CustomerDTO>.Success(result);
    }
}
