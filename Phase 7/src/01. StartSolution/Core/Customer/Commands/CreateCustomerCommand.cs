namespace Core.Customer.Commands;

using Core;

public sealed class CreateCustomerCommand : ICommand<Result<CustomerModel>>
{
	public CreateCustomerModel? Data { get; set; }
}

public sealed class CreateCustomerCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreateCustomerCommand, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
	{
		if (request.Data == null)
		{
			return Result<CustomerModel>.Failure($"Error");
		}

		var entity = new Common.Entities.Customer
		{
			Name = request.Data.Name,
			Email = request.Data.Email,
			Address = request.Data.Address,
			Cellphone = request.Data.Cellphone,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Customers.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<CustomerModel>.Success(entity.Map()) : Result<CustomerModel>.Failure($"Error");
	}
}