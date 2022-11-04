using Microsoft.Extensions.Options;
using TshirtStore.BL.Interfaces;
using TshirtStore.BL.Services;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MsSql;

namespace TshirtStore.Extentions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ITshirtRepository, TshirtSqlRepository>();
            services.AddSingleton<IClientRepository, ClientRepository>();
            services.AddSingleton<IOrderRepository, OrderSqlRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<ITshirtService, TshirtService>();

            return services;
        }
    }
}
