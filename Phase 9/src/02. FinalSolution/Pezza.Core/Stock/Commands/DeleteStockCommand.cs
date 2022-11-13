namespace Pezza.Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.Models;
using Pezza.Core.Helpers;
using Pezza.DataAccess;

public class DeleteStockCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand, Result>
{
    private readonly DatabaseContext databaseContext;

    public DeleteStockCommandHandler(DatabaseContext databaseContext)
        => this.databaseContext = databaseContext;

    public async Task<Result> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        this.databaseContext.Stocks.Remove(findEntity);

        return await CoreHelper.Outcome(this.databaseContext, cancellationToken, "Error deleting stock");
    }
}
