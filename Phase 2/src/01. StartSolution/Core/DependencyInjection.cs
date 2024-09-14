namespace Core;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddTransient(typeof(ITodoCore), typeof(TodoCore));

		return services;
	}
}