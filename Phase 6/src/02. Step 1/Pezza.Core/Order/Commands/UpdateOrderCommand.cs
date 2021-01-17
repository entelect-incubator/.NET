namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateOrderCommand : IRequest<Result<OrderDTO>>
    {
        public OrderDTO Data { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> dataAcess;

        public UpdateOrderCommandHandler(IDataAccess<OrderDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<OrderDTO>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.UpdateAsync(request.Data);
            await this.mediator.Publish(new OrderCompletedEvent { CompletedOrder = findEntity }, cancellationToken);
            return (outcome != null) ? Result<OrderDTO>.Success(outcome) : Result<OrderDTO>.Failure("Error updating a Order");
        }
    }
}