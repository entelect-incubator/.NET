namespace Core.Customer.Commands;

public sealed class DeleteCustomerCommand : LiteBus.Commands.Abstractions.ICommand<Result>
{
	public int? Id { get; set; }
}

public sealed class DeleteCustomerCommandHandler(DatabaseContext databaseContext) : LiteBus.Commands.Abstractions.ICommandHandler<DeleteCustomerCommand, Result>
{
	public async Task<Result> HandleAsync(DeleteCustomerCommand request, CancellationToken cancellationToken)
	{
		if (request.Id is null)
		{
			return Result.Failure("Error");
		}

		var result = await databaseContext.Customers
			.Where(u => u.Id == request.Id)
			.ExecuteDeleteAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}