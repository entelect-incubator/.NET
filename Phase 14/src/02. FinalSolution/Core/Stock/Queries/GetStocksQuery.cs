namespace Core.Stock.Queries;

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

public sealed class GetStocksQuery : IQuery<Result<IEnumerable<PizzaModel>>>
{
    public PizzaModel Data { get; set; }
}

public sealed class GetStocksQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetStocksQuery, Result<IEnumerable<PizzaModel>>>
{
    public async Task<Result<IEnumerable<PizzaModel>>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data ?? new PizzaModel();
        if (string.IsNullOrEmpty(dto.OrderBy))
        {
            dto.OrderBy = "DateCreated desc";
        }

        var entities = databaseContext.Stocks.Select(x => x)
            .AsNoTracking()
            .FilterByName(dto.Name)
            .FilterByUnitOfMeasure(dto.UnitOfMeasure)
            .FilterByValueOfMeasure(dto.ValueOfMeasure)
            .FilterByQuantity(dto.Quantity)
            .FilterByExpiryDate(dto.ExpiryDate)
            .FilterByComment(dto.Comment)

            .OrderBy(dto.OrderBy);

        var count = await entities.CountAsync(cancellationToken);
        var paged = (await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken)).Select(x => x.ToDto()).ToList();

        return Result<IEnumerable<PizzaModel>>.Success(paged, count);
    }
}
