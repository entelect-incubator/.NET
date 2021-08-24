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
        private readonly IDataAccess<Common.Entities.Notify> DataAccess;

        public DeleteNotifyCommandHandler(IDataAccess<Common.Entities.Notify> DataAccess)
            => this.dataAccess = dataAccess;

        public async Task<Result> Handle(DeleteNotifyCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.DeleteAsync(request.Id);

            return (outcome) ? Result.Success() : Result.Failure("Error deleting a Notify");
        }
    }
}
