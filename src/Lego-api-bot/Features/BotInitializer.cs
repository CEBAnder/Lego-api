using Lego_api_bot.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

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
            _botClient.OnCallbackQuery += _botClient_OnCallbackQuery;
            _botClient.OnInlineQuery += _botClient_OnInlineQuery;            
        }

        private static void _botClient_OnInlineQuery(object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
        {
            _logger.Information("_botClient_OnInlineQuery");
        }

        private static void _botClient_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            _logger.Information("_botClient_OnCallbackQuery");
        }

        private static void ProcessMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            var response = MessageProcessor.ProcessMessage(message);
            SendResponse(response);
        }

        private static async void SendResponse(ResponseParams response)
        {
            await _botClient.SendTextMessageAsync(response.ChatId, response.ResponseText, 
                replyMarkup: response.HasCallbackButtons ? new InlineKeyboardMarkup(response.ResponseButtons) : null);
        }
    }
}
