namespace Core.Pizza.Commands;

using Utilities.CQRS;
using Utilities.Results;

public record DeletePizza(int Id) : ICommand<Result>;


public sealed class DeletePizzaHandler(DatabaseContext databaseContext) : ICommandHandler<DeletePizza, Result>
{
	public async Task<Result> Handle(DeletePizza command, CancellationToken cancellationToken = default)
	{
		var findEntity = await PizzaQueries.FindQuery(databaseContext, command.Id);
		if (findEntity is null)
		{
			return Result.Failure("Not found");
		}

		databaseContext.Pizzas.Remove(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}