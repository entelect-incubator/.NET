namespace Core.Pizza.Queries;

using System.Linq;
using Common.Models.Pizza;
using Core;
using DataAccess.Filters;
using LazyCache;

public sealed class GetPizzasQuery : IQuery<ListResult<PizzaModel>>
{
	public SearchPizzaModel Data { get; set; }
}

public sealed class GetPizzasQueryHandler(DatabaseContext databaseContext, IAppCache cache) : IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
	private readonly TimeSpan cacheExpiry = new(12, 0, 0);

	public async Task<ListResult<PizzaModel>> Handle(GetPizzasQuery request, CancellationToken cancellationToken)
	{
		var entity = request.Data;

		Task<IEnumerable<PizzaModel>> DataDelegate() => this.GetData();
		var cachedData = await cache.GetOrAddAsync(Common.Data.CacheKey, DataDelegate, this.cacheExpiry);

		if (cachedData != null)
		{
			var data = cachedData?
				.FilterByName(entity.Name)
				.FilterByDescription(entity.Description)
				.OrderBy(x => x.DateCreated)
				.ToList();

			return ListResult<PizzaModel>.Success(data, cachedData.Count());
		}

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

	private async Task<IEnumerable<PizzaModel>> GetData()
	{
		var entities = await databaseContext.Pizzas.Select(x => x)
			.AsNoTracking()
			.ToListAsync();

		return entities.Map();
	}
}