namespace Core.Customer.Commands;

using System.Threading;
using System.Threading.Tasks;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class DeleteCustomerCommand : ICommand<Result>
{
    public int Id { get; set; }
}

public sealed class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand, Result>
{
    private readonly DatabaseContext databaseContext;

    public DeleteCustomerCommandHandler(DatabaseContext databaseContext)
        => this.databaseContext = databaseContext;

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var findEntity = await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        this.databaseContext.Customers.Remove(findEntity);

        return await CoreHelper.Outcome(this.databaseContext, cancellationToken, "Error deleting a customer");
    }
}
