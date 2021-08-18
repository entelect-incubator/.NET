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
        private readonly IDataAccess<StockDTO> dto;

        public DeleteStockCommandHandler(IDataAccess<StockDTO> dto)
            => this.dto = dto;

        public async Task<Result> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dto.DeleteAsync(request.Id);
            return outcome ? Result.Success() : Result.Failure("Error deleting a Stock");
        }
    }
}
