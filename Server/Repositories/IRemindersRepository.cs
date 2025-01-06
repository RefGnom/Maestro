using Maestro.Data.Models;
using Maestro.Server.Core.Models;

namespace Maestro.Server.Repositories;

public interface IRemindersRepository
{
    Task<List<ReminderDbo>> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId, CancellationToken cancellationToken);
    Task<ReminderDbo?> GetByIdAsync(long reminderId, long integratorId, CancellationToken cancellationToken);
    Task<long> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken);
    Task MarkAsCompleted(RemindersIdDto remindersId, long integratorId, CancellationToken cancellationToken);
}