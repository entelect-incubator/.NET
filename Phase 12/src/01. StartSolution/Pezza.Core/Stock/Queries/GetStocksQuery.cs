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

public sealed class GetStocksQuery : ICommand<ListResult<PizzaModel>>
{
    public PizzaModel Data { get; set; }
}

public sealed class GetStocksQueryHandler : ICommandHandler<GetStocksQuery, ListResult<PizzaModel>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetStocksQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<PizzaModel>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
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

        return ListResult<PizzaModel>.Success(paged, count);
    }
}

