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

		var result = await databaseContext.Customers
			.Where(u => u.Id == request.Id)
			.ExecuteDeleteAsync(cancellationToken);

		return result > 0 ? Result.Success() : Result.Failure("Error");
	}
}