namespace Api.Helpers;

using Utilities.CQRS;

public static class DispatcherExtensions
{
	public static Task<TResult> Query<TQuery, TResult>(this Dispatcher dispatcher, TQuery query, CancellationToken ct = default)
		where TQuery : IQuery<TResult>
		=> dispatcher.Query<TQuery, TResult>(query, ct);

	public static Task<TResult> Send<TResult>(this Dispatcher dispatcher, ICommand<TResult> command, CancellationToken ct = default)
		=> dispatcher.Send<ICommand<TResult>, TResult>(command, ct);

	public static Task Publish<TNotification>(this Dispatcher dispatcher, TNotification notification, CancellationToken ct = default)
		where TNotification : INotification
		=> dispatcher.Publish(notification, ct);
}
