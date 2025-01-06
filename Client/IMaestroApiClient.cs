using Maestro.Server.Core.Models;

namespace Maestro.Client;

public interface IMaestroApiClient
{
    public Task<long> CreateReminderAsync(ReminderDto reminder);
    public Task<ReminderDto?> GetReminderAsync(long reminderId);
    public IAsyncEnumerable<ReminderWithIdDto> GetAllRemindersAsync(DateTime exclusiveStartDateTime);
    public IAsyncEnumerable<ReminderWithIdDto> GetRemindersForUserAsync(long userId, DateTime? exclusiveStartDateTime);
    public Task MarkRemindersAsCompletedAsync(params long[] remindersIds);
}