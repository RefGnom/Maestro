using Maestro.Data.Models;
using Maestro.Server.Private.Models;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Server.Repositories.Results.Reminders;

namespace Maestro.Server.Repositories;

public interface IRemindersRepository
{
    Task<AllRemindersRepositoryResult> GetAllRemindersAsync(AllRemindersDto allRemindersDto, long integratorId, CancellationToken cancellationToken);
    Task<GetRemindersForUserRepositoryResult> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId,
        CancellationToken cancellationToken);
    Task<GetReminderByIdRepositoryResult> GetByIdAsync(ReminderIdDto reminderIdDto, long integratorId, CancellationToken cancellationToken);
    Task<GetCompletedRemindersRepositoryResult> GetCompletedRemindersAsync(CompletedRemindersDto completedRemindersDto,
        CancellationToken cancellationToken);
    Task<GetOldRemindersRepositoryResult> GetOldRemindersAsync(OldRemindersDto oldRemindersDto, CancellationToken cancellationToken);
    Task<AddReminderRepositoryResult> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken);
    Task<SetReminderCompletedRepositoryResult> SetReminderCompleted(ReminderIdDto reminderIdDto, long integratorId,
        CancellationToken cancellationToken);
    Task<DecrementRemindCountRepositoryResult> DecrementRemindCountAsync(ReminderIdDto reminderIdDto, long integratorId,
        CancellationToken cancellationToken);
    Task<SetReminderDateTimeRepositoryResult> SetReminderDateTimeAsync(ReminderDateTimeDto reminderDateTimeDto, long integratorId,
        CancellationToken cancellationToken);
    Task<DeleteReminderByIdRepositoryResult> DeleteReminderByIdAsync(ReminderIdDto reminderIdDto, CancellationToken cancellationToken);
}