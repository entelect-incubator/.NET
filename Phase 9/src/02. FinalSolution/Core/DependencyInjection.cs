namespace Core;

using System.Reflection;
using Common.Profiles;
using Core.Stock.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<Dispatcher>();

        AssemblyScanner.FindValidatorsInAssembly(typeof(CreateStockCommand).Assembly)
            .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
