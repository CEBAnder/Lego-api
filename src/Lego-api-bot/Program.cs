using Lego_api_bot.Features;
using Lego_api_data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lego_api_bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = GetConfiguration();
            var services = ConfigureServices(config);
            
            var serviceProvider = services.BuildServiceProvider();
            var botInitializer = serviceProvider.GetService<BotInitializer>();
            await botInitializer.StartWork();

            Thread.Sleep(int.MaxValue);
        }

        private static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build();

            return config;
        }

        private static IServiceCollection ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddDbContext<LegoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnectionString")));

            var defaultLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "[{Timestamp:dd.MM HH:mm:ss}][{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            services.AddLogging(x => x.AddSerilog(defaultLogger, true));

            services.AddSingleton(configuration);

            services.AddSingleton<BotInitializer>();
            services.AddSingleton<MessageProcessor>();

            return services;
        }
    }
}
