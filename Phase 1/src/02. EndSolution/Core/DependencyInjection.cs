namespace Core;

using Microsoft.Extensions.DependencyInjection;
using Core.Contracts;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddTransient(typeof(IPizzaCore), typeof(PizzaCore));

		return services;
	}
}
