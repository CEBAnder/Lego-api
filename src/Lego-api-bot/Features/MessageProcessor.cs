using Lego_api_bot.Models;
using Lego_api_data;
using Lego_api_data.Models;
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
        
        private const string ThemeSourceName = "themes";
        private const string YearsSourceName = "years";
        private const string LinkPrefix = "rebrickable.com/sets/";

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
                    return await CreateMessageWithYearsSearch(chatId, 1);
            }

            if (textData.StartsWith('/'))
            {
                var filterData = textData.Split('_');
                var searchParam = filterData[0];
                var filterValue = Convert.ToInt32(filterData[1]);
                
                switch (searchParam)
                {
                    case "/tid":
                        return await CreateMessageWithSetsForTheme(chatId, 1, filterValue);
                    case "/yid":
                        return await CreateMessageWithSetsForYear(chatId, 1, filterValue);
                }
            }

            var messageData = textData.Split('/');
            var sourceName = messageData[0];
            var pageNum = Convert.ToInt32(messageData[1]);

            switch (sourceName)
            {
                case ThemeSourceName:
                    return await CreateMessageWithThemesSearch(chatId, pageNum);
                case YearsSourceName:
                    return await CreateMessageWithYearsSearch(chatId, pageNum);
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

            var themes = await _dbContext.Themes
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var sb = new StringBuilder();
            for(int i = 0; i < themes.Count; i++)
            {
                sb.Append($"{i + 1 + pageSize * pageIndex}. <b>{themes[i].Name}</b>{Environment.NewLine}")
                    .Append($"      Наборы по этой теме: /tid_{themes[i].ThemeId}{Environment.NewLine}");
            }

            var totalCount = await _dbContext.Themes.Where(x => x.ParentId == null).CountAsync();
            var hasRemaining = totalCount % pageSize != 0;
            var pagesCount = totalCount / pageSize + (hasRemaining ? 1 : 0);

            var pagingButtons = PagingButtonCreator.CreatePagingButtons(pagesCount, pageNum, ThemeSourceName);

            var response = new ResponseParams(chatId, sb.ToString())
            {
                ResponseMarkup = pagingButtons
            };
            return response;
        }

        private async Task<ResponseParams> CreateMessageWithYearsSearch(long chatId, int pageNum)
        {
            var pageSize = 10;
            var pageIndex = pageNum - 1;

            var years = await _dbContext.Sets
                .Select(x => x.Year)
                .Distinct()
                .OrderBy(x => x)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var sb = new StringBuilder();
            for (int i = 0; i < years.Count; i++)
            {
                sb.Append($"{i + 1 + pageSize * pageIndex}. <b>{years[i]}</b>{Environment.NewLine}")
                    .Append($"      Наборы в этом году: /yid_{years[i]}{Environment.NewLine}");
            }

            var totalCount = await _dbContext.Sets.Select(x => x.Year).Distinct().CountAsync();
            var hasRemaining = totalCount % pageSize != 0;
            var pagesCount = totalCount / pageSize + (hasRemaining ? 1 : 0);

            var pagingButtons = PagingButtonCreator.CreatePagingButtons(pagesCount, pageNum, YearsSourceName);

            var response = new ResponseParams(chatId, sb.ToString())
            {
                ResponseMarkup = pagingButtons
            };
            return response;
        }

        private async Task<ResponseParams> CreateMessageWithSetsForTheme(long chatId, int pageNum, int themeId)
        {
            var pageSize = 10;
            var pageIndex = pageNum - 1;

            var themeIds = await _dbContext.Themes
                .Where(x => x.ThemeId == themeId || x.ParentId == themeId)
                .Select(x => x.ThemeId)
                .ToListAsync();

            var totalCount2 = await _dbContext.Sets.Where(x => themeIds.Contains(x.ThemeId)).CountAsync();

            var sets = await _dbContext.Sets
                .Where(x => themeIds.Contains(x.ThemeId))
                .OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var sb = new StringBuilder();
            for (int i = 0; i < sets.Count; i++)
            {
                sb.Append($"{i + 1 + pageSize * pageIndex}. <b>{sets[i].Name}</b>{Environment.NewLine}")
                    .Append($"      Подробнее про этот набор: {LinkPrefix}{sets[i].SetNumber}{Environment.NewLine}");
            }

            var totalCount = await _dbContext.Sets.Where(x => themeIds.Contains(x.ThemeId)).CountAsync();
            var hasRemaining = totalCount % pageSize != 0;
            var pagesCount = totalCount / pageSize + (hasRemaining ? 1 : 0);

            var pagingButtons = PagingButtonCreator.CreatePagingButtons(pagesCount, pageNum, YearsSourceName);

            var response = new ResponseParams(chatId, sb.ToString())
            {
                ResponseMarkup = pagingButtons
            };
            return response;
        }

        private async Task<ResponseParams> CreateMessageWithSetsForYear(long chatId, int pageNum, int year)
        {
            var pageSize = 10;
            var pageIndex = pageNum - 1;

            var sets = await _dbContext.Sets
                .Where(x => x.Year == year)
                .OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var sb = new StringBuilder();
            for (int i = 0; i < sets.Count; i++)
            {
                sb.Append($"{i + 1 + pageSize * pageIndex}. <b>{sets[i].Name}</b>{Environment.NewLine}")
                    .Append($"      Подробнее про этот набор: {LinkPrefix}{sets[i].SetNumber}{Environment.NewLine}");
            }

            var totalCount = await _dbContext.Sets.Where(x => x.Year == year).CountAsync();
            var hasRemaining = totalCount % pageSize != 0;
            var pagesCount = totalCount / pageSize + (hasRemaining ? 1 : 0);

            var pagingButtons = PagingButtonCreator.CreatePagingButtons(pagesCount, pageNum, YearsSourceName);

            var response = new ResponseParams(chatId, sb.ToString())
            {
                ResponseMarkup = pagingButtons
            };
            return response;
        }
    }
}
