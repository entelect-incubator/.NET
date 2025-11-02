namespace Core.Pizza.Commands;

using Common.Models.Results;

public interface IUpdatePizzaCommand
{
	Task<Result<PizzaModel>> ExecuteAsync(int id, UpdatePizzaModel model, CancellationToken cancellationToken = default);
}

public sealed class UpdatePizzaCommand(DatabaseContext databaseContext) : IUpdatePizzaCommand
{
	public async Task<Result<PizzaModel>> ExecuteAsync(int id, UpdatePizzaModel model, CancellationToken cancellationToken = default)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, id);
		if (findEntity is null)
		{
			return Result<PizzaModel>.Failure("Not found");
		}

		findEntity.Name = model.Name is null ? findEntity.Name : model.Name;
		findEntity.Description = model.Description;
		findEntity.Price = model.Price ?? findEntity.Price;

		var outcome = databaseContext.Pizzas.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<PizzaModel>.Success(findEntity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}