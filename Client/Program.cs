using Core.DateTime;
using Core.IO;
using Core.Logging;
using Data;
using Data.Models;

namespace Client;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await using var dataContext = await DataContext.GetAsync();
        string botToken = "7841520460:AAFu3SqPIx8T7leJAym5wVKVom2yn9_JTv4";
        var bot = new MaestroBot(botToken, dataContext);
        bot.Start();
        Console.ReadKey();
    }
}