namespace Pezza.Core.Stock.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetStocksQuery : IRequest<ListResult<StockDTO>>
    {
        public StockDTO SearchModel { get; set; }
    }

    public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ListResult<StockDTO>>
    {
        private readonly IDataAccess<StockDTO> dataAccess;

        public GetStocksQueryHandler(IDataAccess<StockDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<ListResult<StockDTO>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
            => await this.dataAccess.GetAllAsync(request.SearchModel);
    }
}
