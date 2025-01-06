using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class RemindersRepository(DataContext dataContext) : IRemindersRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<List<ReminderDbo>> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var remindersDbo = _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.UserId == remindersForUserDto.UserId && reminderDbo.IntegratorId == integratorId &&
                                  reminderDbo.IsCompleted == false);

        if (remindersForUserDto.ExclusiveStartDateTime is not null)
        {
            remindersDbo = remindersDbo.Where(reminderDbo => reminderDbo.ReminderTime > remindersForUserDto.ExclusiveStartDateTime);
        }

        var remindersDboList = await remindersDbo
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(remindersForUserDto.Offset)
            .Take(remindersForUserDto.Limit)
            .ToListAsync(cancellationToken);

        return remindersDboList;
    }

    public Task<ReminderDbo?> GetByIdAsync(long reminderId, long integratorId, CancellationToken cancellationToken)
    {
        var reminderDbo = _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderId && reminderDbo.IntegratorId == integratorId && reminderDbo.IsCompleted == false)
            .SingleOrDefaultAsync(cancellationToken);

        return reminderDbo;
    }

    public async Task<long> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken)
    {
        var createdReminderEntity = (await _dataContext.Reminders.AddAsync(reminderDbo, cancellationToken)).Entity;
        await _dataContext.SaveChangesAsync(cancellationToken);
        return createdReminderEntity.Id;
    }

    public async Task MarkAsCompleted(RemindersIdDto remindersId, long integratorId, CancellationToken cancellationToken)
    {
        var reminders = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && remindersId.RemindersId.Contains(reminderDbo.Id))
            .ToListAsync(cancellationToken);

        foreach (var reminder in reminders)
        {
            reminder.IsCompleted = true;
        }

        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}