namespace Pezza.Core.Stock.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateStockCommand : IRequest<Result<Stock>>
    {
        public StockDataDTO Data { get; set; }
    }

    public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Result<Stock>>
    {
        private readonly IDataAccess<Stock> DataAccess;

        public CreateStockCommandHandler(IDataAccess<Stock> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<Stock>> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.DataAccess.SaveAsync(request.Data.Map());

            return (outcome != null) ? Result<Stock>.Success(outcome) : Result<Stock>.Failure("Error adding a Stock");
        }
    }
}
