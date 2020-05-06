using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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
            _botClient.OnCallbackQuery += ProcessCallback;       
        }

        private async void ProcessMessage(object sender, MessageEventArgs e)
        {
            try
            {
                var message = e.Message;
                var response = await _messageProcessor.ProcessMessage(message.Chat.Id, message.Text);
                await _botClient.SendTextMessageAsync(response.ChatId, response.ResponseText, 
                    disableWebPagePreview: true, disableNotification: true,
                    replyMarkup: response.ResponseMarkup, parseMode: ParseMode.Html);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got error while processing message");
            }
        }

        private async void ProcessCallback(object sender, CallbackQueryEventArgs e)
        {
            try
            {
                var callbackData = e.CallbackQuery;
                var response = await _messageProcessor.ProcessMessage(callbackData.Message.Chat.Id, callbackData.Data);
                await _botClient.EditMessageTextAsync(callbackData.Message.Chat.Id, callbackData.Message.MessageId, response.ResponseText,
                    disableWebPagePreview: true, 
                    replyMarkup: (InlineKeyboardMarkup)response.ResponseMarkup, parseMode: ParseMode.Html);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got error while processing callback query");
            }
        }
    }
}
