using Maestro.Core.Extensions;
using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Public.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class RemindersRepository(DataContext dataContext) : IRemindersRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<List<ReminderDbo>> GetAllRemindersAsync(AllRemindersDto allRemindersDto, long integratorId, CancellationToken cancellationToken)
    {
        var reminderDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && reminderDbo.ReminderTime > allRemindersDto.ExclusiveStartDateTime)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(allRemindersDto.Offset)
            .Take(allRemindersDto.Limit)
            .ToListAsync(cancellationToken);

        return reminderDbos;
    }

    public async Task<List<ReminderDbo>> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbosBaseQuery = _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.UserId == remindersForUserDto.UserId && reminderDbo.IntegratorId == integratorId &&
                                  reminderDbo.IsCompleted == false);

        if (remindersForUserDto.ExclusiveStartDateTime is not null)
        {
            reminderDbosBaseQuery =
                reminderDbosBaseQuery.Where(reminderDbo => reminderDbo.ReminderTime > remindersForUserDto.ExclusiveStartDateTime);
        }

        var reminderDbos = await reminderDbosBaseQuery
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(remindersForUserDto.Offset)
            .Take(remindersForUserDto.Limit)
            .ToListAsync(cancellationToken);

        return reminderDbos;
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

    public async Task<List<long>?> MarkAsCompleted(ReminderIdsDto reminderIdsDto, long integratorId, CancellationToken cancellationToken)
    {
        var reminderDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && reminderIdsDto.ReminderIds.Contains(reminderDbo.Id))
            .ToListAsync(cancellationToken);

        if (reminderIdsDto.ReminderIds.Count != reminderDbos.Count)
        {
            var notFoundReminderIds = GetReminderIdsDifference(reminderDbos, reminderIdsDto);
            return notFoundReminderIds;
        }

        reminderDbos.Where(reminderDbo => !reminderDbo.IsCompleted).ForEach(reminderDbo => reminderDbo.IsCompleted = true);

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

    private static List<long> GetReminderIdsDifference(List<ReminderDbo> reminderDbos, ReminderIdsDto reminderIdsDto)
    {
        return reminderIdsDto.ReminderIds.Except(reminderDbos.Select(reminderDbo => reminderDbo.Id)).ToList();
    }
}