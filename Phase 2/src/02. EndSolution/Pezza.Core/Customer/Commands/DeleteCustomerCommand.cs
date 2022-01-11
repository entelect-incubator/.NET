namespace Pezza.Core.Customer.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Models;
    using Pezza.Core.Helpers;
    using Pezza.DataAccess;

    public class DeleteCustomerCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly DatabaseContext databaseContext;

        public DeleteCustomerCommandHandler(DatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            this.databaseContext.Stocks.Remove(findEntity);
            var outcome = await this.databaseContext.SaveChangesAsync(cancellationToken);

            return CoreHelper.CoreResult(outcome, "Error deleting a Customer");
        }
    }
}
