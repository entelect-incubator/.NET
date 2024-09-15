namespace Common.Extensions;

public static class PagingExtensions
{
	public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingArgs pagingArgs)
	{
		var myPagingArgs = pagingArgs;

		if (pagingArgs is null)
		{
			myPagingArgs = PagingArgs.Default;
		}

		return myPagingArgs.UsePaging ? query.Skip(myPagingArgs.Offset).Take(myPagingArgs.Limit) : query;
	}
}