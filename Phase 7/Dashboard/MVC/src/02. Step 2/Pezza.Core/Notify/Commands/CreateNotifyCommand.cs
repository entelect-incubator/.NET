namespace Pezza.Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;
    using Pezza.Core.Helpers;
    using Pezza.DataAccess;

    public class CreateNotifyCommand : IRequest<Result<NotifyDTO>>
    {
        public NotifyDTO Data { get; set; }
    }

    public class CreateNotifyCommandHandler : IRequestHandler<CreateNotifyCommand, Result<NotifyDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public CreateNotifyCommandHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<Result<NotifyDTO>> Handle(CreateNotifyCommand request, CancellationToken cancellationToken)
        {
            var entity = this.mapper.Map<Notify>(request.Data);
            this.databaseContext.Notify.Add(entity);
            var outcome = await this.databaseContext.SaveChangesAsync(cancellationToken);

            return await CoreHelper<NotifyDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating a Notification");
        }
    }
}
