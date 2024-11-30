using Maestro.Core.Configuration.DependencyInjectionConfiguration;
using Maestro.Data;
using SimpleInjector;

namespace Maestro.Client;

internal static class Program
{
    private static async Task Main()
    {
        var container = new Container();
        container = container.Configure().ConfigureSettings();
        container.RegisterSingleton<MaestroBotRunner>();
        await using var dataContext = await DataContext.GetAsync();
        var tcs = new TaskCompletionSource();
        var cts = new CancellationTokenSource();
        var bot = container.GetInstance<MaestroBotRunner>();
        bot.Start(cts);
        await tcs.Task;
    }
}