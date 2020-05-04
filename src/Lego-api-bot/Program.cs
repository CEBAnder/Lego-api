using Lego_api_bot.Extensions;
using Lego_api_bot.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var serviceProvider = ConfigureServices(config);

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

        private static ServiceProvider ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services
                .AddConfuguration(configuration)
                .AddDatabase(configuration)
                .AddCustomLogging(configuration)
                .AddFeatures();

            return services.BuildServiceProvider();
        }
    }
}
