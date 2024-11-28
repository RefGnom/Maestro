using Core.IO;
using Core.Logging;
using Core.Providers;
using Operational.RegularProcesses;
using Operational.RegularProcesses.ProcessCore;

namespace Operational;

public static class EntryPoint
{
    public static async Task Main()
    {
        var processRunner = new ProcessRunner();
        var logFactory = new LogFactory(new DateTimeProvider(), new Writer());
        var process = new ExpiredEventsDetectProcess(logFactory);
        processRunner.RegisterProcess(process);
        await processRunner.RunAsync();
        Console.ReadLine();
    }
}