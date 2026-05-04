namespace Utilities.CQRS;

using MediatR;
using Microsoft.Extensions.DependencyInjection;

public interface ICommand<TResult> : IRequest<TResult> { }
public interface IQuery<TResult> : IRequest<TResult> { }
public interface INotification : MediatR.INotification { }

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
	where TCommand : ICommand<TResult>
{ }

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
	where TQuery : IQuery<TResult>
{ }

public interface INotificationHandler<TNotification> : MediatR.INotificationHandler<TNotification>
	where TNotification : INotification
{ }

public class Dispatcher(IServiceProvider provider)
{
	public Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken ct = default)
		where TCommand : ICommand<TResult>
	{
		var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
		return handler.Handle(command, ct);
	}

	public Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken ct = default)
		where TQuery : IQuery<TResult>
	{
		var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
		return handler.Handle(query, ct);
	}

	public async Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
		where TNotification : INotification
	{
		var handlers = provider.GetServices<INotificationHandler<TNotification>>();
		foreach (var handler in handlers)
			await handler.Handle(notification, ct);
	}
}
