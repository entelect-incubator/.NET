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
        private readonly IDataAccess<StockDTO> dataAcess;

        public GetStocksQueryHandler(IDataAccess<StockDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<StockDTO>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
            => await this.dataAcess.GetAllAsync(request.SearchModel);
    }
}
