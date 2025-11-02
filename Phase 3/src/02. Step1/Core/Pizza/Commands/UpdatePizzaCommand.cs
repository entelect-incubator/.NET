namespace Core.Pizza.Commands;

using Common.Models.Results;

public sealed record UpdatePizza(int Id, UpdatePizzaModel Model) : ICommand<Result<PizzaModel>>;


public sealed class UpdatePizzaHandler(DatabaseContext databaseContext) : ICommandHandler<UpdatePizza, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(UpdatePizza command, CancellationToken cancellationToken = default)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, command.Id);
		if (findEntity is null)
		{
			return Result<PizzaModel>.Failure("Not found");
		}

		findEntity.Name = command.Model.Name is null ? findEntity.Name : command.Model.Name;
		findEntity.Description = command.Model.Description;
		findEntity.Price = command.Model.Price ?? findEntity.Price;

		var outcome = databaseContext.Pizzas.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<PizzaModel>.Success(findEntity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}