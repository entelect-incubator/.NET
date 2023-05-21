namespace Common.Filters;

public static class PizzaFilter
{
	public static IQueryable<Pizza> FilterByName(this IQueryable<Pizza> query, string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return query;
		}

		return query.Where(x => x.Name.Contains(name));
	}

	public static IQueryable<Pizza> FilterByDescription(this IQueryable<Pizza> query, string description)
	{
		if (string.IsNullOrWhiteSpace(description))
		{
			return query;
		}

		return query.Where(x => x.Description.Contains(description));
	}

	public static IQueryable<Pizza> FilterByDateCreated(this IQueryable<Pizza> query, DateTime? dateCreated)
	{
		if (!dateCreated.HasValue)
		{
			return query;
		}

		return query.Where(x => x.DateCreated == dateCreated.Value);
	}
}