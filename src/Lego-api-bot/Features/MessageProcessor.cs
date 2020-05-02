using Lego_api_bot.Models;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Lego_api_bot.Features
{
    public class MessageProcessor
    {
        public static ResponseParams ProcessMessage(Message incomingMessage)
        {
            if (incomingMessage.Text == "/start")
                return CreateWelcomeMessage(incomingMessage.Chat.Id);
            return null;
        }

        private static ResponseParams CreateWelcomeMessage(long chatId)
        {
            var welcomeText = "Привет, как будем искать наборы?";
            var byThemeButton = new InlineKeyboardButton
            {
                Text = "По темам",
                CallbackData = "byTheme"
            };
            var byYears = new InlineKeyboardButton
            {
                Text = "По году выхода",
                CallbackData = "byYears"
            };

            var response = new ResponseParams(chatId, welcomeText)
            { 
                ResponseButtons = new List<InlineKeyboardButton> { byThemeButton, byYears }
            };
            return response;
        }
    }
}
