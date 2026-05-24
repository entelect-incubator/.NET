namespace Core.Pizza.Queries;

using Common.Models.Pizza;

public record GetPizza(int Id) : IQuery<Result<PizzaModel>>
{
	public Task<Result<PizzaModel>> ExecuteAsync(Dispatcher dispatcher, CancellationToken ct = default)
		=> dispatcher.Query<GetPizza, Result<PizzaModel>>(this, ct);
};

public sealed class GetPizzaHandler(DatabaseContext databaseContext) : IQueryHandler<GetPizza, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(GetPizza query, CancellationToken cancellationToken = default)
	{
		var entity = await PizzaQueries.FindQuery(databaseContext, query.Id);
		return entity is null
			? Result<PizzaModel>.Failure("Not Found")
			: Result<PizzaModel>.Success(entity.Map());
	}
}