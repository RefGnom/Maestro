using Maestro.Core.Models;

namespace Maestro.Client;

public interface IEventsApiClient
{
    public Task CreateAsync(ReminderDto reminder);
    public Task<ReminderDto?> FindAsync(long id);
    public Task<ReminderDto[]> SelectRemindersForUserAsync(long userId);
    public Task<ReminderDto[]> SelectRemindersForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate);
    public Task MarkEventsAsCompletedAsync(params long[] eventIds);
}