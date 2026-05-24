namespace Core.Pizza.Commands;

using Common.Models.Pizza;
using Common.Models.Results;

public interface ICreatePizzaCommand
{
	Task<Result<PizzaModel>> ExecuteAsync(CreatePizzaModel model, CancellationToken cancellationToken = default);
}

public sealed class CreatePizzaCommand(DatabaseContext databaseContext) : ICreatePizzaCommand
{
	public async Task<Result<PizzaModel>> ExecuteAsync(CreatePizzaModel model, CancellationToken cancellationToken = default)
	{
		var entity = new Common.Entities.Pizza
		{
			Name = model.Name,
			Description = model.Description,
			Price = model.Price,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Pizzas.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<PizzaModel>.Success(entity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}