namespace Common.Filters;

public static class OrderFilter
{
	public static IQueryable<Order> FilterByCustomerId(this IQueryable<Order> query, int? customerID)
	{
		if (!customerID.HasValue)
		{
			return query;
		}

		return query.Where(x => x.CustomerId == customerID.Value);
	}
}