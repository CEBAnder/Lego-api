using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Lego_api_bot.Features
{
    public class PagingButtonCreator
    {
        public static List<InlineKeyboardButton> CreatePagingButtons(int pagesCount, int currentPage)
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
                    pagingButtons.Add(CreateDefaultPagingButton(currentPage, i));
                }
            }
            else
            {
                if (currentPage >= 1 && 
                    currentPage <= 3)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        pagingButtons.Add(CreateDefaultPagingButton(currentPage, i));
                    }
                    pagingButtons.Add(CreateNextPageButton(3));
                    pagingButtons.Add(CreateLastPageButton(pagesCount));
                    return pagingButtons;
                }
                if (currentPage >= pagesCount - 2 && 
                    currentPage <= pagesCount)
                {
                    pagingButtons.Add(CreateFirstPageButton());
                    pagingButtons.Add(CreatePreviousPageButton(pagesCount - 2));
                    for (int i = pagesCount - 2; i <= pagesCount; i++)
                    {
                        pagingButtons.Add(CreateDefaultPagingButton(currentPage, i));
                    }
                }
                else
                {
                    pagingButtons.Add(CreateFirstPageButton());
                    pagingButtons.Add(CreatePreviousPageButton(currentPage));
                    pagingButtons.Add(CreateDefaultPagingButton(currentPage, currentPage));
                    pagingButtons.Add(CreateNextPageButton(currentPage));
                    pagingButtons.Add(CreateLastPageButton(pagesCount));
                }
            }

            return pagingButtons;
        }

        private static InlineKeyboardButton CreateDefaultPagingButton(int pageNum, int indexerValue)
        {
            if (pageNum == indexerValue)
            {
                return CreateDefaultCurrentPageButton(pageNum);
            }

            return new InlineKeyboardButton
            {
                Text = $"{indexerValue}",
                CallbackData = indexerValue.ToString()
            };
        }

        private static InlineKeyboardButton CreateDefaultCurrentPageButton(int pageNum)
        {
            return new InlineKeyboardButton
            {
                Text = $"•{pageNum}•",
                CallbackData = pageNum.ToString()
            };
        }

        private static InlineKeyboardButton CreateFirstPageButton()
        {
            return new InlineKeyboardButton
            {
                Text = "<<1",
                CallbackData = "1"
            };
        }

        private static InlineKeyboardButton CreatePreviousPageButton(int currentPageNum)
        {
            return new InlineKeyboardButton
            {
                Text = $"<{currentPageNum - 1}",
                CallbackData = (currentPageNum - 1).ToString()
            };
        }

        private static InlineKeyboardButton CreateNextPageButton(int currentPageNum)
        {
            return new InlineKeyboardButton
            {
                Text = $"{currentPageNum + 1}>",
                CallbackData = (currentPageNum + 1).ToString()
            };
        }

        private static InlineKeyboardButton CreateLastPageButton(int lastPageNum)
        {
            return new InlineKeyboardButton
            {
                Text = $"{lastPageNum}>>",
                CallbackData = lastPageNum.ToString()
            };
        }
    }
}
