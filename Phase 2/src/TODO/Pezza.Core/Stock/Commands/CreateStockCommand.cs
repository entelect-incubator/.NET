namespace Pezza.Core.Stock.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateStockCommand : IRequest<Result<Stock>>
    {
        public Stock Stock { get; set; }
    }

    public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Result<Stock>>
    {
        private readonly IStockDataAccess dataAcess;

        public CreateStockCommandHandler(IStockDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Stock>> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            request.Stock.DateCreated = DateTime.Now;
            var outcome = await this.dataAcess.SaveAsync(request.Stock);

            return (outcome != null) ? Result<Stock>.Success(outcome) : Result<Stock>.Failure("Error adding a Stock");
        }
    }
}
