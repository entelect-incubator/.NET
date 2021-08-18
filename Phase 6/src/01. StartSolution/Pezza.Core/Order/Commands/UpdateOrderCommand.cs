namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class UpdateOrderCommand : IRequest<Result<OrderDTO>>
    {
        public OrderDTO Data { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> dto;

        public UpdateOrderCommandHandler(IDataAccess<OrderDTO> dto) => this.dto = dto;

        public async Task<Result<OrderDTO>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dto.UpdateAsync(request.Data);
            return (outcome != null) ? Result<OrderDTO>.Success(outcome) : Result<OrderDTO>.Failure("Error updating a Order");
        }
    }
}