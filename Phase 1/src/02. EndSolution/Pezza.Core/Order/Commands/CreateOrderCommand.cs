namespace Pezza.Core.Order.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.DataAccess.Contracts;

    public partial class CreateOrderCommand : IRequest<Order>
    {
        public decimal Amount { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderDataAccess dataAcess;

        public CreateOrderCommandHandler(IOrderDataAccess orderDataAcess) => this.dataAcess = orderDataAcess;

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            => await this.dataAcess.SaveAsync(new Order
            {
                Amount = request.Amount,
                Customer = request.Customer,
                Restaurant = request.Restaurant,
                OrderItems = request.OrderItems,
                DateCreated = DateTime.Now
            });
    }
}
