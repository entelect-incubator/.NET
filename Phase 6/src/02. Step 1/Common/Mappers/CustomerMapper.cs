namespace Common.Mappers;

public static class CustomerMapper
{
	public static CustomerModel Map(this Customer entity)
		=> new()
		{
			Id = entity.Id,
			Name = entity.Name,
			Address = entity.Address,
			Cellphone = entity.Cellphone,
			Email = entity.Email,
			DateCreated = entity.DateCreated
		};

	public static Customer Map(this CustomerModel model)
	{
		var entity = new Customer
		{
			Id = model.Id,
			Name = model.Name,
			DateCreated = model.DateCreated
		};

		if (!string.IsNullOrEmpty(model.Address))
		{
			entity.Address = model.Address;
		}

		if (!string.IsNullOrEmpty(model.Cellphone))
		{
			entity.Cellphone = model.Cellphone;
		}

		if (!string.IsNullOrEmpty(model.Email))
		{
			entity.Email = model.Email;
		}

		return entity;
	}

	public static IEnumerable<CustomerModel> Map(this List<Customer> entities)
		=> entities.Select(x => x.Map());

	public static IEnumerable<Customer> Map(this List<CustomerModel> models)
		=> models.Select(x => x.Map());
}
