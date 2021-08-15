namespace Pezza.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using Pezza.Common.Profiles;
    using Pezza.Core.Contracts;
    using Pezza.DataAccess.Contracts;
    using Pezza.DataAccess.Data;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IStockCore), typeof(StockCore));
            services.AddTransient(typeof(IStockDataAccess), typeof(StockDataAccess));
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
