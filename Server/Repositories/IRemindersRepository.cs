using Maestro.Data.Models;

namespace Maestro.Server.Repositories;

public interface IRemindersRepository
{
    Task<List<ReminderDbo>> GetForUser(long userId, CancellationToken cancellationToken);
    Task AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken);
}