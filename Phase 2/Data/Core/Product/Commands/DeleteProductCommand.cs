namespace Pezza.Core.Product.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class DeleteProductCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IDataAccess<ProductDTO> dataAccess;

        public DeleteProductCommandHandler(IDataAccess<ProductDTO> dataAccess)
            => this.dataAccess = dataAccess;

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.DeleteAsync(request.Id);
            return (outcome) ? Result.Success() : Result.Failure("Error deleting a Product");
        }
    }
}
