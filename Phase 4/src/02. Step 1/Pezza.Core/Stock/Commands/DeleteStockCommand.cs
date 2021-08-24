namespace Pezza.Core.Stock.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class DeleteStockCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand, Result>
    {
        private readonly IDataAccess<StockDTO> dataAccess;

        public DeleteStockCommandHandler(IDataAccess<StockDTO> dataAccess)
            => this.dataAccess = dataAccess;

        public async Task<Result> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.DeleteAsync(request.Id);
            return outcome ? Result.Success() : Result.Failure("Error deleting a Stock");
        }
    }
}
