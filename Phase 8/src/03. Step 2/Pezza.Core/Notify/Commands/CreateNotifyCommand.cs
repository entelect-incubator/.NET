namespace Pezza.Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class CreateNotifyCommand : IRequest<Result<NotifyDTO>>
    {
        public NotifyDTO Data { get; set; }
    }

    public class CreateNotifyCommandHandler : IRequestHandler<CreateNotifyCommand, Result<NotifyDTO>>
    {
        private readonly IDataAccess<NotifyDTO> dataAccess;

        public CreateNotifyCommandHandler(IDataAccess<NotifyDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<NotifyDTO>> Handle(CreateNotifyCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.SaveAsync(request.Data);
            return (outcome != null) ? Result<NotifyDTO>.Success(outcome) : Result<NotifyDTO>.Failure("Error creating a Notification");
        }
    }
}
