namespace Core;

using Microsoft.Extensions.DependencyInjection;
using Common.Profiles;
using Core.Contracts;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddTransient(typeof(IPizzaCore), typeof(PizzaCore));
		services.AddAutoMapper(typeof(MappingProfile));

		return services;
	}
}
