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
        private readonly IDataAccess<Common.Entities.Stock> DataAccess;

        public UpdateStockCommandHandler(IDataAccess<Common.Entities.Stock> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<Common.Entities.Stock>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.DataAccess.GetAsync(request.Id);

            findEntity.Name = !string.IsNullOrEmpty(request.Data?.Name) ? request.Data?.Name : findEntity.Name;
            findEntity.UnitOfMeasure = !string.IsNullOrEmpty(request.Data?.UnitOfMeasure) ? request.Data?.UnitOfMeasure : findEntity.UnitOfMeasure;
            findEntity.ValueOfMeasure = request.Data?.ValueOfMeasure ?? findEntity.ValueOfMeasure;
            findEntity.Quantity = request.Data.Quantity ?? findEntity.Quantity;
            findEntity.ExpiryDate = request.Data.ExpiryDate ?? findEntity.ExpiryDate;
            findEntity.Comment = request.Data?.Comment;
            var outcome = await this.DataAccess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Stock>.Success(outcome) : Result<Common.Entities.Stock>.Failure("Error updating a Stock");
        }
    }
}