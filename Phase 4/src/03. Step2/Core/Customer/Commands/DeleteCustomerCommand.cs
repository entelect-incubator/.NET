namespace Core.Customer.Commands;

using Core;

public sealed class DeleteCustomerCommand : ICommand<Result>
{
	public int? Id { get; set; }
}

public sealed class DeleteCustomerCommandHandler(DatabaseContext databaseContext) : ICommandHandler<DeleteCustomerCommand, Result>
{
	public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
	{
		if (request.Id is null)
		{
			return Result.Failure("Error");
		}

		var entity = await databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
		if (entity is null)
		{
			return Result.Failure("Error");
		}

		databaseContext.Customers.Remove(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}