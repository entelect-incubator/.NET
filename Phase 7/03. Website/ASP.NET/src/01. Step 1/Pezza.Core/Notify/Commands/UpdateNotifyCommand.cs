namespace Pezza.Core.Notify.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateNotifyCommand : IRequest<Result<NotifyDTO>>
    {
        public NotifyDTO Data { get; set; }
    }

    public class UpdateNotifyCommandHandler : IRequestHandler<UpdateNotifyCommand, Result<NotifyDTO>>
    {
        private readonly IDataAccess<NotifyDTO> DataAccess;

        public UpdateNotifyCommandHandler(IDataAccess<NotifyDTO> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<NotifyDTO>> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.DataAccess.UpdateAsync(request.Data);
            return (outcome != null) ? Result<NotifyDTO>.Success(outcome) : Result<NotifyDTO>.Failure("Error updating notification");
        }
    }
}