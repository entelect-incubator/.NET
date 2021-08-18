namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class CreateOrderCommand : IRequest<Result<OrderDTO>>
    {
        public OrderDTO Data { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> dto;

        public CreateOrderCommandHandler(IDataAccess<OrderDTO> orderDataAccess) => this.dto = orderDataAccess;

        public async Task<Result<OrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dto.SaveAsync(request.Data);
            return (outcome != null) ? Result<OrderDTO>.Success(outcome) : Result<OrderDTO>.Failure("Error adding a Order");
        }
    }
}
