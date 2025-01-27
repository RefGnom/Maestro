using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Private.Models;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Server.Repositories.Results.Reminders;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class RemindersRepository(DataContext dataContext) : IRemindersRepository
{
    private readonly DataContext _dataContext = dataContext;

    #region Get

    public async Task<GetAllRemindersRepositoryResult> GetAllRemindersAsync(AllRemindersDto allRemindersDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && reminderDbo.RemindDateTime > allRemindersDto.ExclusiveStartDateTime)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(allRemindersDto.Offset)
            .Take(allRemindersDto.Limit)
            .ToListAsync(cancellationToken);

        return new GetAllRemindersRepositoryResult(true, reminderDbos);
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

    public async Task<GetReminderByIdRepositoryResult> GetByIdAsync(ReminderIdDto reminderIdDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderIdDto.ReminderId && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        return reminderDbo is null
            ? new GetReminderByIdRepositoryResult(false, null) { IsReminderFound = false }
            : new GetReminderByIdRepositoryResult(true, reminderDbo);
    }

    public async Task<GetCompletedRemindersRepositoryResult> GetCompletedRemindersAsync(CompletedRemindersDto completedRemindersDto,
        CancellationToken cancellationToken)
    {
        var reminderDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IsCompleted)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(completedRemindersDto.Offset)
            .Take(completedRemindersDto.Limit)
            .ToListAsync(cancellationToken);

        return new GetCompletedRemindersRepositoryResult(true, reminderDbos);
    }

    public async Task<GetOldRemindersRepositoryResult> GetOldRemindersAsync(OldRemindersDto oldRemindersDto, CancellationToken cancellationToken)
    {
        var reminderDbos = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.RemindDateTime <= oldRemindersDto.InclusiveBeforeDateTime)
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(oldRemindersDto.Offset)
            .Take(oldRemindersDto.Limit)
            .ToListAsync(cancellationToken);

        return new GetOldRemindersRepositoryResult(true, reminderDbos);
    }

    #endregion

    #region Post

    public async Task<AddReminderRepositoryResult> AddAsync(ReminderDbo newReminderDbo, CancellationToken cancellationToken)
    {
        var createdReminderDbo = (await _dataContext.Reminders.AddAsync(newReminderDbo, cancellationToken)).Entity;
        await _dataContext.SaveChangesAsync(cancellationToken);
        return new AddReminderRepositoryResult(true, createdReminderDbo.Id);
    }

    #endregion

    #region Patch

    public async Task<SetReminderCompletedRepositoryResult> SetReminderCompleted(ReminderIdDto reminderIdDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.IntegratorId == integratorId && reminderDbo.Id == reminderIdDto.ReminderId)
            .SingleOrDefaultAsync(cancellationToken);

        if (reminderDbo is null)
        {
            return new SetReminderCompletedRepositoryResult(false)
            {
                IsReminderFound = false
            };
        }

        if (reminderDbo.IsCompleted)
        {
            return new SetReminderCompletedRepositoryResult(false)
            {
                IsCompletedAlreadySet = true
            };
        }

        reminderDbo.IsCompleted = true;
        await _dataContext.SaveChangesAsync(cancellationToken);

        return new SetReminderCompletedRepositoryResult(true);
    }

    public async Task<DecrementRemindCountRepositoryResult> DecrementRemindCountAsync(ReminderIdDto reminderIdDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderIdDto.ReminderId && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        if (reminderDbo is null)
        {
            return new DecrementRemindCountRepositoryResult(false, null)
            {
                IsReminderFound = false
            };
        }

        if (reminderDbo.RemindCount is 0)
        {
            return new DecrementRemindCountRepositoryResult(false, null)
            {
                IsRemindCountEqualZero = true
            };
        }

        reminderDbo.RemindCount--;
        await _dataContext.SaveChangesAsync(cancellationToken);

        return new DecrementRemindCountRepositoryResult(true, reminderDbo.RemindCount);
    }

    public async Task<SetReminderDateTimeRepositoryResult> SetReminderDateTimeAsync(ReminderDateTimeDto reminderDateTimeDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbo = await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderDateTimeDto.ReminderId && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        if (reminderDbo is null)
            return new SetReminderDateTimeRepositoryResult(false) { IsReminderFound = false };

        reminderDbo.RemindDateTime = reminderDateTimeDto.DateTime;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return new SetReminderDateTimeRepositoryResult(true);
    }

    #endregion

    #region Delete

    public async Task<DeleteReminderByIdRepositoryResult> DeleteReminderByIdAsync(ReminderIdDto reminderIdDto, CancellationToken cancellationToken)
    {
        await _dataContext.Reminders
            .Where(reminderDbo => reminderDbo.Id == reminderIdDto.ReminderId)
            .ExecuteDeleteAsync(cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return new DeleteReminderByIdRepositoryResult(true);
    }

    #endregion
}