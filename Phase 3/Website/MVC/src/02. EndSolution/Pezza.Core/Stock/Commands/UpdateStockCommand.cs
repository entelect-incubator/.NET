namespace Pezza.Core.Stock.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateStockCommand : IRequest<Result<Common.Entities.Stock>>
    {
        public int Id { get; set; }

        public StockDataDTO Data { get; set; }
    }

    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, Result<Common.Entities.Stock>>
    {
        private readonly IDataAccess<Common.Entities.Stock> dataAcess;

        public UpdateStockCommandHandler(IDataAccess<Common.Entities.Stock> dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Stock>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);

            if (!string.IsNullOrEmpty(request.Data?.Name))
            {
                findEntity.Name = request.Data?.Name;
            }

            if (!string.IsNullOrEmpty(request.Data?.UnitOfMeasure))
            {
                findEntity.UnitOfMeasure = request.Data?.UnitOfMeasure;
            }

            if (request.Data.ValueOfMeasure.HasValue)
            {
                findEntity.ValueOfMeasure = request.Data?.ValueOfMeasure;
            }

            if (request.Data.Quantity.HasValue)
            {
                findEntity.Quantity = request.Data.Quantity.Value;
            }

            if (request.Data.ExpiryDate.HasValue)
            {
                findEntity.ExpiryDate = request.Data?.ExpiryDate;
            }

            if (!string.IsNullOrEmpty(request.Data?.Comment))
            {
                findEntity.Comment = request.Data?.Comment;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Stock>.Success(outcome) : Result<Common.Entities.Stock>.Failure("Error updating a Stock");
        }
    }
}