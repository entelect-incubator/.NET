namespace Core.Order.Commands;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Core.Helpers;
using DataAccess;

public sealed class DeleteOrderCommand : ICommand<Result>
{
    public int Id { get; set; }
}

public sealed class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand, Result>
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
