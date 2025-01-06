using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class RemindersRepository(DataContext dataContext) : IRemindersRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<List<ReminderDbo>> GetAllRemindersAsync(AllRemindersDto allRemindersDto, long integratorId, CancellationToken cancellationToken)
    {
        var remindersDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && reminderDbo.ReminderTime > allRemindersDto.ExclusiveStartDateTime)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(allRemindersDto.Offset)
            .Take(allRemindersDto.Limit)
            .ToListAsync(cancellationToken);

        return remindersDbos;
    }

    public async Task<List<ReminderDbo>> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var remindersDbosBaseQuery = _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.UserId == remindersForUserDto.UserId && reminderDbo.IntegratorId == integratorId &&
                                  reminderDbo.IsCompleted == false);

        if (remindersForUserDto.ExclusiveStartDateTime is not null)
        {
            remindersDbosBaseQuery =
                remindersDbosBaseQuery.Where(reminderDbo => reminderDbo.ReminderTime > remindersForUserDto.ExclusiveStartDateTime);
        }

        var remindersDbos = await remindersDbosBaseQuery
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(remindersForUserDto.Offset)
            .Take(remindersForUserDto.Limit)
            .ToListAsync(cancellationToken);

        return remindersDbos;
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
        var createdReminderDbo = (await _dataContext.Reminders.AddAsync(reminderDbo, cancellationToken)).Entity;
        await _dataContext.SaveChangesAsync(cancellationToken);
        return createdReminderDbo.Id;
    }

    public async Task<List<long>?> MarkAsCompleted(RemindersIdsDto remindersIdsDto, long integratorId, CancellationToken cancellationToken)
    {
        var remindersDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && remindersIdsDto.RemindersIds.Contains(reminderDbo.Id))
            .ToListAsync(cancellationToken);

        if (remindersIdsDto.RemindersIds.Count != remindersDbos.Count)
        {
            var notFoundRemindersIds = GetRemindersIdsDifference(remindersDbos, remindersIdsDto);
            return notFoundRemindersIds;
        }

        foreach (var reminder in remindersDbos.Where(reminder => !reminder.IsCompleted))
        {
            reminder.IsCompleted = true;
        }

        await _dataContext.SaveChangesAsync(cancellationToken);
        return null;
    }

    public async Task<int?> DecrementRemindCountAsync(long reminderId, long integratorId, CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderId && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        if (reminderDbo is null)
        {
            return null;
        }

        reminderDbo.RemindCount--;
        await _dataContext.SaveChangesAsync(cancellationToken);

        return reminderDbo.RemindCount;
    }

    private static List<long> GetRemindersIdsDifference(List<ReminderDbo> remindersDbos, RemindersIdsDto remindersIdsDto)
    {
        return remindersIdsDto.RemindersIds.Except(remindersDbos.Select(reminderDbo => reminderDbo.Id)).ToList();
    }
}