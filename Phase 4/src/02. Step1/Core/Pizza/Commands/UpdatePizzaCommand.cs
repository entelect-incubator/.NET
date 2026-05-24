namespace Core.Pizza.Commands;

using Common.Models.Pizza;

public sealed class UpdatePizzaCommand : ICommand<Result<PizzaModel>>
{
	public required int Id { get; set; }

	public required UpdatePizzaModel Data { get; set; }
}

public sealed class UpdatePizzaCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdatePizzaCommand, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(UpdatePizzaCommand request, CancellationToken cancellationToken)
	{
		var model = request.Data;
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, request.Id);
		if (findEntity is null)
		{
			return Result<PizzaModel>.Failure("Not found");
		}

		findEntity.Name = !string.IsNullOrEmpty(model?.Name) ? model?.Name : findEntity.Name;
		findEntity.Description = !string.IsNullOrEmpty(model?.Description) ? model?.Description : findEntity.Description;
		findEntity.Price = model.Price.HasValue ? model.Price.Value : findEntity.Price;

		var outcome = databaseContext.Pizzas.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<PizzaModel>.Success(findEntity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}