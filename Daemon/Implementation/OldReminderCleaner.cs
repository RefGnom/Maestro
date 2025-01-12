using Maestro.Client.Daemon;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Operational.ProcessesCore;

namespace Maestro.Daemon.Implementation;

public class OldReminderCleaner(
    ILog<OldReminderCleaner> log,
    IDaemonMaestroApiClient daemonMaestroApiClient,
    IDateTimeProvider dateTimeProvider
) : RegularProcessBase<OldReminderCleaner>(log)
{
    private readonly IDaemonMaestroApiClient _daemonMaestroApiClient = daemonMaestroApiClient;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public override string ProcessName => "Процесс по очистке старых напоминаний";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Interval => TimeSpan.FromDays(1);

    protected async override Task UnsafeRunAsync()
    {
        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();
        var endDateTime = currentDateTime - TimeSpan.FromDays(14);

        await foreach (var reminderWithIdDto in _daemonMaestroApiClient.GetOldRemindersAsync(endDateTime))
        {
            await _daemonMaestroApiClient.DeleteReminderAsync(reminderWithIdDto.ReminderId);
            await Task.Delay(250);
        }
    }
}