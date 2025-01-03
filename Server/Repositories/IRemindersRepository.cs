using Maestro.Data.Models;
using Maestro.Server.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Repositories;

public interface IRemindersRepository
{
    Task<List<ReminderDbo>> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId, CancellationToken cancellationToken);
    Task<List<ReminderDbo>> GetForUserInTimeRangeAsync(RemindersForUserWithTimeRangeDto remindersForUserDto, long integratorId, CancellationToken cancellationToken);
    Task<ReminderDbo?> GetByIdAsync(ReminderIdDto reminderIdDto, long integratorId, CancellationToken cancellationToken);
    Task<long> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken);
    Task MarkAsCompleted(long[] remindersId, long integratorId, CancellationToken cancellationToken);
}