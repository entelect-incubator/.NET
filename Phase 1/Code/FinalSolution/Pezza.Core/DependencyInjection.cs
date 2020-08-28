namespace Pezza.Core
{
    using System.Reflection;
    using AutoMapper;
    using Pezza.Common.Behaviours;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Pezza.DataAccess.Data;
    using Pezza.DataAccess.Contracts;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.AddTransient(typeof(IOrderDataAccess), typeof(OrderDataAccess));
            services.AddTransient(typeof(IStockDataAccess), typeof(StockDataAccess));
            services.AddTransient(typeof(INotifyDataAccess), typeof(NotifyDataAccess));
            services.AddTransient(typeof(IProductDataAccess), typeof(ProductDataAccess));
            services.AddTransient(typeof(ICustomerDataAccess), typeof(CustomerDataAccess));
            services.AddTransient(typeof(IRestaurantDataAccess), typeof(RestaurantDataAccess));

            return services;
        }
    }
}
