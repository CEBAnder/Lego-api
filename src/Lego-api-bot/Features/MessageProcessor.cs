using Lego_api_bot.Models;
using Lego_api_data;
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

            var themes = _dbContext.Themes.Where(x => x.ThemeId > pageSize * (pageNum - 1) && x.ThemeId <= pageSize * pageNum).OrderBy(x => x.ThemeId).ToList();            

            var sb = new StringBuilder();

            for(int i = 0; i < themes.Count; i++)
            {
                sb.Append($"{i + pageSize * (pageNum - 1) + 1}. <b>{themes[i].Name}</b>").Append(Environment.NewLine);
            }

            var totalCount = _dbContext.Themes.Count();
            var pagesCount = totalCount / pageSize + (totalCount % pageSize == 0 ? 0 : 1);
            var pagingButtons = CreatePagingButtons(pagesCount, pageNum);

            var response = new ResponseParams(chatId, sb.ToString())
            {
                ResponseMarkup = new InlineKeyboardMarkup(pagingButtons)
            };
            return response;
        }

        private List<InlineKeyboardButton> CreatePagingButtons(int pagesCount, int currentPage)
        {
            if (pagesCount == 1)
                return null;

            var pagingButtons = new List<InlineKeyboardButton>();
            if (pagesCount >= 2 && pagesCount <= 5)
            {
                for (int i = 1; i <= pagesCount; i++)
                {
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = i == currentPage ? $"•{i}•": $"{i}",
                        CallbackData = i.ToString()
                    });
                }
            }
            else
            {
                if (currentPage >= 1 && currentPage <= 3)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        pagingButtons.Add(new InlineKeyboardButton
                        {
                            Text = i == currentPage ? $"•{i}•" : $"{i}",
                            CallbackData = i.ToString()
                        });
                    }
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = "4>",
                        CallbackData = "4"
                    });
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = $"{pagesCount}>>",
                        CallbackData = pagesCount.ToString()
                    });
                    return pagingButtons;
                }
                if (currentPage >= pagesCount - 2 && currentPage <= pagesCount)
                {
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = "<<1",
                        CallbackData = "1"
                    });
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = $"<{pagesCount - 3}",
                        CallbackData = (pagesCount - 3).ToString()
                    });
                    for (int i = pagesCount - 2; i <= pagesCount; i++)
                    {
                        pagingButtons.Add(new InlineKeyboardButton
                        {
                            Text = i == currentPage ? $"•{i}•" : $"{i}",
                            CallbackData = i.ToString()
                        });
                    }
                }
                else
                {
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = "<<1",
                        CallbackData = "1"
                    });
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = $"<{currentPage - 1}",
                        CallbackData = (currentPage - 1).ToString()
                    });
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = currentPage.ToString(),
                        CallbackData = currentPage.ToString()
                    });
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = $"{currentPage + 1}>",
                        CallbackData = (currentPage + 1).ToString()
                    });
                    pagingButtons.Add(new InlineKeyboardButton
                    {
                        Text = $"{pagesCount}>>",
                        CallbackData = pagesCount.ToString()
                    });
                }
            }

            return pagingButtons;
        }
    }
}
