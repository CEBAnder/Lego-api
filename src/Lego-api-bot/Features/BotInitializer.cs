using Lego_api_bot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Lego_api_bot.Features
{
    public class BotInitializer
    {
        private readonly TelegramBotClient _botClient;
        private readonly MessageProcessor _messageProcessor;
        private readonly ILogger<BotInitializer> _logger;

        public BotInitializer(IConfiguration config, MessageProcessor messageProcessor, ILogger<BotInitializer> logger)
        {
            var token = config["BotToken"] ?? throw new ArgumentNullException(nameof(config));
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);            
            _botClient = new TelegramBotClient(token, client);
        }

        public async Task StartWork()
        {
            try
            {
                var me = await _botClient.GetMeAsync();
                _botClient.StartReceiving();
                _logger.LogInformation($"My id: {me.Id} my name: {me.FirstName} {me.LastName}");                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got error while starting bot's work");
                throw;
            }
            
            _botClient.OnMessage += ProcessMessage;
            _botClient.OnCallbackQuery += _botClient_OnCallbackQuery;
            _botClient.OnInlineQuery += _botClient_OnInlineQuery;            
        }

        private void _botClient_OnInlineQuery(object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
        {
            _logger.LogInformation("_botClient_OnInlineQuery");
        }

        private void _botClient_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            _logger.LogInformation("_botClient_OnCallbackQuery");
        }

        private async void ProcessMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {
                var message = e.Message;
                var response = _messageProcessor.ProcessMessage(message);
                await SendResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got error while processing message");
            }
        }

        private async Task SendResponse(ResponseParams response)
        {
            await _botClient.SendTextMessageAsync(response.ChatId, response.ResponseText, 
                replyMarkup: response.ResponseMarkup);
        }
    }
}
