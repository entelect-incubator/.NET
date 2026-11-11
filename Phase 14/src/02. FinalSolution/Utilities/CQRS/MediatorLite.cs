namespace Utilities.CQRS;

using System.Reflection;

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
	public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default)
	{
		return SendCommand(command, ct);
	}

	private async Task<TResult> SendCore<TResult>(ICommand<TResult> command, CancellationToken ct)
	{
		var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
		var handler = provider.GetRequiredService(handlerType);
		var task = (Task<TResult>)handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.Handle))!.Invoke(handler, [command, ct])!;
		return await task;
	}

	public Task<TResult> Send<TRequest, TResult>(TRequest request, CancellationToken ct = default)
	{
		return SendCore<TResult>((object)request!, ct);
	}

	private Task<TResult> SendCore<TResult>(object request, CancellationToken ct)
	{
		if (request is ICommand<TResult> command)
		{
			return SendCommand(command, ct);
		}

		if (request is IQuery<TResult> query)
		{
			return QueryCore(query, ct);
		}

		throw new ArgumentException($"Request type '{request.GetType().FullName}' is not a supported command or query.", nameof(request));
	}

	private async Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken ct)
	{
		var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
		var handler = provider.GetRequiredService(handlerType);
		var task = (Task<TResult>)handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.Handle))!.Invoke(handler, [command, ct])!;
		return await task;
	}

	public Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken ct = default)
	{
		return QueryCore(query, ct);
	}

	private async Task<TResult> QueryCore<TResult>(IQuery<TResult> query, CancellationToken ct)
	{
		var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
		var handler = provider.GetRequiredService(handlerType);
		var task = (Task<TResult>)handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.Handle))!.Invoke(handler, [query, ct])!;
		return await task;
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

public static class DispatcherRegistration
{
    public static IServiceCollection AddDispatcher(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<Dispatcher>();

        var handlerDefinitions = new HashSet<Type>
        {
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
            typeof(INotificationHandler<>),
        };

        foreach (var implementationType in assembly.GetTypes().Where(type => type is { IsAbstract: false, IsInterface: false }))
        {
            foreach (var serviceType in implementationType.GetInterfaces()
                .Where(type => type.IsGenericType && handlerDefinitions.Contains(type.GetGenericTypeDefinition())))
            {
                services.AddScoped(serviceType, implementationType);
            }
        }

        return services;
    }
}