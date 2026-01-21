namespace Core;

using System.Reflection;
using Core.Customer.Commands;
using Dispatch;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// Register custom Dispatcher
		services.AddScoped<Dispatcher>();

		// Register all command and query handlers from Core assembly
		var assembly = typeof(CreateCustomerCommand).Assembly;
		services.Scan(scan => scan
			.FromAssemblies(assembly)
			.AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime()
		);

		services.Scan(scan => scan
			.FromAssemblies(assembly)
			.AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime()
		);

		// Register FluentValidation validators
		AssemblyScanner.FindValidatorsInAssembly(typeof(CreatePizzaCommand).Assembly)
		   .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}