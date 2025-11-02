namespace Core.Notify.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using Core.Helpers;
using DataAccess;

public sealed class UpdateNotifyCommand : ICommand<Result<NotifyDTO>>
{
    public NotifyDTO Data { get; set; }
}

public sealed class UpdateNotifyCommandHandler : ICommandHandler<UpdateNotifyCommand, Result<NotifyDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public UpdateNotifyCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<NotifyDTO>> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity is null)
        {
            return null;
        }

        findEntity.CustomerId = dto.CustomerId ?? findEntity.CustomerId;
        findEntity.Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : findEntity.Email;
        findEntity.Sent = dto.Sent ?? findEntity.Sent;
        findEntity.Retry = dto.Retry ?? findEntity.Retry;

        this.databaseContext.Notify.Update(findEntity);

        return await CoreHelper<NotifyDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, findEntity, "Error updating notification");
    }
}