namespace Pezza.Core.Stock.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.DTO;
using Pezza.Common.Extensions;
using Pezza.Common.Filters;
using Pezza.Common.Models;
using Pezza.DataAccess;

public class GetStocksQuery : IRequest<ListResult<StockDTO>>
{
    public StockDTO Data { get; set; }
}

public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ListResult<StockDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetStocksQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<StockDTO>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        if (string.IsNullOrEmpty(dto.OrderBy))
        {
            dto.OrderBy = "DateCreated desc";
        }

        var entities = this.databaseContext.Stocks.Select(x => x)
            .AsNoTracking()
            .FilterByName(dto.Name)
            .FilterByUnitOfMeasure(dto.UnitOfMeasure)
            .FilterByValueOfMeasure(dto.ValueOfMeasure)
            .FilterByQuantity(dto.Quantity)
            .FilterByExpiryDate(dto.ExpiryDate)
            .FilterByComment(dto.Comment)

            .OrderBy(dto.OrderBy);

        var count = entities.Count();
        var paged = this.mapper.Map<List<StockDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return ListResult<StockDTO>.Success(paged, count);
    }
}
