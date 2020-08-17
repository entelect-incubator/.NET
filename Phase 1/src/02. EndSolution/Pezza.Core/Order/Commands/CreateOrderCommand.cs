namespace Pezza.Core.Order.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateOrderCommand : IRequest<Result<Order>>
    {
        public Order Order { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Order>>
    {
        private readonly IOrderDataAccess dataAcess;

        public CreateOrderCommandHandler(IOrderDataAccess orderDataAcess) => this.dataAcess = orderDataAcess;

        public async Task<Result<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            request.Order.DateCreated = DateTime.Now;
            var outcome = await this.dataAcess.SaveAsync(request.Order);

            return (outcome != null) ? Result<Order>.Success(outcome) : Result<Order>.Failure("Error adding a Order");
        }
    }
}
