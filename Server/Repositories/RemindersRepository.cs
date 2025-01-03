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
        var remindersDboList = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.UserId == remindersForUserDto.UserId && reminderDbo.IntegratorId == integratorId)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(remindersForUserDto.Offset)
            .Take(remindersForUserDto.Limit)
            .ToListAsync(cancellationToken);

        return remindersDboList;
    }

    public async Task<List<ReminderDbo>> GetForUserInTimeRangeAsync(RemindersForUserWithTimeRangeDto remindersForUserDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var remindersDboList = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.UserId == remindersForUserDto.UserId
                              && reminderDbo.IntegratorId == integratorId
                              && reminderDbo.ReminderTime >= remindersForUserDto.InclusiveStartDate
                              && reminderDbo.ReminderTime < remindersForUserDto.ExclusiveEndDate)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(remindersForUserDto.Offset)
            .Take(remindersForUserDto.Limit)
            .ToListAsync(cancellationToken);

        return remindersDboList;
    }

    public Task<ReminderDbo?> GetByIdAsync(ReminderIdDto reminderIdDto, long integratorId, CancellationToken cancellationToken)
    {
        var reminderDbo = _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderIdDto.Id && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        return reminderDbo;
    }

    public async Task<long> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken)
    {
        var createdReminderEntity = (await _dataContext.Reminders.AddAsync(reminderDbo, cancellationToken)).Entity;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return createdReminderEntity.Id;
    }

    public async Task MarkAsCompleted(long[] remindersId, long integratorId, CancellationToken cancellationToken)
    {
        var reminders = await _dataContext.Reminders
        .Where(reminderDbo => remindersId.Contains(reminderDbo.Id) && reminderDbo.IntegratorId == integratorId)
        .ToListAsync(cancellationToken);

        foreach (var reminder in reminders)
        {
            reminder.IsCompleted = true;
        }

        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}