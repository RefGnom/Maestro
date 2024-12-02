using Maestro.Core.Logging;
using Operational.RegularProcesses.ProcessCore;

namespace Operational.RegularProcesses;

public class ExpiredEventsDetectProcess(ILogFactory logFactory) : RegularProcessBase(logFactory.CreateLog<ExpiredEventsDetectProcess>())
{
    public override string ProcessName => "Процесс по очищению истёкших эвентов";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Timeout => TimeSpan.FromSeconds(3);

    protected override Task RunAsync()
    {
        Console.WriteLine("Эвенты очищенны");
        return Task.CompletedTask;
    }
}