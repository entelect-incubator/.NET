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
    }

    public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ListResult<StockDTO>>
    {
        private readonly IDataAccess<StockDTO> DataAccess;

        public GetStocksQueryHandler(IDataAccess<StockDTO> DataAccess) => this.DataAccess = DataAccess;

        public async Task<ListResult<StockDTO>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAllAsync();
            return ListResult<StockDTO>.Success(search, search.Count);
        }
    }
}
