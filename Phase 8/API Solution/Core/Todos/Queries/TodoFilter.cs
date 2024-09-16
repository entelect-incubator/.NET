namespace Core.Todos.Queries;

using Common.Entities;

public static class TodoFilter
{
	public static IQueryable<Todo> FilterByTask(this IQueryable<Todo> query, string? task)
		=> string.IsNullOrWhiteSpace(task) ? query : query.Where(x => x.Task.Contains(task));

	public static IQueryable<Todo> FilterByCompleted(this IQueryable<Todo> query, bool? isCompleted)
		=> !isCompleted.HasValue ? query : query.Where(x => x.IsCompleted == isCompleted.Value);

	public static IQueryable<Todo> FilterByDate(this IQueryable<Todo> query, DateTime? dateCreated = null, int? year = null, int? month = null, int? day = null)
	{
		if (dateCreated.HasValue)
		{
			return query.Where(x => x.DateCreated.HasValue && x.DateCreated.Value.Date == dateCreated.Value.Date);
		}

		if (year.HasValue)
		{
			query = query.Where(x => x.DateCreated.HasValue && x.DateCreated.Value.Year == year.Value);
		}

		if (month.HasValue)
		{
			query = query.Where(x => x.DateCreated.HasValue && x.DateCreated.Value.Month == month.Value);
		}

		if (day.HasValue)
		{
			query = query.Where(x => x.DateCreated.HasValue && x.DateCreated.Value.Day == day.Value);
		}

		return query;
	}
}