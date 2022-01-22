namespace Pezza.Core.Stock.Queries
{
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

    public class GetStocksQuery : IRequest<ListResult<StockDTO>>
    {
    }

    public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ListResult<StockDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public GetStocksQueryHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<ListResult<StockDTO>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var entities = this.databaseContext.Stocks.Select(x => x).AsNoTracking();

            var count = entities.Count();
            var paged = this.mapper.Map<List<StockDTO>>(await entities.ToListAsync(cancellationToken));

            return ListResult<StockDTO>.Success(paged, count);
        }
    }
}
