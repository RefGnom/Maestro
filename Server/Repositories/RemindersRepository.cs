using Maestro.Data;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class RemindersRepository(DataContext dataContext) : IRemindersRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<List<ReminderDbo>> GetForUser(long userId, CancellationToken cancellationToken)
    {
        var remindersDboList = await _dataContext.Reminders.Where(reminderDbo => reminderDbo.UserId == userId).ToListAsync(cancellationToken);
        return remindersDboList;
    }

    public async Task AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken)
    {
        await _dataContext.Reminders.AddAsync(reminderDbo, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}