namespace Dispatch;

public static class QueryMediatorExtensions
{
	// Convenience overload for type inference when calling Query
	public static Task<TResult> Query<TResult>(this Dispatcher dispatcher, IQuery<TResult> query, CancellationToken ct = default)
		=> dispatcher.Query<IQuery<TResult>, TResult>(query, ct);
}
