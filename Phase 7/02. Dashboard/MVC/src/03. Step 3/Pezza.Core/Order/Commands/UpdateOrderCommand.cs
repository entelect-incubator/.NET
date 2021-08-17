namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.Core.Order.Events;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateOrderCommand : IRequest<Result<OrderDTO>>
    {
        public OrderDTO Data { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> DataAccess;

        private readonly IMediator mediator;

        public UpdateOrderCommandHandler(IDataAccess<OrderDTO> DataAccess, IMediator mediator)
            => (this.DataAccess, this.mediator) = (DataAccess, mediator);

        public async Task<Result<OrderDTO>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.DataAccess.UpdateAsync(request.Data);
            if (request.Data.Completed.HasValue)
            {
                await this.mediator.Publish(new OrderCompletedEvent { CompletedOrder = outcome }, cancellationToken);
            }

            return (outcome != null) ? Result<OrderDTO>.Success(outcome) : Result<OrderDTO>.Failure("Error updating a Order");
        }
    }
}