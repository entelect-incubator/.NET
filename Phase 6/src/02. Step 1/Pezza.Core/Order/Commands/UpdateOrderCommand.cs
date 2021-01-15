namespace Pezza.Core.Order.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.Core.Notify.Commands;
    using Pezza.Core.Order.Events;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateOrderCommand : IRequest<Result<Common.Entities.Order>>
    {
        public int Id { get; set; }

        public OrderDataDTO Data { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<Common.Entities.Order>>
    {
        private readonly IDataAccess<Common.Entities.Order> dataAccess;
        private readonly IMediator mediator;

        public UpdateOrderCommandHandler(IDataAccess<Common.Entities.Order> dataAcess, IMediator mediator)
        {
            this.dataAccess = dataAcess;
            this.mediator = mediator;
        }

        public async Task<Result<Common.Entities.Order>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAccess.GetAsync(request.Id);
            findEntity.Completed = request.Data?.Completed ?? findEntity.Completed;
            findEntity.RestaurantId = request.Data?.RestaurantId ?? findEntity.RestaurantId;
            findEntity.CustomerId = request.Data?.CustomerId ?? findEntity.CustomerId;
            var outcome = await this.dataAccess.UpdateAsync(findEntity);

            if (outcome != null)
            {
                await this.mediator.Publish(new OrderCompletedEvent { CompletedOrder = findEntity }, cancellationToken);
                return Result<Common.Entities.Order>.Success(outcome);
            }
            else
            {
                return Result<Common.Entities.Order>.Failure("Error updating a Order");
            }
        }
    }
}