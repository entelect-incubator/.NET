namespace Pezza.Core.Stock.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetStockQuery : IRequest<Result<Common.Entities.Stock>>
    {
        public int Id { get; set; }
    }

    public class GetStockQueryHandler : IRequestHandler<GetStockQuery, Result<Common.Entities.Stock>>
    {
        private readonly IDataAccess<Common.Entities.Stock> DataAccess;

        public GetStockQueryHandler(IDataAccess<Common.Entities.Stock> DataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<Common.Entities.Stock>> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAccess.GetAsync(request.Id);

            return Result<Common.Entities.Stock>.Success(search);
        }
    }
}
