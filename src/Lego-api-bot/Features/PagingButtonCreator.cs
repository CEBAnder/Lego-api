using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Lego_api_bot.Features
{
    public class PagingButtonCreator
    {
        public static InlineKeyboardMarkup CreatePagingButtons(int pagesCount, int currentPage, string sourceName, int sourceVal = 1)
        {
            if (pagesCount == 1)
                return null;

            var pagingButtons = new List<InlineKeyboardButton>();
            int buttonsCount = 5;
            if (pagesCount >= 2 && 
                pagesCount <= buttonsCount)
            {
                for (int i = 1; i <= pagesCount; i++)
                {
                    pagingButtons.Add(CreateDefaultPagingButton(currentPage, i, sourceName, sourceVal));
                }
            }
            else
            {
                if (currentPage >= 1 && 
                    currentPage <= 3)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        pagingButtons.Add(CreateDefaultPagingButton(currentPage, i, sourceName, sourceVal));
                    }
                    pagingButtons.Add(CreateNextPageButton(3, sourceName, sourceVal));
                    pagingButtons.Add(CreateLastPageButton(pagesCount, sourceName, sourceVal));
                    return new InlineKeyboardMarkup(pagingButtons);
                }
                if (currentPage >= pagesCount - 2 && 
                    currentPage <= pagesCount)
                {
                    pagingButtons.Add(CreateFirstPageButton(sourceName, sourceVal));
                    pagingButtons.Add(CreatePreviousPageButton(pagesCount - 2, sourceName, sourceVal));
                    for (int i = pagesCount - 2; i <= pagesCount; i++)
                    {
                        pagingButtons.Add(CreateDefaultPagingButton(currentPage, i, sourceName, sourceVal));
                    }
                }
                else
                {
                    pagingButtons.Add(CreateFirstPageButton(sourceName, sourceVal));
                    pagingButtons.Add(CreatePreviousPageButton(currentPage, sourceName, sourceVal));
                    pagingButtons.Add(CreateDefaultPagingButton(currentPage, currentPage, sourceName, sourceVal));
                    pagingButtons.Add(CreateNextPageButton(currentPage, sourceName, sourceVal));
                    pagingButtons.Add(CreateLastPageButton(pagesCount, sourceName, sourceVal));
                }
            }

            return new InlineKeyboardMarkup(pagingButtons);
        }

        private static string FormCallbackData(int indexerValue, string sourceName, int sourceValue)
        {
            return $"{sourceName}_{sourceValue}_{indexerValue}";
        }

        private static InlineKeyboardButton CreateDefaultPagingButton(int pageNum, int indexerValue, string sourceName, int sourceValue)
        {
            if (pageNum == indexerValue)
            {
                return CreateDefaultCurrentPageButton(pageNum, sourceName, sourceValue);
            }

            return new InlineKeyboardButton
            {
                Text = $"{indexerValue}",
                CallbackData = FormCallbackData(indexerValue, sourceName, sourceValue)
            };
        }

        private static InlineKeyboardButton CreateDefaultCurrentPageButton(int pageNum, string sourceName, int sourceValue)
        {
            return new InlineKeyboardButton
            {
                Text = $"•{pageNum}•",
                CallbackData = FormCallbackData(pageNum, sourceName, sourceValue)
        };
        }

        private static InlineKeyboardButton CreateFirstPageButton(string sourceName, int sourceValue)
        {
            return new InlineKeyboardButton
            {
                Text = "<<1",
                CallbackData = FormCallbackData(1, sourceName, sourceValue)
            };
        }

        private static InlineKeyboardButton CreatePreviousPageButton(int currentPageNum, string sourceName, int sourceValue)
        {
            return new InlineKeyboardButton
            {
                Text = $"<{currentPageNum - 1}",
                CallbackData = FormCallbackData(currentPageNum - 1, sourceName, sourceValue)
            };
        }

        private static InlineKeyboardButton CreateNextPageButton(int currentPageNum, string sourceName, int sourceValue)
        {
            return new InlineKeyboardButton
            {
                Text = $"{currentPageNum + 1}>",
                CallbackData = FormCallbackData(currentPageNum + 1, sourceName, sourceValue)
            };
        }

        private static InlineKeyboardButton CreateLastPageButton(int lastPageNum, string sourceName, int sourceValue)
        {
            return new InlineKeyboardButton
            {
                Text = $"{lastPageNum}>>",
                CallbackData = FormCallbackData(lastPageNum, sourceName, sourceValue)
            };
        }
    }
}
