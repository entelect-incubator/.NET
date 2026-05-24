namespace Utilities.CQRS;

using Microsoft.Extensions.DependencyInjection;

public interface ICommand<TResult> { }
public interface IQuery<TResult> { }
public interface INotification { }

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
	Task<TResult> Handle(TCommand command, CancellationToken ct);
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
	Task<TResult> Handle(TQuery query, CancellationToken ct);
}

public interface INotificationHandler<TNotification> where TNotification : INotification
{
	Task Handle(TNotification notification, CancellationToken ct);
}

public class Dispatcher(IServiceProvider provider)
{
	public Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken ct = default)
		where TCommand : ICommand<TResult>
	{
		var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
		return handler.Handle(command, ct);
	}

	public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default)
	{
		var commandType = command.GetType();
		var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));
		dynamic handler = provider.GetRequiredService(handlerType);
		return handler.Handle((dynamic)command, ct);
	}

	public Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken ct = default)
		where TQuery : IQuery<TResult>
	{
		var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
		return handler.Handle(query, ct);
	}

	public Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken ct = default)
	{
		var queryType = query.GetType();
		var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
		dynamic handler = provider.GetRequiredService(handlerType);
		return handler.Handle((dynamic)query, ct);
	}

	public async Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
		where TNotification : INotification
	{
		var handlers = provider.GetServices<INotificationHandler<TNotification>>();
		foreach (var handler in handlers)
			await handler.Handle(notification, ct);
	}
}
