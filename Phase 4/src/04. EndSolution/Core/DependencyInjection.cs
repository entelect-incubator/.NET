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
		// Register custom Dispatcher with pipeline behavior support
		services.AddScoped<Dispatcher>();

		var assembly = typeof(CreateCustomerCommand).Assembly;

		// Register all command handlers using Scrutor
		services.Scan(scan => scan
			.FromAssemblies(assembly)
			.AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime()
		);

		// Register all query handlers using Scrutor
		services.Scan(scan => scan
			.FromAssemblies(assembly)
			.AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime()
		);

		// Register validators
		AssemblyScanner.FindValidatorsInAssembly(typeof(CreateCustomerCommand).Assembly)
		   .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}