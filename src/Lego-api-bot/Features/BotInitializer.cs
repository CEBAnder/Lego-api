using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Lego_api_bot.Features
{
    public class BotInitializer
    {
        static TelegramBotClient _botClient;
        static ILogger _logger;

        static BotInitializer()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build();

            _logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient(clientHandler);

            var token = config["BotToken"];
            _botClient = new TelegramBotClient(token, client);
        }

        public static async Task StartWork()
        {
            var me = await _botClient.GetMeAsync();
            _logger.Information($"My id: {me.Id} my name: {me.FirstName} {me.LastName}");
            
            _botClient.StartReceiving();
            _botClient.OnMessage += ProcessMessage;
        }

        private static async void ProcessMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            await _botClient.SendTextMessageAsync(message.Chat, "Hello world");
        }
    }
}
