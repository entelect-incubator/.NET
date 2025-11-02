namespace Core.Customer.Commands;

public record DeleteCustomer(int Id) : ICommand<Result>;

public sealed class DeleteCustomerHandler(DatabaseContext databaseContext) : ICommandHandler<DeleteCustomer, Result>
{
	public async Task<Result> Handle(DeleteCustomer command, CancellationToken cancellationToken = default)
	{
		var entity = await databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
		if (entity is null)
		{
			return Result.Failure("Error");
		}

		databaseContext.Customers.Remove(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}