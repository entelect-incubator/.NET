namespace Core.Customer.Commands;

using Common.Models.Customer;
using Common.Models.Results;

public interface IUpdateCustomerCommand
{
	Task<Result<CustomerModel>> ExecuteAsync(int id, UpdateCustomerModel model, CancellationToken cancellationToken = default);
}

public sealed class UpdateCustomerCommand(DatabaseContext databaseContext) : IUpdateCustomerCommand
{
	public async Task<Result<CustomerModel>> ExecuteAsync(int id, UpdateCustomerModel model, CancellationToken cancellationToken = default)
	{
		var findEntity = await CustomerQueries.FindQuery(databaseContext, id);
		if (findEntity is null)
		{
			return Result<CustomerModel>.Failure("Not found");
		}

		findEntity.Name = model.Name is null ? findEntity.Name : model.Name;
		findEntity.Address = model.Address;
		findEntity.Cellphone = model.Cellphone;
		findEntity.Email = model.Email;

		databaseContext.Customers.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<CustomerModel>.Success(findEntity.Map()) : Result<CustomerModel>.Failure("Error");
	}
}