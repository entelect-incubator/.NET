namespace Core.Product.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using DataAccess;

public class GetProductsQuery : IRequest<ListResult<ProductDTO>>
{
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ListResult<ProductDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetProductsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var entities = this.databaseContext.Orders.Select(x => x).AsNoTracking();

        var count = entities.Count();
        var paged = this.mapper.Map<List<ProductDTO>>(await entities.ToListAsync(cancellationToken));

        return ListResult<ProductDTO>.Success(paged, count);
    }
}
