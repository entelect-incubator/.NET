namespace Pezza.Core
{
    using System.Reflection;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Pezza.Common.Behaviours;
    using Pezza.DataAccess.Contracts;
    using Pezza.DataAccess.Data;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IDataAccess<Common.Entities.Order>), typeof(OrderDataAccess));
            services.AddTransient(typeof(IDataAccess<Common.Entities.Stock>), typeof(StockDataAccess));
            services.AddTransient(typeof(IDataAccess<Common.Entities.Notify>), typeof(NotifyDataAccess));
            services.AddTransient(typeof(IDataAccess<Common.Entities.Product>), typeof(ProductDataAccess));
            services.AddTransient(typeof(IDataAccess<Common.Entities.Customer>), typeof(CustomerDataAccess));
            services.AddTransient(typeof(IDataAccess<Common.Entities.Restaurant>), typeof(RestaurantDataAccess));

            return services;
        }
    }
}
