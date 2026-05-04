namespace Core.Pizza.Commands;

using Common.Models.Pizza;

public sealed record CreatePizza(CreatePizzaModel Model) : ICommand<Result<PizzaModel>>;


public sealed class CreatePizzaHandler(DatabaseContext databaseContext) : ICommandHandler<CreatePizza, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(CreatePizza query, CancellationToken cancellationToken = default)
	{
		var entity = new Common.Entities.Pizza
		{
			Name = query.Model.Name,
			Description = query.Model.Description,
			Price = query.Model.Price,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Pizzas.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<PizzaModel>.Success(entity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}