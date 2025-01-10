using Maestro.Server.Public.Models.Reminders;

namespace Maestro.Client.Daemon;

public interface IDaemonMaestroApiClient
{
    public IAsyncEnumerable<ReminderWithIdDto> GetCompletedRemindersAsync();
    public IAsyncEnumerable<ReminderWithIdDto> GetRemindersAsync(DateTime exclusiveStartDateTime, DateTime inclusiveEndDateTime);
    public Task DeleteReminder(long reminderId);
}