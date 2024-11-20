using Core.DateTime;
using Core.IO;
using Core.Logging;
using Data;

namespace Client;
internal class Program
{   
    private static async Task Main(string[] args)
    {
        await using var dataContext = await DataContext.GetAsync();
        string botToken = "7841520460:AAFu3SqPIx8T7leJAym5wVKVom2yn9_JTv4";
        var tcs = new TaskCompletionSource(); 
        var cts = new CancellationTokenSource();
        var bot = new MaestroBotRunner(botToken, new Log(new DateTimeProvider(), new Writer()));
        bot.Start(cts);
        await tcs.Task;
    }
}