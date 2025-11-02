namespace Core.Customer.Commands;

public record UpdateCustomer(int Id, UpdateCustomerModel Model) : ICommand<Result<CustomerModel>>;

public sealed class UpdateCustomerHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateCustomer, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(UpdateCustomer command, CancellationToken cancellationToken = default)
	{
		var findEntity = await CustomerQueries.FindQuery(databaseContext, command.Id);
		if (findEntity is null)
		{
			return Result<CustomerModel>.Failure("Not found");
		}

		findEntity.Name = command.Model.Name is null ? findEntity.Name : command.Model.Name;
		findEntity.Address = command.Model.Address;
		findEntity.Cellphone = command.Model.Cellphone;
		findEntity.Email = command.Model.Email;

		databaseContext.Customers.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<CustomerModel>.Success(findEntity.Map()) : Result<CustomerModel>.Failure("Error");
	}
}