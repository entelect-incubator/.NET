namespace Core;

using System.Reflection;
using Common.CQRS;
using Core.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddLazyCache();
		services.AddScoped<Dispatcher>();

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		RegisterHandlers(services, Assembly.GetExecutingAssembly());
		return services;
	}

	private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
	{
		var handlerTypes = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract);

		foreach (var implementationType in handlerTypes)
		{
			foreach (var serviceType in implementationType.GetInterfaces())
			{
				if (!serviceType.IsGenericType)
				{
					continue;
				}

				var genericDefinition = serviceType.GetGenericTypeDefinition();
				if (genericDefinition == typeof(ICommandHandler<,>) || genericDefinition == typeof(IQueryHandler<,>) || genericDefinition == typeof(INotificationHandler<>))
				{
					services.AddScoped(serviceType, implementationType);
				}
			}
		}
	}
}