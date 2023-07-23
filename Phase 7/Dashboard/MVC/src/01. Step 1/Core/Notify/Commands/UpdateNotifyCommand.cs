namespace Core.Notify.Commands;

public class UpdateNotifyCommand : IRequest<Result>
{
	public int Id { get; set; }

	public bool Sent { get; set; }
}

public class UpdateNotifyCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateNotifyCommand, Result>
{
	public async Task<Result> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Notifies.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, request.Id);
		if (findEntity == null)
		{
			return Result.Failure("Not found");
		}

		findEntity.Sent = request.Sent;

		var outcome = databaseContext.Notifies.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}