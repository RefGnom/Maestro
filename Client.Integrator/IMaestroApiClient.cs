using Maestro.Server.Public.Models.Reminders;
using Maestro.Server.Public.Models.Schedules;

namespace Maestro.Client.Integrator;

public interface IMaestroApiClient
{
    public Task<long> CreateReminderAsync(ReminderDto reminder);
    public Task<long> CreateScheduleAsync(ScheduleDto schedule);
    public Task<ReminderDto?> GetReminderAsync(long reminderId);
    public IAsyncEnumerable<ReminderWithIdDto> GetAllRemindersAsync(DateTime exclusiveStartDateTime);
    public IAsyncEnumerable<ReminderWithIdDto> GetRemindersForUserAsync(long userId, DateTime? exclusiveStartDateTime);
    public IAsyncEnumerable<SchedulesForUserDto> GetSchedulesForUserAsync(long userId, DateTime? exclusiveStartDateTime);
    public Task SetReminderCompletedAsync(long reminderIds);
    public Task<int> DecrementRemindCountAsync(long reminderId);
    public Task SetReminderDateTimeAsync(long reminderId, DateTime dateTime);
    public Task<long?> CreateScheduleAsync(ScheduleDto reminder);
    public Task<ScheduleDto?> GetScheduleByIdAsync(long scheduleId);
    public IAsyncEnumerable<ScheduleWithIdDto> GetAllSchedulesAsync(DateTime exclusiveStartDateTime);
    public IAsyncEnumerable<ScheduleWithIdDto> GetSchedulesForUserAsync(long userId, DateTime? exclusiveStartDateTime);
    public Task SetScheduleCompletedAsync(long scheduleId);
}