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
        private readonly IStockDataAccess dataAcess;

        public GetStockQueryHandler(IStockDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Stock>> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAsync(request.Id);

            return Result<Common.Entities.Stock>.Success(search);
        }
    }
}
