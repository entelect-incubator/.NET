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
        private readonly IDataAccess<StockDTO> dataAcess;

        public CreateStockCommandHandler(IDataAccess<StockDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<StockDTO>> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.SaveAsync(request.Data);
            return (outcome != null) ? Result<StockDTO>.Success(outcome) : Result<StockDTO>.Failure("Error adding a Stock");
        }
    }
}
