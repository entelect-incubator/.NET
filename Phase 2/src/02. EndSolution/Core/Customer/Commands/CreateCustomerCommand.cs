namespace Core.Customer.Commands;

using Common.Models.Results;

public interface ICreateCustomerCommand
{
	Task<Result<CustomerModel>> ExecuteAsync(CreateCustomerModel model, CancellationToken cancellationToken = default);
}

public sealed class CreateCustomerCommand(DatabaseContext databaseContext) : ICreateCustomerCommand
{
	public async Task<Result<CustomerModel>> ExecuteAsync(CreateCustomerModel model, CancellationToken cancellationToken)
	{
		var entity = new Common.Entities.Customer
		{
			Name = model.Name,
			Email = model.Email,
			Address = model.Address,
			Cellphone = model.Cellphone,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Customers.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<CustomerModel>.Success(entity.Map()) : Result<CustomerModel>.Failure($"Error");
	}
}