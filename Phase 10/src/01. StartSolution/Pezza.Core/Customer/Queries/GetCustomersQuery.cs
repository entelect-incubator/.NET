namespace Core.Customer.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Common.Extensions;
using Pezza.Common.Filters;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetCustomersQuery : IQuery<Result<IEnumerable<CustomerDTO>>>
{
    public CustomerDTO? Data { get; set; }
}

public sealed class GetCustomersQueryHandler(DatabaseContext databaseContext, IMapper mapper) : IQueryHandler<GetCustomersQuery, Result<IEnumerable<CustomerDTO>>>
{
    public async Task<Result<IEnumerable<CustomerDTO>>> HandleAsync(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            return Result<IEnumerable<CustomerDTO>>.Failure("Customer search criteria is required");
        }

        var dto = request.Data;

        dto.OrderBy ??= "DateCreated desc";

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
        var paged = mapper.Map<List<CustomerDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return Result<IEnumerable<CustomerDTO>>.Success(paged, count);
    }
}

