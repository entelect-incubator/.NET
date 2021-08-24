namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateOrderCommand : IRequest<Result<Order>>
    {
        public OrderDataDTO Data { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Order>>
    {
        private readonly IDataAccess<Order> DataAccess;

        public CreateOrderCommandHandler(IDataAccess<Order> orderDataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.SaveAsync(request.Data.Map());

            return (outcome != null) ? Result<Order>.Success(outcome) : Result<Order>.Failure("Error adding a Order");
        }
    }
}
