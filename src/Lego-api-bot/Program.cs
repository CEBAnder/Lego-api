using Lego_api_bot.Features;
using System.Threading;
using System.Threading.Tasks;

namespace Lego_api_bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await BotInitializer.StartWork();

            Thread.Sleep(int.MaxValue);
        }
    }
}
