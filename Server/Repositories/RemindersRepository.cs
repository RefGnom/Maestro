using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Server.Repositories.Results.Reminders;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class RemindersRepository(DataContext dataContext) : IRemindersRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<AllRemindersRepositoryResult> GetAllRemindersAsync(AllRemindersDto allRemindersDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && reminderDbo.RemindDateTime > allRemindersDto.ExclusiveStartDateTime)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(allRemindersDto.Offset)
            .Take(allRemindersDto.Limit)
            .ToListAsync(cancellationToken);

        return new AllRemindersRepositoryResult(true, reminderDbos);
    }

    public async Task<GetRemindersForUserRepositoryResult> GetForUserAsync(RemindersForUserDto remindersForUserDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbosBaseQuery = _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.UserId == remindersForUserDto.UserId && reminderDbo.IntegratorId == integratorId);

        if (remindersForUserDto.ExclusiveStartDateTime is not null)
        {
            reminderDbosBaseQuery =
                reminderDbosBaseQuery.Where(reminderDbo => reminderDbo.RemindDateTime > remindersForUserDto.ExclusiveStartDateTime);
        }

        var reminderDbos = await reminderDbosBaseQuery
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(remindersForUserDto.Offset)
            .Take(remindersForUserDto.Limit)
            .ToListAsync(cancellationToken);

        return new GetRemindersForUserRepositoryResult(true, reminderDbos);
    }

    public async Task<GetReminderByIdRepositoryResult> GetByIdAsync(long reminderId, long integratorId, CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderId && reminderDbo.IntegratorId == integratorId && reminderDbo.IsCompleted == false)
            .SingleOrDefaultAsync(cancellationToken);

        return reminderDbo is null
            ? new GetReminderByIdRepositoryResult(false, null) { IsReminderFound = false }
            : new GetReminderByIdRepositoryResult(true, reminderDbo);
    }

    public async Task<AddReminderRepositoryResult> AddAsync(ReminderDbo reminderDbo, CancellationToken cancellationToken)
    {
        var createdReminderDbo = (await _dataContext.Reminders.AddAsync(reminderDbo, cancellationToken)).Entity;
        await _dataContext.SaveChangesAsync(cancellationToken);
        return new AddReminderRepositoryResult(true, createdReminderDbo.Id);
    }

    public async Task<SetRemindersCompletedRepositoryResult> SetRemindersCompleted(ReminderIdDto reminderIdDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && reminderDbo.Id == reminderIdDto.ReminderId)
            .SingleOrDefaultAsync(cancellationToken);

        if (reminderDbo is null)
        {
            return new SetRemindersCompletedRepositoryResult(false)
            {
                IsReminderFound = false
            };
        }

        if (reminderDbo.IsCompleted)
        {
            return new SetRemindersCompletedRepositoryResult(false)
            {
                IsCompletedAlreadySet = true
            };
        }

        reminderDbo.IsCompleted = true;
        await _dataContext.SaveChangesAsync(cancellationToken);
        return new SetRemindersCompletedRepositoryResult(true);
    }

    public async Task<DecrementRemindCountRepositoryResult> DecrementRemindCountAsync(long reminderId, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderId && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        if (reminderDbo is null)
        {
            return new DecrementRemindCountRepositoryResult(false, null)
            {
                IsReminderFound = false
            };
        }

        reminderDbo.RemindCount--;
        await _dataContext.SaveChangesAsync(cancellationToken);

        return new DecrementRemindCountRepositoryResult(true, reminderDbo.RemindCount);
    }

    public async Task<SetReminderDateTimeRepositoryResult> SetReminderDateTimeAsync(SetReminderDateTimeDto setReminderDateTimeDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == setReminderDateTimeDto.ReminderId && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        if (reminderDbo is null)
            return new SetReminderDateTimeRepositoryResult(false) { IsReminderFound = false };

        reminderDbo.RemindDateTime = setReminderDateTimeDto.DateTime;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return new SetReminderDateTimeRepositoryResult(true);
    }
}