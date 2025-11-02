namespace Core;

using Core.Pizza.Commands;
using Core.Pizza.Queries;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency injection configuration for the Core application layer.
/// Registers LiteBus mediators and core application services.
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
		services.Scan(scan => scan
			.FromAssemblyOf<ICreatePizzaCommand>()
			.AddClasses(c => c.InNamespaces(
				"Core.Pizza.Commands",
				"Core.Customer.Commands"))
				.AsImplementedInterfaces()
				.WithScopedLifetime());

		services.Scan(scan => scan
			.FromAssemblyOf<IGetPizzaQuery>()
			.AddClasses(c => c.InNamespaces(
				"Core.Pizza.Queries",
				"Core.Customer.Queries"))
				.AsImplementedInterfaces()
				.WithScopedLifetime());

		return services;
	}
}