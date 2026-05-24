namespace Core.Pizza.Commands;

using Common.Models.Results;

public interface IDeletePizzaCommand
{
	Task<Result> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}

public sealed class DeletePizzaCommand(DatabaseContext databaseContext) : IDeletePizzaCommand
{
	public async Task<Result> ExecuteAsync(int id, CancellationToken cancellationToken = default)
	{
		var findEntity = await PizzaQueries.FindQuery(databaseContext, id);
		if (findEntity is null)
		{
			return Result.Failure("Not found");
		}

		databaseContext.Pizzas.Remove(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}