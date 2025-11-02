namespace Core.Customer.Commands;

using Utilities.CQRS;
using Utilities.Results;

public record CreateCustomer(CreateCustomerModel Model) : ICommand<Result<CustomerModel>>;

public sealed class CreateCustomerHandler(DatabaseContext databaseContext) : ICommandHandler<CreateCustomer, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(CreateCustomer command, CancellationToken cancellationToken)
	{
		var entity = new Common.Entities.Customer
		{
			Name = command.Model.Name,
			Email = command.Model.Email,
			Address = command.Model.Address,
			Cellphone = command.Model.Cellphone,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Customers.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<CustomerModel>.Success(entity.Map()) : Result<CustomerModel>.Failure($"Error");
	}
}