using Lego_api_bot.Features;
using Lego_api_data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Lego_api_bot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfuguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LegoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnectionString")));

            return services;
        }

        public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration, "Serilog")
                .CreateLogger();

            services.AddLogging(x => x.AddSerilog(defaultLogger, true));

            return services;
        }

        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            services.AddSingleton<BotInitializer>();
            services.AddSingleton<MessageProcessor>();

            return services;
        }
    }
}
