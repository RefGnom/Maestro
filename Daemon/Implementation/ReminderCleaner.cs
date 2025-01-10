using Maestro.Core.Logging;
using Maestro.Operational.ProcessesCore;

namespace Maestro.Daemon.Implementation;

public class ReminderCleaner(ILog<ReminderCleaner> log) : RegularProcessBase<ReminderCleaner>(log)
{
    public override string ProcessName => "Процесс по очистке напоминания из бд";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Interval => TimeSpan.FromDays(1);
    protected override Task TryRunAsync()
    {
        throw new NotImplementedException();
    }
}