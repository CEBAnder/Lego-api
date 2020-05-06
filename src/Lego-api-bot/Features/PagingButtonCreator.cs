using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Lego_api_bot.Features
{
    public class PagingButtonCreator
    {
        public static InlineKeyboardMarkup CreatePagingButtons(int pagesCount, int currentPage, string sourceName)
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
                    pagingButtons.Add(CreateDefaultPagingButton(currentPage, i, sourceName));
                }
            }
            else
            {
                if (currentPage >= 1 && 
                    currentPage <= 3)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        pagingButtons.Add(CreateDefaultPagingButton(currentPage, i, sourceName));
                    }
                    pagingButtons.Add(CreateNextPageButton(3, sourceName));
                    pagingButtons.Add(CreateLastPageButton(pagesCount, sourceName));
                    return new InlineKeyboardMarkup(pagingButtons);
                }
                if (currentPage >= pagesCount - 2 && 
                    currentPage <= pagesCount)
                {
                    pagingButtons.Add(CreateFirstPageButton(sourceName));
                    pagingButtons.Add(CreatePreviousPageButton(pagesCount - 2, sourceName));
                    for (int i = pagesCount - 2; i <= pagesCount; i++)
                    {
                        pagingButtons.Add(CreateDefaultPagingButton(currentPage, i, sourceName));
                    }
                }
                else
                {
                    pagingButtons.Add(CreateFirstPageButton(sourceName));
                    pagingButtons.Add(CreatePreviousPageButton(currentPage, sourceName));
                    pagingButtons.Add(CreateDefaultPagingButton(currentPage, currentPage, sourceName));
                    pagingButtons.Add(CreateNextPageButton(currentPage, sourceName));
                    pagingButtons.Add(CreateLastPageButton(pagesCount, sourceName));
                }
            }

            return new InlineKeyboardMarkup(pagingButtons);
        }

        private static InlineKeyboardButton CreateDefaultPagingButton(int pageNum, int indexerValue, string sourceName)
        {
            if (pageNum == indexerValue)
            {
                return CreateDefaultCurrentPageButton(pageNum, sourceName);
            }

            return new InlineKeyboardButton
            {
                Text = $"{indexerValue}",
                CallbackData = $"{sourceName}/{indexerValue}"
            };
        }

        private static InlineKeyboardButton CreateDefaultCurrentPageButton(int pageNum, string sourceName)
        {
            return new InlineKeyboardButton
            {
                Text = $"•{pageNum}•",
                CallbackData = $"{sourceName}/{pageNum}"
            };
        }

        private static InlineKeyboardButton CreateFirstPageButton(string sourceName)
        {
            return new InlineKeyboardButton
            {
                Text = "<<1",
                CallbackData = $"{sourceName}/1"
            };
        }

        private static InlineKeyboardButton CreatePreviousPageButton(int currentPageNum, string sourceName)
        {
            return new InlineKeyboardButton
            {
                Text = $"<{currentPageNum - 1}",
                CallbackData = $"{sourceName}/{currentPageNum - 1}"
            };
        }

        private static InlineKeyboardButton CreateNextPageButton(int currentPageNum, string sourceName)
        {
            return new InlineKeyboardButton
            {
                Text = $"{currentPageNum + 1}>",
                CallbackData = $"{sourceName}/{currentPageNum + 1}"
            };
        }

        private static InlineKeyboardButton CreateLastPageButton(int lastPageNum, string sourceName)
        {
            return new InlineKeyboardButton
            {
                Text = $"{lastPageNum}>>",
                CallbackData = $"{sourceName}/{lastPageNum}"
            };
        }
    }
}
