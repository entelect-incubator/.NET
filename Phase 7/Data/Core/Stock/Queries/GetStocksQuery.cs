namespace Pezza.Core.Stock.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetStocksQuery : IRequest<ListResult<Common.Entities.Stock>>
    {
    }

    public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ListResult<Common.Entities.Stock>>
    {
        private readonly IDataAccess<Common.Entities.Stock> DataAccess;

        public GetStocksQueryHandler(IDataAccess<Common.Entities.Stock> DataAccess) => this.dataAccess = dataAccess;

        public async Task<ListResult<Common.Entities.Stock>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAccess.GetAllAsync();

            return ListResult<Common.Entities.Stock>.Success(search);
        }
    }
}
