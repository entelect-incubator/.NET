namespace DataAccess.Filters;

public static class OrderFilter
{
	public static IQueryable<Order> FilterByCustomerId(this IQueryable<Order> query, int? customerID)
	{
		return !customerID.HasValue ? query : query.Where(x => x.CustomerId == customerID.Value);
	}
}