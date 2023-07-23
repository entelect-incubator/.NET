namespace Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Common.Models;
    using Core.Helpers;
    using DataAccess;

    public class DeleteNotifyCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteNotifyCommandHandler : IRequestHandler<DeleteNotifyCommand, Result>
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
}
