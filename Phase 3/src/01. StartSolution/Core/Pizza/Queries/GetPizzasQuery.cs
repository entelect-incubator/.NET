namespace Core.Pizza.Queries;

using Common.Models.Pizza;
using Common.Models.Results;

public interface IGetPizzasQuery
{
	Task<Result<IEnumerable<PizzaModel>>> ExecuteAsync(CancellationToken cancellationToken = default);

}

public sealed class GetPizzasQuery(DatabaseContext databaseContext) : IGetPizzasQuery
{
	public async Task<Result<IEnumerable<PizzaModel>>> ExecuteAsync(CancellationToken cancellationToken = default)
	{
		var entities = databaseContext.Pizzas.Select(x => x).AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<PizzaModel>>.Success(paged.Map(), count);
	}
}