namespace Core.Stock.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Extensions;
using Common.Filters;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetStocksQuery : IQuery<Result<IEnumerable<PizzaModel>>>
{
    public PizzaModel Data { get; set; }
}

public sealed class GetStocksQueryHandler : IQueryHandler<GetStocksQuery, Result<IEnumerable<PizzaModel>>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetStocksQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<IEnumerable<PizzaModel>>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
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

        var count = await entities.CountAsync(cancellationToken);
        var paged = this.mapper.Map<List<PizzaModel>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return Result<IEnumerable<PizzaModel>>.Success(paged, count);
    }
}
