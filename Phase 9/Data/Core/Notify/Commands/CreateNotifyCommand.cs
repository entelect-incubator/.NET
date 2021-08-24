namespace Pezza.Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateNotifyCommand : IRequest<Result<Notify>>
    {
        public NotifyDataDTO Data { get; set; }
    }

    public class CreateNotifyCommandHandler : IRequestHandler<CreateNotifyCommand, Result<Notify>>
    {
        private readonly IDataAccess<Notify> DataAccess;

        public CreateNotifyCommandHandler(IDataAccess<Notify> DataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<Notify>> Handle(CreateNotifyCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.SaveAsync(request.Data.Map());

            return (outcome != null) ? Result<Notify>.Success(outcome) : Result<Notify>.Failure("Error creating a Notification");
        }
    }
}
