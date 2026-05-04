namespace Core.Product.Commands;

using System.Threading;
using System.Threading.Tasks;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class DeleteProductCommand : ICommand<Result>
{
    public int Id { get; set; }
}

public sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Result>
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
