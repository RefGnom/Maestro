using Maestro.Core.Configuration;

namespace Maestro.Client;

internal static class Program
{
    private static async Task Main()
    {
        await ApplicationRunner.RunAsync<ClientApplication, ClientConfigurator>();
    }
}