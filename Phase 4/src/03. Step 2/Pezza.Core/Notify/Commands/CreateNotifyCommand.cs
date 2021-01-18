namespace Pezza.Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateNotifyCommand : IRequest<Result<NotifyDTO>>
    {
        public NotifyDTO Data { get; set; }
    }

    public class CreateNotifyCommandHandler : IRequestHandler<CreateNotifyCommand, Result<NotifyDTO>>
    {
        private readonly IDataAccess<NotifyDTO> dataAcess;

        public CreateNotifyCommandHandler(IDataAccess<NotifyDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<NotifyDTO>> Handle(CreateNotifyCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.SaveAsync(request.Data);
            return (outcome != null) ? Result<NotifyDTO>.Success(outcome) : Result<NotifyDTO>.Failure("Error creating a Notification");
        }
    }
}
