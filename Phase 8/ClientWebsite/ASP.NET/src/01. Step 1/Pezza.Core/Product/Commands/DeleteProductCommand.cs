namespace Core.Product.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Common.Models;
    using Core.Helpers;
    using DataAccess;

    public class DeleteProductCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly DatabaseContext databaseContext;

        public DeleteProductCommandHandler(DatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            this.databaseContext.Products.Remove(findEntity);

            return await CoreHelper.Outcome(this.databaseContext, cancellationToken, "Error deleting a product");
        }
    }
}
