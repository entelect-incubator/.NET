namespace Pezza.Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.Core.Helpers;
    using Pezza.DataAccess;

    public class UpdateNotifyCommand : IRequest<Result<NotifyDTO>>
    {
        public NotifyDTO Data { get; set; }
    }

    public class UpdateNotifyCommandHandler : IRequestHandler<UpdateNotifyCommand, Result<NotifyDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public UpdateNotifyCommandHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<Result<NotifyDTO>> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var findEntity = await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
            if (findEntity == null)
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
}