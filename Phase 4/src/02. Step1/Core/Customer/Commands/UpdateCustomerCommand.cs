namespace Core.Customer.Commands;

using Common.Models.Customer;

public sealed class UpdateCustomerCommand : ICommand<Result<CustomerModel>>
{
	public int? Id { get; set; }

	public UpdateCustomerModel? Data { get; set; }
}

public sealed class UpdateCustomerCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateCustomerCommand, Result<CustomerModel>>
{
	public Task<Result<CustomerModel>> Handle(UpdateCustomerCommand command, CancellationToken ct) => throw new NotImplementedException();

	public async Task<Result<CustomerModel>> HandleAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
	{
		if (request.Data is null || request.Id is null)
		{
			return Result<CustomerModel>.Failure("Error updating a Customer");
		}

		var model = request.Data;
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, request.Id.Value);
		if (findEntity is null)
		{
			return Result<CustomerModel>.Failure("Not found");
		}

		findEntity.Name = !string.IsNullOrEmpty(model?.Name) ? model?.Name : findEntity.Name;
		findEntity.Address = !string.IsNullOrEmpty(model?.Address) ? model?.Address : findEntity.Address;
		findEntity.Cellphone = !string.IsNullOrEmpty(model?.Cellphone) ? model?.Cellphone : findEntity.Cellphone;
		findEntity.Email = !string.IsNullOrEmpty(model?.Email) ? model?.Email : findEntity.Email;

		var outcome = databaseContext.Customers.Update(findEntity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<CustomerModel>.Success(findEntity.Map()) : Result<CustomerModel>.Failure("Error");
	}
}