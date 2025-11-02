namespace Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.Models;
using Core.Helpers;
using DataAccess;

public sealed class DeleteStockCommand : ICommand<Result>
{
    public int Id { get; set; }
}

public sealed class DeleteStockCommandHandler : ICommandHandler<DeleteStockCommand, Result>
{
    private readonly DatabaseContext databaseContext;

    public DeleteStockCommandHandler(DatabaseContext databaseContext)
        => this.databaseContext = databaseContext;

    public async Task<Result> HandleAsync(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        this.databaseContext.Stocks.Remove(findEntity);

        return await CoreHelper.Outcome(this.databaseContext, cancellationToken, "Error deleting pizza");
    }
}

