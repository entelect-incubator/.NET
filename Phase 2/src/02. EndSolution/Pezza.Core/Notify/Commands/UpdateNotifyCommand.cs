namespace Pezza.Core.Notify.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateNotifyCommand : IRequest<Result<Common.Entities.Notify>>
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }

        public DateTime? DateSent { get; set; }
    }

    public class UpdateNotifyCommandHandler : IRequestHandler<UpdateNotifyCommand, Result<Common.Entities.Notify>>
    {
        private readonly IDataAccess<Common.Entities.Notify> dataAcess;

        public UpdateNotifyCommandHandler(IDataAccess<Common.Entities.Notify> dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Notify>> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);

            if (request.CustomerId.HasValue)
            {
                findEntity.CustomerId = request.CustomerId.Value;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                findEntity.Email = request.Email;
            }

            if (request.Sent.HasValue)
            {
                findEntity.Sent = request.Sent.Value;
            }

            if (request.Retry.HasValue)
            {
                findEntity.Retry = request.Retry.Value;
            }

            if (request.DateSent.HasValue)
            {
                findEntity.DateSent = request.DateSent.Value;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Notify>.Success(outcome) : Result<Common.Entities.Notify>.Failure("Error updating a Notify");
        }
    }
}