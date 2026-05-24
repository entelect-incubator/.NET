namespace Api.Helpers;

using Core;

public static class DispatcherExtensions
{
	public static Task<TResult> Query<TResult>(this Dispatcher dispatcher, IQuery<TResult> query, CancellationToken ct = default)
		=> dispatcher.Query((dynamic)query, ct);

	public static Task<TResult> Send<TResult>(this Dispatcher dispatcher, ICommand<TResult> command, CancellationToken ct = default)
		=> dispatcher.Send((dynamic)command, ct);

	public static Task Publish<TNotification>(this Dispatcher dispatcher, TNotification notification, CancellationToken ct = default)
		where TNotification : INotification
		=> dispatcher.Publish(notification, ct);
}
