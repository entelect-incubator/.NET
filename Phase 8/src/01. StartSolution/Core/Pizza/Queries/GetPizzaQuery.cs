namespace Core.Pizza.Queries;

using Common.Models.Pizza;
using Core;

public sealed class GetPizzaQuery : IQuery<Result<PizzaModel>>
{
	public int Id { get; set; }
}

public sealed class GetPizzaQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetPizzaQuery, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(GetPizzaQuery request, CancellationToken cancellationToken)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var entity = await query(databaseContext, request.Id);
		return entity is null ? Result<PizzaModel>.Failure("Not Found") : Result<PizzaModel>.Success(entity.Map());
	}
}