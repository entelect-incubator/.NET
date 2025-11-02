namespace Core.Pizza.Commands;

public sealed class DeletePizzaCommand : ICommand<Result>
{
	public int? Id { get; set; }
}

public sealed class DeletePizzaCommandHandler(DatabaseContext databaseContext) : ICommandHandler<DeletePizzaCommand, Result>
{
	public async Task<Result> HandleAsync(DeletePizzaCommand request, CancellationToken cancellationToken)
	{
		if (request.Id is null)
		{
			return Result.Failure("Error");
		}

		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, request.Id.Value);
		if (findEntity is null)
		{
			return Result.Failure("Not found");
		}

		databaseContext.Pizzas.Remove(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}