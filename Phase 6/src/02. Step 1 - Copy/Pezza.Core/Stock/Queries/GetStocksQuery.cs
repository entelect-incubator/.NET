namespace Pezza.Core.Stock.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetStocksQuery : IRequest<ListResult<StockDTO>>
    {
        public StockDataDTO SearchModel { get; set; }
    }

    public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ListResult<StockDTO>>
    {
        private readonly IDataAccess<Common.Entities.Stock> dataAcess;

        public GetStocksQueryHandler(IDataAccess<Common.Entities.Stock> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<StockDTO>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.SearchModel);

            return ListResult<StockDTO>.Success(search.Data.Map(), search.Count);
        }
    }
}
