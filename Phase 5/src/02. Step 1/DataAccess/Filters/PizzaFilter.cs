namespace Common.Filters;

public static class PizzaFilter
{
	public static IQueryable<Pizza> FilterByName(this IQueryable<Pizza> query, string name)
	{
		return string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));
	}

	public static IQueryable<Pizza> FilterByDescription(this IQueryable<Pizza> query, string description)
	{
		return string.IsNullOrWhiteSpace(description) ? query : query.Where(x => x.Description.Contains(description));
	}

	public static IQueryable<Pizza> FilterByDateCreated(this IQueryable<Pizza> query, DateTime? dateCreated)
	{
		return !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
	}
}