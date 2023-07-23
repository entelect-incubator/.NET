namespace Core.Customer.Commands;

public class UpdateCustomerCommand : IRequest<Result<CustomerModel>>
{
	public int? Id { get; set; }

	public UpdateCustomerModel? Data { get; set; }
}

public class UpdateCustomerCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateCustomerCommand, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
	{
		if (request.Data == null || request.Id == null)
		{
			return Result<CustomerModel>.Failure("Error updating a Customer");
		}

		var model = request.Data;
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, request.Id.Value);
		if (findEntity == null)
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