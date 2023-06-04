namespace Core.Pizza.Queries;

using System.Linq;
using LazyCache;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
	public SearchPizzaModel Data { get; set; }

	public class GetPizzasQueryHandler(DatabaseContext databaseContext, IAppCache cache) : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
	{
		private readonly TimeSpan cacheExpiry = new(12, 0, 0);

		public async Task<ListResult<PizzaModel>> Handle(GetPizzasQuery request, CancellationToken cancellationToken)
		{
			var entity = request.Data;

			Task<IEnumerable<PizzaModel>> DataDelegate() => this.GetData();
			var cachedData = await cache.GetOrAddAsync(Common.Data.CacheKey, DataDelegate, this.cacheExpiry);

			if(cachedData != null)
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

			var count = entities.Count();
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
}