using Maestro.Core.Configuration;

namespace Maestro.TelegramIntegrator;

internal static class Program
{
    private static async Task Main()
    {
        await ApplicationRunner.RunAsync<ClientApplication, ClientConfigurator>();
    }
}