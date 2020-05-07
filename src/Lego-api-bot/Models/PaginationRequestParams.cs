namespace Lego_api_bot.Models
{
    public class PaginationRequestParams
    {
        public string FilterSourceName { get; set; }

        public int FilterSourceValue { get; set; }

        public int PageNum { get; set; }

        public static PaginationRequestParams FromString(string stringToParse, char separator = '_')
        {
            var dataElements = stringToParse.Split(separator);
            if (!int.TryParse(dataElements[1], out int filteredSourceValue))
            {
                filteredSourceValue = 0;
            }
            var pageNum = 1;
            if (dataElements.Length > 2)
            {
                int.TryParse(dataElements[2], out pageNum);
            }
            return new PaginationRequestParams
            {
                FilterSourceName = dataElements[0],
                FilterSourceValue = filteredSourceValue,
                PageNum = pageNum
            };
        }
    }
}
