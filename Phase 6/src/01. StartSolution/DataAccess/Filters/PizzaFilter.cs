namespace DataAccess.Filters;

public static class PizzaFilter
{
	public static IQueryable<Pizza> FilterByName(this IQueryable<Pizza> query, string? name)
		=> string.IsNullOrEmpty(name) ? query : query.Where(x => x.Name.Contains(name));

	public static IQueryable<Pizza> FilterByDescription(this IQueryable<Pizza> query, string? description)
		=> string.IsNullOrEmpty(description) ? query : query.Where(x => x.Description!.Contains(description));

	public static IQueryable<Pizza> FilterByDateCreated(this IQueryable<Pizza> query, DateTime? dateCreated)
		=> !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
}