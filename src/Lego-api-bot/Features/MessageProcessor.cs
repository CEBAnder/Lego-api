using Lego_api_bot.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Lego_api_bot.Features
{
    public class MessageProcessor
    {
        private readonly ILogger<MessageProcessor> _logger;

        public MessageProcessor(ILogger<MessageProcessor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ResponseParams ProcessMessage(Message incomingMessage)
        {
            _logger.LogInformation($"Processing message with text: {incomingMessage.Text}");
            switch (incomingMessage.Text)
            {
                case "/start":
                    return CreateWelcomeMessage(incomingMessage.Chat.Id);
                case "По темам":
                    return null;
                case "По году выхода":
                    return null;
            }

            return null;
        }

        private static ResponseParams CreateWelcomeMessage(long chatId)
        {
            var welcomeText = "Привет, как будем искать наборы?";
            var byThemeButton = new KeyboardButton
            {
                Text = "По темам"                
            };
            var byYears = new KeyboardButton
            {
                Text = "По году выхода"
            };

            var response = new ResponseParams(chatId, welcomeText)
            { 
                ResponseMarkup = new ReplyKeyboardMarkup(new List<KeyboardButton> { byThemeButton, byYears })
            };
            return response;
        }

        private static ResponseParams CreateMessageWithThemesSearch(long chatId)
        {
            return null;
        }
    }
}
