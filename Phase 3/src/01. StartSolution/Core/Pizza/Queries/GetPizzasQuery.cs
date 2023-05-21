namespace Core.Pizza.Queries;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
	public class GetPizzasQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
	{
		public async Task<ListResult<PizzaModel>> Handle(GetPizzasQuery request, CancellationToken cancellationToken)
		{
			var entities = databaseContext.Pizzas.Select(x => x).AsNoTracking();

			var count = entities.Count();
			var paged = await entities.ToListAsync(cancellationToken);

			return ListResult<PizzaModel>.Success(paged.Map(), count);
		}
	}
}