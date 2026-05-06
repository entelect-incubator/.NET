namespace Core;

using Microsoft.Extensions.DependencyInjection;

public interface ICommand<TResult>
{
}

public interface ICommandHandler<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	Task<TResult> Handle(TCommand command, CancellationToken ct);
}

public interface INotification
{
}

public interface INotificationHandler<TNotification>
	where TNotification : INotification
{
	Task Handle(TNotification notification, CancellationToken ct);
}

public interface IPipelineBehavior<TRequest, TResult>
{
	Task<TResult> Handle(TRequest request, Func<Task<TResult>> next, CancellationToken ct);
}

public delegate Task<TResult> RequestHandlerDelegate<TResult>();

public interface IQuery<TResult>
{
}

public interface IQueryHandler<TQuery, TResult>
	where TQuery : IQuery<TResult>
{
	Task<TResult> Handle(TQuery query, CancellationToken ct);
}

public class Dispatcher(IServiceProvider provider)
{
	public Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken ct = default)
		where TCommand : ICommand<TResult>
	{
		var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
		return this.ExecutePipeline(command, () => handler.Handle(command, ct), ct);
	}

	public Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken ct = default)
		where TQuery : IQuery<TResult>
	{
		var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
		return this.ExecutePipeline(query, () => handler.Handle(query, ct), ct);
	}

	public Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
		where TNotification : INotification
	{
		var handlers = provider.GetServices<INotificationHandler<TNotification>>();
		return Task.WhenAll(handlers.Select(handler => handler.Handle(notification, ct)));
	}

	private async Task<TResult> ExecutePipeline<TRequest, TResult>(TRequest request, Func<Task<TResult>> handler, CancellationToken ct)
	{
		var behaviors = provider.GetServices<IPipelineBehavior<TRequest, TResult>>().ToList();

		var next = handler;
		for (var i = behaviors.Count - 1; i >= 0; i--)
		{
			var behavior = behaviors[i];
			var currentNext = next;
			next = () => behavior.Handle(request, currentNext, ct);
		}

		return await next();
	}
}
