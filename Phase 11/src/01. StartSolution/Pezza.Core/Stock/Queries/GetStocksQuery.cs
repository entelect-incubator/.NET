namespace Core.Stock.Queries;

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

public sealed class GetStocksQuery : IQuery<Result<IEnumerable<PizzaModel>>>
{
    public PizzaModel? Data { get; set; }
}

public sealed class GetStocksQueryHandler(DatabaseContext databaseContext, IMapper mapper)
    : IQueryHandler<GetStocksQuery, Result<IEnumerable<PizzaModel>>>
{
    public async Task<Result<IEnumerable<PizzaModel>>> HandleAsync(GetStocksQuery request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            return Result<IEnumerable<PizzaModel>>.Failure("Stock search data is required");
        }

        var dto = request.Data;
        dto.OrderBy ??= "DateCreated desc";

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
        var paged = mapper.Map<List<PizzaModel>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return Result<IEnumerable<PizzaModel>>.Success(paged, count);
    }
}

