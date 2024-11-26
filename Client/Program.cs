using Data;
using SimpleInjector;

namespace Client;

internal static class Program
{
    private static async Task Main()
    {
        var container = new Container();
        container.ConfigureServices()
            .ConfigureSettings();
        await using var dataContext = await DataContext.GetAsync();
        var tcs = new TaskCompletionSource();
        var cts = new CancellationTokenSource();
        var bot = container.GetInstance<MaestroBotRunner>();
        bot.Start(cts);
        await tcs.Task;
    }
}