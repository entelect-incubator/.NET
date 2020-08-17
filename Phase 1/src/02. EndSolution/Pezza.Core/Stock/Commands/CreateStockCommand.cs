namespace Pezza.Core.Stock.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.DataAccess.Contracts;

    public partial class CreateStockCommand : IRequest<Stock>
    {
        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal? ValueOfMeasure { get; set; }

        public int Quantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Comment { get; set; }
    }

    public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Stock>
    {
        private readonly IStockDataAccess dataAcess;

        public CreateStockCommandHandler(IStockDataAccess dataAcess) 
            => this.dataAcess = dataAcess;

        public async Task<Stock> Handle(CreateStockCommand request, CancellationToken cancellationToken)
            => await this.dataAcess.SaveAsync(new Stock
            {
                Name = request.Name,
                UnitOfMeasure = request.UnitOfMeasure,
                ValueOfMeasure = request.ValueOfMeasure,
                Quantity = request.Quantity,
                ExpiryDate = request.ExpiryDate,
                Comment = request.Comment,
                DateCreated = DateTime.Now
            });
    }
}
