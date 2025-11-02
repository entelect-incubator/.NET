namespace Core.Pizza.Queries;

public record GetPizzas() : IQuery<Result<IEnumerable<PizzaModel>>>
{
	public Task<Result<IEnumerable<PizzaModel>>> ExecuteAsync(Dispatcher dispatcher, CancellationToken ct = default)
		=> dispatcher.Query<GetPizzas, Result<IEnumerable<PizzaModel>>>(this, ct);
};

public sealed class GetPizzasHandler(DatabaseContext databaseContext) : IQueryHandler<GetPizzas, Result<IEnumerable<PizzaModel>>>
{
	public async Task<Result<IEnumerable<PizzaModel>>> Handle(GetPizzas query, CancellationToken cancellationToken = default)
	{
		var entities = databaseContext.Pizzas.Select(x => x).AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<PizzaModel>>.Success(paged.Map(), count);
	}
}