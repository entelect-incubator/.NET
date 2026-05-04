namespace Core;

using System.Reflection;
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

public interface IQuery<TResult>
{
}

public interface IQueryHandler<TQuery, TResult>
	where TQuery : IQuery<TResult>
{
	Task<TResult> Handle(TQuery query, CancellationToken ct);
}

public interface IRequestExceptionAction<TRequest, TException>
	where TException : Exception
{
	Task Execute(TRequest request, TException exception, CancellationToken ct);
}

public interface IRequestExceptionHandler<TRequest, TResult, TException>
	where TException : Exception
{
	Task<TResult> Handle(TRequest request, TException exception, CancellationToken ct);
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

	public async Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
		where TNotification : INotification
	{
		var handlers = provider.GetServices<INotificationHandler<TNotification>>();
		foreach (var handler in handlers)
		{
			await handler.Handle(notification, ct);
		}
	}

	// Compose behaviors in registration order (outermost = first registered) and execute the handler through the chain.
	private async Task<TResult> ExecutePipeline<TRequest, TResult>(TRequest request, Func<Task<TResult>> handler, CancellationToken ct)
	{
		var behaviors = provider.GetServices<IPipelineBehavior<TRequest, TResult>>().ToList();

		// Build the pipeline chain
		var next = handler;
		for (var i = behaviors.Count - 1; i >= 0; i--)
		{
			var behavior = behaviors[i];
			var currentNext = next;
			next = () => behavior.Handle(request, currentNext, ct);
		}

		try
		{
			return await next();
		}
		catch (Exception ex)
		{
			await this.ProcessExceptionActions(request, ex, ct);
			var (success, result) = await this.TryHandleException<TRequest, TResult>(request, ex, ct);
			if (success)
			{
				return result!;
			}

			throw;
		}
	}

	private async Task ProcessExceptionActions<TRequest>(TRequest request, Exception ex, CancellationToken ct)
	{
		// Execute all actions sorted by priority
		var actions = this.GetExceptionActions<TRequest, Exception>().ToList();
		foreach (var action in actions)
		{
			await action.Execute(request, ex, ct);
		}
	}

	private async Task<(bool success, TResult? result)> TryHandleException<TRequest, TResult>(TRequest request, Exception ex, CancellationToken ct)
	{
		// Only one handler should produce a result.
		var handlers = this.GetExceptionHandlers<TRequest, TResult>(ex).ToList();
		foreach (var handler in handlers)
		{
			var result = await handler.Handle(request, ex, ct);
			return (true, result);
		}

		return (false, default);
	}

	// Priority sorting utilities
	private IEnumerable<IRequestExceptionAction<TRequest, TEx>> GetExceptionActions<TRequest, TEx>() where TEx : Exception
	{
		var all = provider.GetServices<IRequestExceptionAction<TRequest, TEx>>();
		return SortByPriority(typeof(TRequest), all);
	}

	private IEnumerable<IRequestExceptionHandler<TRequest, TResult, Exception>> GetExceptionHandlers<TRequest, TResult>(Exception ex)
	{
		// Resolve handlers registered for base Exception; priority sorting decides best match.
		var handlers = provider.GetServices<IRequestExceptionHandler<TRequest, TResult, Exception>>();
		return SortByPriority(typeof(TRequest), handlers);
	}

	// Placeholder for future per-exception-type resolution if needed.
	private static IEnumerable<T> SortByPriority<T>(Type requestType, IEnumerable<T> items)
	{
		var reqAsm = requestType.Assembly;
		var reqNs = requestType.Namespace ?? string.Empty;

		return items.OrderByDescending(item => PriorityScore(reqAsm, reqNs, item!.GetType()));
	}

	private static int PriorityScore(Assembly reqAsm, string reqNs, Type itemType)
	{
		var score = 0;
		var itemAsm = itemType.Assembly;
		var itemNs = itemType.Namespace ?? string.Empty;

		if (itemAsm == reqAsm) score += 4;
		if (itemNs.StartsWith(reqNs, StringComparison.Ordinal)) score += 2;
		if (reqNs.Contains(itemNs, StringComparison.Ordinal)) score += 1;

		return score;
	}
}
