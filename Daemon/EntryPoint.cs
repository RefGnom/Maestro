using Maestro.Core.Configuration;
using Maestro.Daemon.Configuration;

namespace Maestro.Daemon;

public static class EntryPoint
{
    public static async Task Main()
    {
        await ApplicationRunner.RunAsync<DaemonApplication, DaemonConfigurator>();
    }
}