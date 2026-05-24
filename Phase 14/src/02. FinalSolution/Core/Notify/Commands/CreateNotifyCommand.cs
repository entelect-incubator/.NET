namespace Core.Notify.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;

public sealed class CreateNotifyCommand : ICommand<Result<NotifyDTO>>
{
    public NotifyDTO Data { get; set; }
}

public sealed class CreateNotifyCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreateNotifyCommand, Result<NotifyDTO>>
{
    public async Task<Result<NotifyDTO>> Handle(CreateNotifyCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Data.ToEntity();
        databaseContext.Notify.Add(entity);
        var outcome = await CoreHelper<NotifyDTO>.Outcome(databaseContext, cancellationToken, request.Data, "Error creating a Notification");
        if (outcome.Succeeded)
        {
            request.Data.Id = entity.Id;
        }

        return outcome;
    }
}
