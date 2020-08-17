namespace Pezza.Core.Stock.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateStockCommand : IRequest<Result<Common.Entities.Stock>>
    {
        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal? ValueOfMeasure { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Comment { get; set; }
    }

    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, Result<Common.Entities.Stock>>
    {
        private readonly IStockDataAccess dataAcess;

        public UpdateStockCommandHandler(IStockDataAccess dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Stock>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(new StockSearchModel
            {
                Name = request.Name
            });
            var findEntity = search.FirstOrDefault();

            if (!string.IsNullOrEmpty(request.Name))
            {
                findEntity.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.UnitOfMeasure))
            {
                findEntity.UnitOfMeasure = request.UnitOfMeasure;
            }

            if (request.ValueOfMeasure.HasValue)
            {
                findEntity.ValueOfMeasure = request.ValueOfMeasure;
            }

            if (request.Quantity.HasValue)
            {
                findEntity.Quantity = request.Quantity.Value;
            }

            if (request.ExpiryDate.HasValue)
            {
                findEntity.ExpiryDate = request.ExpiryDate;
            }

            if (!string.IsNullOrEmpty(request.Comment))
            {
                findEntity.Comment = request.Comment;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Stock>.Success(outcome) : Result<Common.Entities.Stock>.Failure("Error updating a Stock");
        }
    }
}