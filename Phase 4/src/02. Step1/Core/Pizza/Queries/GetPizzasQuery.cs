namespace Core.Pizza.Queries;

public sealed class GetPizzasQuery : IQuery<Result<IEnumerable<PizzaModel>>>
{
}

public sealed class GetPizzasQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetPizzasQuery, Result<IEnumerable<PizzaModel>>>
{
	public async Task<Result<IEnumerable<PizzaModel>>> HandleAsync(GetPizzasQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Pizzas.Select(x => x).AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<PizzaModel>>.Success(paged.Map(), count);
	}
}