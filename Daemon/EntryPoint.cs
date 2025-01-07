using Daemon.Configuration;
using Maestro.Core.Configuration;

namespace Daemon;

public static class EntryPoint
{
    public static async Task Main()
    {
        await ApplicationRunner.RunAsync<DaemonApplication, DaemonConfigurator>();
    }
}