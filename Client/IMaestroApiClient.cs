using Maestro.Server.Core.Models;

namespace Maestro.Client;

public interface IMaestroApiClient
{
    public Task<long> CreateReminderAsync(ReminderDto reminder);
    public Task<ReminderDto?> GetReminderAsync(long reminderId);
    public IAsyncEnumerable<ReminderWithIdDto> GetRemindersForUserAsync(long userId, DateTime? exclusiveStartDateTime);
    public Task MarkRemindersAsCompletedAsync(params long[] remindersId);
    //public Task<long> CreateScheduleAsync(ScheduleDto schedule);
    //public IAsyncEnumerable<ScheduleDto> GetSchedulesForUserAsync(long userId, DateTime? exclusiveStartDateTime);
}