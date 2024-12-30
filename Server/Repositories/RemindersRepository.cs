using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class RemindersRepository(DataContext dataContext) : IRemindersRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<List<ReminderDbo>> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId, CancellationToken cancellationToken)
    {
        var remindersDboList = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.UserId == remindersForUserDto.UserId && reminderDbo.IntegratorId == integratorId)
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
}