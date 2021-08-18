namespace Pezza.Core.Stock.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateStockCommand : IRequest<Result<StockDTO>>
    {
        public StockDTO Data { get; set; }
    }

    public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Result<StockDTO>>
    {
        private readonly IDataAccess<StockDTO> DataAccess;

        public CreateStockCommandHandler(IDataAccess<StockDTO> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<StockDTO>> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.DataAccess.SaveAsync(request.Data);
            return (outcome != null) ? Result<StockDTO>.Success(outcome) : Result<StockDTO>.Failure("Error adding a Stock");
        }
    }
}
