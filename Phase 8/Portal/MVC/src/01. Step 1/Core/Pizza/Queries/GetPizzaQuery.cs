namespace Core.Pizza.Queries;

public class GetPizzaQuery : IRequest<Result<PizzaModel>>
{
	public int Id { get; set; }
}

public class GetPizzaQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetPizzaQuery, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(GetPizzaQuery request, CancellationToken cancellationToken)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
		var entity = await query(databaseContext, request.Id);
		if (entity == null)
		{
			return Result<PizzaModel>.Failure("Not Found");
		}

		return Result<PizzaModel>.Success(entity.Map());
	}
}