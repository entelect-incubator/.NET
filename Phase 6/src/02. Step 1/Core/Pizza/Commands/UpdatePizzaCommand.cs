namespace Core.Pizza.Commands;

using Common.Models.Pizza;
using Core;
using LazyCache;

public sealed class UpdatePizzaCommand : ICommand<Result<PizzaModel>>
{
	public int? Id { get; set; }

	public UpdatePizzaModel? Data { get; set; }
}

public sealed class UpdatePizzaCommandHandler(DatabaseContext databaseContext, IAppCache cache) : ICommandHandler<UpdatePizzaCommand, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(UpdatePizzaCommand request, CancellationToken cancellationToken)
	{
		if (request.Data is null || request.Id is null)
		{
			return Result<PizzaModel>.Failure("Error");
		}

		var model = request.Data!;
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, request.Id.Value);
		if (findEntity is null)
		{
			return Result<PizzaModel>.Failure("Not found");
		}

		findEntity.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : findEntity.Name;
		findEntity.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : findEntity.Description;
		findEntity.Price = model.Price.HasValue ? model.Price.Value : findEntity.Price;

		var outcome = databaseContext.Pizzas.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		cache.Remove(Common.Data.CacheData.CacheKey);

		return result > 0 ? Result<PizzaModel>.Success(findEntity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}