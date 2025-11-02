namespace Core;

using System.Reflection;
using Core.Customer.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// Register LiteBus command and query handlers
		// Handlers will be registered through reflection scanning
		var assembly = typeof(CreateCustomerCommand).Assembly;

		// Register all command handlers
		var commandHandlerTypes = assembly.GetTypes()
			.Where(t => t.Name.EndsWith("CommandHandler") && !t.IsInterface && !t.IsAbstract)
			.ToList();

		foreach (var handlerType in commandHandlerTypes)
		{
			services.AddScoped(handlerType);
		}

		// Register all query handlers
		var queryHandlerTypes = assembly.GetTypes()
			.Where(t => t.Name.EndsWith("QueryHandler") && !t.IsInterface && !t.IsAbstract)
			.ToList();

		foreach (var handlerType in queryHandlerTypes)
		{
			services.AddScoped(handlerType);
		}

		// Register validators
		AssemblyScanner.FindValidatorsInAssembly(typeof(CreateCustomerCommand).Assembly)
		   .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}