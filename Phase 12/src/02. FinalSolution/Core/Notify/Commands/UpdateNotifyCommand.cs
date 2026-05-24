namespace Core.Notify.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class UpdateNotifyCommand : ICommand<Result<NotifyDTO>>
{
    public NotifyDTO Data { get; set; }
}

public sealed class UpdateNotifyCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateNotifyCommand, Result<NotifyDTO>>
{
    public async Task<Result<NotifyDTO>> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity is null)
        {
            return null;
        }

        findEntity.CustomerId = dto.CustomerId ?? findEntity.CustomerId;
        findEntity.Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : findEntity.Email;
        findEntity.Sent = dto.Sent ?? findEntity.Sent;
        findEntity.Retry = dto.Retry ?? findEntity.Retry;

        databaseContext.Notify.Update(findEntity);

        return await CoreHelper<NotifyDTO>.Outcome(databaseContext, cancellationToken, findEntity.ToDto(), "Error updating notification");
    }
}