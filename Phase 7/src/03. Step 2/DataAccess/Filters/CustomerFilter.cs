namespace Common.Filters;

public static class CustomerFilter
{
	public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return query;
		}

		return query.Where(x => x.Name.Contains(name));
	}

	public static IQueryable<Customer> FilterByAddress(this IQueryable<Customer> query, string address)
	{
		if (string.IsNullOrWhiteSpace(address))
		{
			return query;
		}

		return query.Where(x => x.Address.Contains(address));
	}

	public static IQueryable<Customer> FilterByPhone(this IQueryable<Customer> query, string cellphone)
	{
		if (string.IsNullOrWhiteSpace(cellphone))
		{
			return query;
		}

		return query.Where(x => x.Cellphone.Contains(cellphone));
	}

	public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			return query;
		}

		return query.Where(x => x.Email.Contains(email));
	}

	public static IQueryable<Customer> FilterByDateCreated(this IQueryable<Customer> query, DateTime? dateCreated)
	{
		if (!dateCreated.HasValue)
		{
			return query;
		}

		return query.Where(x => x.DateCreated == dateCreated.Value);
	}
}