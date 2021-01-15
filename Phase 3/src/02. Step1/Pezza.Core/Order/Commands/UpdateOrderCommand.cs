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
        private readonly IDataAccess<Common.Entities.Order> dataAcess;

        public UpdateOrderCommandHandler(IDataAccess<Common.Entities.Order> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Order>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);
            findEntity.Completed = request.Data?.Completed ?? findEntity.Completed;
            findEntity.RestaurantId = request.Data?.RestaurantId ?? findEntity.RestaurantId;
            findEntity.CustomerId = request.Data?.CustomerId ?? findEntity.CustomerId;
            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Order>.Success(outcome) : Result<Common.Entities.Order>.Failure("Error updating a Order");
        }
    }
}