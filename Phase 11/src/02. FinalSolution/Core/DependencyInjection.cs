namespace Core;

using System.Reflection;
using Core.Stock.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateStockCommand).Assembly);

        AssemblyScanner.FindValidatorsInAssembly(typeof(CreateStockCommand).Assembly)
            .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
