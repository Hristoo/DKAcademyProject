using Kafka.Services;
using ThirtStore.Models.Models;
using TshirtStore.BL.Interfaces;
using TshirtStore.BL.Services;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MondoDB;
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
            services.AddSingleton<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddSingleton<IReportSqlRepository, ReportSqlRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<ITshirtService, TshirtService>();
            services.AddSingleton<IShoppingCartService, ShoppingCartService>();
            services.AddSingleton<Producer<int, Order>>();
            services.AddHostedService<Consumer<int, Order>>();

            return services;
        }
    }
}
