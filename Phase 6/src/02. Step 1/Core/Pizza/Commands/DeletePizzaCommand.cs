namespace Core.Pizza.Commands;

using LazyCache;

public class DeletePizzaCommand : IRequest<Result>
{
	public int? Id { get; set; }

	public class DeletePizzaCommandHandler(DatabaseContext databaseContext, IAppCache cache) : IRequestHandler<DeletePizzaCommand, Result>
	{
		public async Task<Result> Handle(DeletePizzaCommand request, CancellationToken cancellationToken)
		{
			if (request.Id == null)
			{
				return Result.Failure("Error");
			}

			var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
			var findEntity = await query(databaseContext, request.Id.Value);
			if (findEntity == null)
			{
				return Result.Failure("Not found");
			}

			databaseContext.Pizzas.Remove(findEntity);
			var result = await databaseContext.SaveChangesAsync(cancellationToken);

			cache.Remove(Common.Data.CacheKey);

			return result > 0 ? Result.Success() : Result.Failure("Error");
		}
	}
}