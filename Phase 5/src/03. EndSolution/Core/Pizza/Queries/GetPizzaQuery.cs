namespace Core.Pizza.Queries;

using Common.Models.Pizza;

public sealed class GetPizzaQuery : LiteBus.Queries.Abstractions.IQuery<Result<PizzaModel>>
{
	public int Id { get; set; }
}

public sealed class GetPizzaQueryHandler(DatabaseContext databaseContext) : LiteBus.Queries.Abstractions.IQueryHandler<GetPizzaQuery, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> HandleAsync(GetPizzaQuery request, CancellationToken cancellationToken)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var entity = await query(databaseContext, request.Id);
		return entity is null ? Result<PizzaModel>.Failure("Not Found") : Result<PizzaModel>.Success(entity.Map());
	}
}