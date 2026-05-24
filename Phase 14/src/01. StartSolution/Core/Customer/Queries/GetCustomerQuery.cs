namespace Core.Customer.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetCustomerQuery : IQuery<Result<CustomerDTO>>
{
    public int Id { get; set; }
}

public sealed class GetCustomerQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomerQuery, Result<CustomerDTO>>
{
    public async Task<Result<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var result = (await databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken))?.ToDto();
        return Result<CustomerDTO>.Success(result);
    }
}
