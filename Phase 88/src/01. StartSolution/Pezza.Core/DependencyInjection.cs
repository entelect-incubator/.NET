namespace Core;

using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Common.Behaviours;
using Common.Profiles;
using Core.Stock.Commands;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateStockCommand).GetTypeInfo().Assembly);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        AssemblyScanner.FindValidatorsInAssembly(typeof(CreateStockCommand).Assembly)
            .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        ////services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
