using Maestro.Server.Core.Models;

namespace Maestro.Client;

public interface IMaestroApiClient
{
    public Task<long> CreateReminderAsync(ReminderDto reminder);
    public Task<ReminderDto?> GetReminderAsync(long reminderId);
    public IAsyncEnumerable<ReminderDtoWithId> GetRemindersForUserAsync(long userId);
    public Task<ReminderDto[]> GetRemindersForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate);
    public Task MarkRemindersAsCompletedAsync(params long[] remindersId);
}