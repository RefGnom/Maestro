using Maestro.Core.Logging;
using Maestro.Operational.RegularProcesses.ProcessCore;

namespace Maestro.Operational.RegularProcesses;

public class DetectExpiredEventsProcess(ILogFactory logFactory) : RegularProcessBase(logFactory.CreateLog<DetectExpiredEventsProcess>())
{
    public override string ProcessName => ProcessNames.DetectExpiredEventsProcessName;
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Timeout => TimeSpan.FromSeconds(3);

    protected override Task RunAsync()
    {
        Console.WriteLine("Эвенты очищенны");
        return Task.CompletedTask;
    }
}