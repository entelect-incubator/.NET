namespace Pezza.Core.Stock.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetStocksQuery : IRequest<ListResult<Common.Entities.Stock>>
    {
        public StockSearchModel StockSearchModel { get; set; }
    }

    public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ListResult<Common.Entities.Stock>>
    {
        private readonly IStockDataAccess dataAcess;

        public GetStocksQueryHandler(IStockDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Stock>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.StockSearchModel);

            return ListResult<Common.Entities.Stock>.Success(search);
        }
    }
}
