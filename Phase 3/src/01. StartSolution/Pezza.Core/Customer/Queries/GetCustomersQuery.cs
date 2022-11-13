namespace Pezza.Core.Customer.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.DTO;
using Pezza.Common.Models;
using Pezza.DataAccess;

public class GetCustomersQuery : IRequest<ListResult<CustomerDTO>>
{
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ListResult<CustomerDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetCustomersQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<CustomerDTO>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var entities = this.databaseContext.Customers.Select(x => x).AsNoTracking();

        var count = entities.Count();
        var paged = this.mapper.Map<List<CustomerDTO>>(await entities.ToListAsync(cancellationToken));

        return ListResult<CustomerDTO>.Success(paged, count);
    }
}
