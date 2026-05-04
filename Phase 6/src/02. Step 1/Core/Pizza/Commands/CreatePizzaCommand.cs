namespace Core.Pizza.Commands;

using Common.Models.Pizza;
using Core;
using LazyCache;

public sealed class CreatePizzaCommand : ICommand<Result<PizzaModel>>
{
	public CreatePizzaModel? Data { get; set; }
}

public sealed class CreatePizzaCommandHandler(DatabaseContext databaseContext, IAppCache cache) : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
	{
		if (request.Data is null)
		{
			return Result<PizzaModel>.Failure("Error");
		}

		var entity = new Common.Entities.Pizza
		{
			Name = request.Data.Name,
			Description = request.Data.Description,
			Price = request.Data.Price,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Pizzas.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		cache.Remove(Common.Data.CacheKey);

		return result > 0 ? Result<PizzaModel>.Success(entity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}