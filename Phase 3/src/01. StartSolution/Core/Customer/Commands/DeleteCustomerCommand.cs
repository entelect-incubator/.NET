namespace Core.Customer.Commands;

using Common.Models.Results;

public interface IDeleteCustomerCommand
{
	Task<Result> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}

public sealed class DeleteCustomerCommand(DatabaseContext databaseContext) : IDeleteCustomerCommand
{
	public async Task<Result> ExecuteAsync(int id, CancellationToken cancellationToken = default)
	{
		var entity = await databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		if (entity is null)
		{
			return Result.Failure("Error");
		}

		databaseContext.Customers.Remove(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}