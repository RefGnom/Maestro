using Maestro.Server.Public.Models.Reminders;

namespace Maestro.Client.Daemon;

public interface IDaemonMaestroApiClient
{
    public IAsyncEnumerable<ReminderWithIdDto> GetCompletedRemindersAsync();
    public IAsyncEnumerable<ReminderWithIdDto> GetOldRemindersAsync(DateTime inclusiveBeforeDateTime);
    public Task DeleteReminderAsync(long reminderId);
}