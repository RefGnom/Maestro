using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Public.Models.Schedules;
using Maestro.Server.Repositories.Results.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class SchedulesRepository(DataContext dataContext) : ISchedulesRepository
{
    private readonly DataContext _dataContext = dataContext;

    #region Get

    public async Task<GetAllSchedulesRepositoryResult> GetAllSchedulesAsync(AllSchedulesDto allSchedulesDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var scheduleDbos = await _dataContext.Schedules
            .Where(scheduleDbo => scheduleDbo.IntegratorId == integratorId && scheduleDbo.StartDateTime > allSchedulesDto.ExclusiveStartDateTime)
            .OrderBy(scheduleDbo => scheduleDbo.Id)
            .Skip(allSchedulesDto.Offset)
            .Take(allSchedulesDto.Limit)
            .ToListAsync(cancellationToken);

        return new GetAllSchedulesRepositoryResult(true, scheduleDbos);
    }

    public async Task<GetScheduleByIdRepositoryResult> GetByIdAsync(ScheduleIdDto scheduleIdDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var scheduleDbo = await _dataContext.Schedules
            .Where(reminderDbo => reminderDbo.Id == scheduleIdDto.ScheduleId && reminderDbo.IntegratorId == integratorId)
            .SingleOrDefaultAsync(cancellationToken);

        return scheduleDbo is null
            ? new GetScheduleByIdRepositoryResult(false, null) { IsScheduleFound = false }
            : new GetScheduleByIdRepositoryResult(true, scheduleDbo);
    }

    public async Task<GetSchedulesForUserRepositoryResult> GetForUserAsync(SchedulesForUserDto schedulesForUserDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var reminderDbosBaseQuery = _dataContext.Schedules
            .Where(reminderDbo => reminderDbo.UserId == schedulesForUserDto.UserId && reminderDbo.IntegratorId == integratorId);

        if (schedulesForUserDto.ExclusiveStartDateTime is not null)
        {
            reminderDbosBaseQuery =
                reminderDbosBaseQuery.Where(reminderDbo => reminderDbo.StartDateTime > schedulesForUserDto.ExclusiveStartDateTime);
        }

        var reminderDbos = await reminderDbosBaseQuery
            .OrderBy(reminderDbo => reminderDbo.Id)
            .Skip(schedulesForUserDto.Offset)
            .Take(schedulesForUserDto.Limit)
            .ToListAsync(cancellationToken);

        return new GetSchedulesForUserRepositoryResult(true, reminderDbos);
    }

    #endregion

    #region Post

    public async Task<AddScheduleRepositoryResult> AddAsync(ScheduleDbo newScheduleDbo, CancellationToken cancellationToken)
    {
        if (!newScheduleDbo.CanOverlap)
        {
            var endDateTime = newScheduleDbo.StartDateTime.Add(newScheduleDbo.Duration);

            var overlappedSchedules = await _dataContext.Schedules
                .Where(scheduleDbo =>
                    scheduleDbo.StartDateTime < newScheduleDbo.StartDateTime &&
                    newScheduleDbo.StartDateTime < scheduleDbo.StartDateTime + scheduleDbo.Duration ||
                    scheduleDbo.StartDateTime < endDateTime && endDateTime < scheduleDbo.StartDateTime + scheduleDbo.Duration &&
                    scheduleDbo.IntegratorId == newScheduleDbo.IntegratorId && 
                    scheduleDbo.UserId == newScheduleDbo.UserId)
                .ToListAsync(cancellationToken);

            if (overlappedSchedules.Count is not 0)
            {
                return new AddScheduleRepositoryResult(false, null)
                {
                    IsScheduleOverlap = true,
                    OverlapScheduleIds = overlappedSchedules.Select(overlappedSchedule => overlappedSchedule.Id).ToList()
                };
            }
        }

        var createdScheduleDbo = (await _dataContext.Schedules.AddAsync(newScheduleDbo, cancellationToken)).Entity;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return new AddScheduleRepositoryResult(true, createdScheduleDbo.Id);
    }

    #endregion

    #region Patch

    public async Task<SetScheduleCompletedRepositoryResult> SetScheduleCompleted(ScheduleIdDto scheduleIdDto, long integratorId,
        CancellationToken cancellationToken)
    {
        var scheduleDbo = await _dataContext.Schedules
            .Where(scheduleDbo => scheduleDbo.IntegratorId == integratorId && scheduleDbo.Id == scheduleIdDto.ScheduleId)
            .SingleOrDefaultAsync(cancellationToken);

        if (scheduleDbo is null)
        {
            return new SetScheduleCompletedRepositoryResult(false)
            {
                IsScheduleFound = false
            };
        }

        if (scheduleDbo.IsCompleted)
        {
            return new SetScheduleCompletedRepositoryResult(false)
            {
                IsCompletedAlreadySet = true
            };
        }

        scheduleDbo.IsCompleted = true;
        await _dataContext.SaveChangesAsync(cancellationToken);

        return new SetScheduleCompletedRepositoryResult(true);
    }

    #endregion
}