using Maestro.Core.Configuration;

namespace Operational;

public static class EntryPoint
{
    public static async Task Main()
    {
        await ApplicationRunner.RunAsync<OperationalApplication, OperationalConfigurator>();
    }
}