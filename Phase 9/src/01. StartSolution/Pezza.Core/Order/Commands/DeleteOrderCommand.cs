namespace Pezza.Core.Order.Commands;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.Models;
using Pezza.Core.Helpers;
using Pezza.DataAccess;

public class DeleteOrderCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result>
{
    private readonly DatabaseContext databaseContext;

    public DeleteOrderCommandHandler(DatabaseContext databaseContext)
        => this.databaseContext = databaseContext;

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var findEntity = await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        this.databaseContext.Orders.Remove(findEntity);

        return await CoreHelper.Outcome(this.databaseContext, cancellationToken, "Error deleting a order");
    }
}
