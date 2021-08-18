namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class DeleteOrderCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result>
    {
        private readonly IDataAccess<OrderDTO> dto;

        public DeleteOrderCommandHandler(IDataAccess<OrderDTO> dto)
            => this.dto = dto;

        public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dto.DeleteAsync(request.Id);
            return outcome ? Result.Success() : Result.Failure("Error deleting a Order");
        }
    }
}
