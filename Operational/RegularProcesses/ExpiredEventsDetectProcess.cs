using Core.Logging;
using Operational.RegularProcesses.ProcessCore;

namespace Operational.RegularProcesses;

public class ExpiredEventsDetectProcess(ILogFactory logFactory) : RegularProcessBase(logFactory.ForContext<ExpiredEventsDetectProcess>())
{
    public override string ProcessName => "Процесс по очищению истёкших эвентов";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Timeout => TimeSpan.FromSeconds(3);

    public override Task RunAsync()
    {
        Console.WriteLine("Эвенты очищенны");
        return Task.CompletedTask;
    }
}