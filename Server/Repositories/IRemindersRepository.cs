using Maestro.Data.Models;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Server.Repositories.Results.Reminders;

namespace Maestro.Server.Repositories;

public interface IRemindersRepository
{
    Task<AllRemindersRepositoryResult> GetAllRemindersAsync(AllRemindersDto allRemindersDto, long integratorId, CancellationToken cancellationToken);
    Task<RemindersForUserRepositoryResult> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId, CancellationToken cancellationToken);
    Task<ReminderByIdRepositoryResult> GetByIdAsync(long reminderId, long integratorId, CancellationToken cancellationToken);
    Task<AddReminderRepositoryResult> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken);
    Task<SetRemindersCompletedRepositoryResult> SetRemindersCompleted(ReminderIdDto reminderIdDto, long integratorId, CancellationToken cancellationToken);
    Task<DecrementRemindCountRepositoryResult> DecrementRemindCountAsync(long reminderId, long integratorId, CancellationToken cancellationToken);
    Task<SetReminderDateTimeRepositoryResult> SetReminderDateTimeAsync(SetReminderDateTimeDto setReminderDateTimeDto, long integratorId, CancellationToken cancellationToken);
}