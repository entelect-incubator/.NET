namespace Core.Pizza.Queries;

using Common.Models.Pizza;
using Common.Models.Results;

public interface IGetPizzaQuery
{
	Task<Result<PizzaModel>> ExecuteAsync(int id, CancellationToken cancellationToken = default);

}

public sealed class GetPizzaQuery(DatabaseContext databaseContext) : IGetPizzaQuery
{
	public async Task<Result<PizzaModel>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
	{
		var entity = await PizzaQueries.FindQuery(databaseContext, id);
		return entity is null
			? Result<PizzaModel>.Failure("Not Found")
			: Result<PizzaModel>.Success(entity.Map());
	}
}