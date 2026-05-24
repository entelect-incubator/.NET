namespace Core;

using Core.Pizza.Commands;
using Core.Pizza.Queries;
using Microsoft.Extensions.DependencyInjection;
using Utilities.CQRS;

/// <summary>
/// Dependency injection configuration for the Core application layer.
/// Registers custom Dispatcher and core application services.
/// </summary>
public static class DependencyInjection
{
	/// <summary>
	/// Adds application services to the dependency injection container.
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	/// <returns>The configured service collection</returns>
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<Dispatcher>();

		services.Scan(scan => scan
			.FromAssemblyOf<CreatePizzaHandler>()
			.AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime());

		services.Scan(scan => scan
			.FromAssemblyOf<GetPizzaHandler>()
			.AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime());

		return services;
	}
}