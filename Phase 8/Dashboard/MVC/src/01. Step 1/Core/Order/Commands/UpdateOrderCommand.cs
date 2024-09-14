namespace Core.Notify.Commands;

public class UpdateOrderCommand : IRequest<Result>
{
	public int Id { get; set; }

	public bool Completed { get; set; }
}

public class UpdateOrderCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateOrderCommand, Result>
{
	public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Orders.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, request.Id);
		if (findEntity == null)
		{
			return Result.Failure("Not found");
		}

		findEntity.Completed = request.Completed;

		var outcome = databaseContext.Orders.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}