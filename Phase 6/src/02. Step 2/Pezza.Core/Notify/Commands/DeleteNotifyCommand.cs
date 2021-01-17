namespace Pezza.Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class DeleteNotifyCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteNotifyCommandHandler : IRequestHandler<DeleteNotifyCommand, Result>
    {
        private readonly IDataAccess<NotifyDTO> dataAcess;

        public DeleteNotifyCommandHandler(IDataAccess<NotifyDTO> dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result> Handle(DeleteNotifyCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.DeleteAsync(request.Id);
            return outcome ? Result.Success() : Result.Failure("Error deleting notification");
        }
    }
}
