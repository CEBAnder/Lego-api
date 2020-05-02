using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Lego_api_bot.Models
{
    public class ResponseParams
    {
        public long ChatId { get; set; }

        public string ResponseText { get; set; }

        public bool HasCallbackButtons => ResponseButtons != null;

        public List<InlineKeyboardButton> ResponseButtons { get; set; }

        public ResponseParams(long chatId, string responseText)
        {
            ChatId = chatId;
            ResponseText = responseText;
        }
    }
}
