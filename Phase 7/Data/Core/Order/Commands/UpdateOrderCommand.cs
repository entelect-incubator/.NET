namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateOrderCommand : IRequest<Result<Common.Entities.Order>>
    {
        public int Id { get; set; }

        public OrderDataDTO Data { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<Common.Entities.Order>>
    {
        private readonly IDataAccess<Common.Entities.Order> DataAccess;

        public UpdateOrderCommandHandler(IDataAccess<Common.Entities.Order> DataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<Common.Entities.Order>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAccess.GetAsync(request.Id);
            findEntity.Completed = request.Data?.Completed ?? findEntity.Completed;
            findEntity.RestaurantId = request.Data?.RestaurantId ?? findEntity.RestaurantId;
            findEntity.CustomerId = request.Data?.CustomerId ?? findEntity.CustomerId;
            var outcome = await this.dataAccess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Order>.Success(outcome) : Result<Common.Entities.Order>.Failure("Error updating a Order");
        }
    }
}