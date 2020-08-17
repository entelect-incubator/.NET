namespace Pezza.Core.Notify.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class DeleteNotifyCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteNotifyCommandHandler : IRequestHandler<DeleteNotifyCommand, Result>
    {
        private readonly INotifyDataAccess dataAcess;

        public DeleteNotifyCommandHandler(INotifyDataAccess dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result> Handle(DeleteNotifyCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);
            var outcome = await this.dataAcess.DeleteAsync(findEntity);

            return (outcome) ? Result.Success() : Result.Failure("Error deleting a Notify");
        }
    }
}
