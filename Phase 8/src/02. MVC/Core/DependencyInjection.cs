namespace Core;

using System.Reflection;
using Core.Behaviours;
using Core.Todos.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddLazyCache();
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddTodoCommand>());
		AssemblyScanner.FindValidatorsInAssembly(typeof(AddTodoCommand).Assembly)
			.ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		return services;
	}
}