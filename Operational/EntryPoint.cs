using Maestro.Core.Configuration;

namespace Maestro.Operational;

public static class EntryPoint
{
    public static async Task Main()
    {
        await ApplicationRunner.RunAsync<OperationalApplication, OperationalConfigurator>();
    }
}