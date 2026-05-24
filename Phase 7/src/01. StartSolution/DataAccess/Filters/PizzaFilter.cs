namespace DataAccess.Filters;

using Common.Models.Pizza;

public static class PizzaFilter
{
	public static IQueryable<Pizza> FilterByName(this IQueryable<Pizza> query, string name) => string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));

	public static IEnumerable<PizzaModel> FilterByName(this IEnumerable<PizzaModel> query, string name) => string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));

	public static IQueryable<Pizza> FilterByDescription(this IQueryable<Pizza> query, string description) => string.IsNullOrWhiteSpace(description) ? query : query.Where(x => x.Description.Contains(description));

	public static IEnumerable<PizzaModel> FilterByDescription(this IEnumerable<PizzaModel> query, string description) => string.IsNullOrWhiteSpace(description) ? query : query.Where(x => x.Description.Contains(description));

	public static IQueryable<Pizza> FilterByDateCreated(this IQueryable<Pizza> query, DateTime? dateCreated) => !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);

	public static IEnumerable<PizzaModel> FilterByDateCreated(this IEnumerable<PizzaModel> query, DateTime? dateCreated) => !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
}