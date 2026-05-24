namespace Core;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency injection configuration for the Core application layer.
/// Registers business logic services for Phase 2.
/// LiteBus configuration will be handled in the API layer's Startup.cs
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
		// Register business logic services
		services.AddTransient<IPizzaCore, PizzaCore>();

		return services;
	}
}