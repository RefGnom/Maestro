using Maestro.Client.Daemon;
using Maestro.Core.Logging;
using Maestro.Operational.ProcessesCore;

namespace Maestro.Daemon.Implementation;

public class CompletedReminderCleaner(
    ILog<CompletedReminderCleaner> log,
    IDaemonMaestroApiClient daemonMaestroApiClient
) : RegularProcessBase<CompletedReminderCleaner>(log)
{
    public override string ProcessName => "Процесс по очистке напоминаний из бд";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Interval => TimeSpan.FromDays(1);

    protected async override Task UnsafeRunAsync()
    {
        await foreach (var reminderWithIdDto in daemonMaestroApiClient.GetCompletedRemindersAsync())
        {
            await daemonMaestroApiClient.DeleteReminder(reminderWithIdDto.ReminderId);
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}