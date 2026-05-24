namespace Core.Notify.Commands;

using System.Threading;
using System.Threading.Tasks;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class DeleteNotifyCommand : ICommand<Result>
{
    public int Id { get; set; }
}

public sealed class DeleteNotifyCommandHandler : ICommandHandler<DeleteNotifyCommand, Result>
{
    private readonly DatabaseContext databaseContext;

    public DeleteNotifyCommandHandler(DatabaseContext databaseContext)
        => this.databaseContext = databaseContext;

    public async Task<Result> Handle(DeleteNotifyCommand request, CancellationToken cancellationToken)
    {
        var findEntity = await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        this.databaseContext.Notify.Remove(findEntity);

        return await CoreHelper.Outcome(this.databaseContext, cancellationToken, "Error deleting a notification");
    }
}
