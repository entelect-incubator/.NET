namespace Pezza.Core.Stock.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetStockQuery : IRequest<ListResult<Common.Entities.Stock>>
    {
        public StockSearchModel StockSearchModel { get; set; }
    }

    public class GetStocksQueryHandler : IRequestHandler<GetStockQuery, ListResult<Common.Entities.Stock>>
    {
        private readonly IStockDataAccess dataAcess;

        public GetStocksQueryHandler(IStockDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Stock>> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.StockSearchModel);

            return ListResult<Common.Entities.Stock>.Success(search);
        }
    }
}
