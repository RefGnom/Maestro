using Maestro.Data.Models;
using Maestro.Server.Public.Models;

namespace Maestro.Server.Repositories;

public interface IRemindersRepository
{
    Task<List<ReminderDbo>> GetAllRemindersAsync(AllRemindersDto allRemindersDto, long integratorId, CancellationToken cancellationToken);
    Task<List<ReminderDbo>> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId, CancellationToken cancellationToken);
    Task<ReminderDbo?> GetByIdAsync(long reminderId, long integratorId, CancellationToken cancellationToken);
    Task<long> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken);
    Task<List<long>?> MarkAsCompleted(ReminderIdsDto reminderIdsDto, long integratorId, CancellationToken cancellationToken);
    Task<int?> DecrementRemindCountAsync(long reminderId, long integratorId, CancellationToken cancellationToken);
}