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
		services.AddScoped<Dispatcher>();

		var assembly = typeof(CreateCustomerCommand).Assembly;

		services.Scan(scan => scan
			.FromAssemblies(assembly)
			.AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime()
			.AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime());

		AssemblyScanner.FindValidatorsInAssembly(assembly)
		   .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}