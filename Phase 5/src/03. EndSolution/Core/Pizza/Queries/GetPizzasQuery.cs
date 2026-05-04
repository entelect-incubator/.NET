namespace Core.Pizza.Queries;

using Common.Models.Pizza;
using DataAccess.Filters;

public sealed class GetPizzasQuery : LiteBus.Queries.Abstractions.IQuery<ListResult<PizzaModel>>
{
	public SearchPizzaModel Data { get; set; }
}

public sealed class GetPizzasQueryHandler(DatabaseContext databaseContext) : LiteBus.Queries.Abstractions.IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
	public async Task<ListResult<PizzaModel>> HandleAsync(GetPizzasQuery request, CancellationToken cancellationToken)
	{
		var entity = request.Data;
		if (string.IsNullOrEmpty(entity.OrderBy))
		{
			entity.OrderBy = "DateCreated desc";
		}

		var entities = databaseContext.Pizzas
			.Select(x => x)
			.AsNoTracking()
			.FilterByName(entity.Name)
			.FilterByDescription(entity.Description)
			.OrderBy(entity.OrderBy);

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

		return ListResult<PizzaModel>.Success(paged.Map(), count);
	}
}