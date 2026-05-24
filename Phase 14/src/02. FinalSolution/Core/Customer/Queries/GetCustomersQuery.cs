namespace Core.Customer.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Extensions;
using Common.Filters;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetCustomersQuery : IQuery<Result<IEnumerable<CustomerDTO>>>
{
    public CustomerDTO Data { get; set; }
}

public sealed class GetCustomersQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomersQuery, Result<IEnumerable<CustomerDTO>>>
{
    public async Task<Result<IEnumerable<CustomerDTO>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data ?? new CustomerDTO();

        if (string.IsNullOrEmpty(dto.OrderBy))
        {
            dto.OrderBy = "DateCreated desc";
        }

        var entities = databaseContext.Customers.Select(x => x)
            .AsNoTracking()
            .FilterByName(dto.Name)
            .FilterByAddress(dto.Address?.Address)
            .FilterByCity(dto.Address?.City)
            .FilterByProvince(dto.Address?.Province)
            .FilterByPostalCode(dto.Address?.PostalCode)
            .FilterByPhone(dto.Phone)
            .FilterByEmail(dto.Email)
            .FilterByContactPerson(dto.ContactPerson)
            .OrderBy(dto.OrderBy);

        var count = await entities.CountAsync(cancellationToken);
        var paged = (await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken)).Select(x => x.ToDto()).ToList();

        return Result<IEnumerable<CustomerDTO>>.Success(paged, count);
    }
}
