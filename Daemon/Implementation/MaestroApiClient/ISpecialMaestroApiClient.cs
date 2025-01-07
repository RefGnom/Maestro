using Maestro.Server.Core.Models;

namespace Daemon.Implementation.MaestroApiClient;

public interface ISpecialMaestroApiClient
{
    public IAsyncEnumerable<ReminderWithIdDto> GetCompletedRemindersAsync();
    public IAsyncEnumerable<ReminderWithIdDto> GetRemindersAsync(DateTime exclusiveStartDateTime, DateTime inclusiveEndDateTime);
    public Task DeleteReminder(long reminderId);
}