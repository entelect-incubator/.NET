namespace Pezza.Core.Notify.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateNotifyCommand : IRequest<Result<Common.Entities.Notify>>
    {
        public int Id { get; set; }

        public NotifyDataDTO Data { get; set; }
    }

    public class UpdateNotifyCommandHandler : IRequestHandler<UpdateNotifyCommand, Result<Common.Entities.Notify>>
    {
        private readonly IDataAccess<Common.Entities.Notify> dataAcess;

        public UpdateNotifyCommandHandler(IDataAccess<Common.Entities.Notify> dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Notify>> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);

            if (request.Data.CustomerId.HasValue)
            {
                findEntity.CustomerId = request.Data.CustomerId.Value;
            }

            if (!string.IsNullOrEmpty(request.Data?.Email))
            {
                findEntity.Email = request.Data?.Email;
            }

            if (request.Data.Sent.HasValue)
            {
                findEntity.Sent = request.Data.Sent.Value;
            }

            if (request.Data.Retry.HasValue)
            {
                findEntity.Retry = request.Data.Retry.Value;
            }

            findEntity.DateSent = DateTime.Now;

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Notify>.Success(outcome) : Result<Common.Entities.Notify>.Failure("Error updating a Notify");
        }
    }
}