namespace Utilities.CQRS;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

public interface ICommand<TResult> { }
public interface IQuery<TResult> { }
public interface INotification { }

public interface ICommandHandler<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	Task<TResult> Handle(TCommand command, CancellationToken ct);
}

public interface IQueryHandler<TQuery, TResult>
	where TQuery : IQuery<TResult>
{
	Task<TResult> Handle(TQuery query, CancellationToken ct);
}

public interface INotificationHandler<TNotification>
	where TNotification : INotification
{
	Task Handle(TNotification notification, CancellationToken ct);
}

public class Dispatcher(IServiceProvider provider)
{
	// Command overload that infers TResult from the command type at call sites.
	public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default)
		=> this.Send((dynamic)command, ct);

	// Query overload for controller/job ergonomics.
	public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken ct = default)
		=> this.Query((dynamic)query, ct);

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
		{
			await handler.Handle(notification, ct);
		}
	}

	public Task Publish(INotification notification, CancellationToken ct = default)
		=> this.Publish((dynamic)notification, ct);
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