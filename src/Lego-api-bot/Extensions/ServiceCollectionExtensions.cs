using Lego_api_bot.Features;
using Lego_api_data;
using Lego_api_data.Helpers;
using Lego_api_data.Models;
using Lego_api_data.Repositories;
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
            services.AddDbContext<LegoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("LegoDatabase")));
            services.AddScoped<IRepository<Theme>, ThemesRepository>();
            services.AddScoped<IRepository<Set>, SetsRepository>();

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
            services.AddScoped<BotInitializer>();
            services.AddScoped<MessageProcessor>();

            return services;
        }
    }
}
