using Lego_api_bot.Models;
using Lego_api_data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Lego_api_bot.Features
{
    public class MessageProcessor
    {
        private readonly LegoDbContext _dbContext;
        private readonly ILogger<MessageProcessor> _logger;

        public MessageProcessor(LegoDbContext legoDbContext, ILogger<MessageProcessor> logger)
        {
            _dbContext = legoDbContext ?? throw new ArgumentNullException(nameof(legoDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResponseParams> ProcessMessage(long chatId, string textData)
        {
            _logger.LogInformation($"Processing message with text: {textData}");
            switch (textData)
            {
                case "/start":
                    return CreateWelcomeMessage(chatId);
                case "По темам":
                    return await CreateMessageWithThemesSearch(chatId, 1);
                case "По году выхода":
                    return null;
            }

            return await CreateMessageWithThemesSearch(chatId, Convert.ToInt32(textData));
        }

        private ResponseParams CreateWelcomeMessage(long chatId)
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

        private async Task<ResponseParams> CreateMessageWithThemesSearch(long chatId, int pageNum)
        {
            var pageSize = 10;
            var pageIndex = pageNum - 1;

            var themes = await _dbContext.Themes.Skip(pageSize * pageIndex).Take(pageSize).OrderBy(x => x.ThemeId).ToListAsync();

            var sb = new StringBuilder();
            for(int i = 0; i < themes.Count; i++)
            {
                sb.Append($"{i + 1 + pageSize * pageIndex}. <b>{themes[i].Name}</b>{Environment.NewLine}")
                    .Append($"  Наборы по этой теме: /tid_{themes[i].ThemeId}{Environment.NewLine}");
            }

            var totalCount = _dbContext.Themes.Count();
            var hasRemaining = totalCount % pageSize != 0;
            var pagesCount = totalCount / pageSize + (hasRemaining ? 1 : 0);

            var pagingButtons = PagingButtonCreator.CreatePagingButtons(pagesCount, pageNum);

            var response = new ResponseParams(chatId, sb.ToString())
            {
                ResponseMarkup = new InlineKeyboardMarkup(pagingButtons)
            };
            return response;
        }
    }
}
