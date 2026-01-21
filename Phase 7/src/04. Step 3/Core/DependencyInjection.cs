namespace Core;

using System.Reflection;
using Common.Behaviour;
using Core.Customer.Commands;
using Dispatch;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		var assembly = typeof(CreateCustomerCommand).Assembly;

		// Register dispatcher for CQRS operations
		services.AddScoped<Dispatcher>();

		// Register all command and query handlers using Scrutor
		services.Scan(scan => scan
			.FromAssemblies(assembly)
			.AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime()
			.AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime());

		AssemblyScanner.FindValidatorsInAssembly(typeof(CreatePizzaCommand).Assembly)
		   .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}