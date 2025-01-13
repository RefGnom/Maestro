using Maestro.Data.Models;
using Maestro.Server.Public.Models.Schedules;
using Maestro.Server.Repositories.Results.Schedules;

namespace Maestro.Server.Repositories;

public interface ISchedulesRepository
{
    #region Get

    Task<GetAllSchedulesRepositoryResult> GetAllSchedulesAsync(AllSchedulesDto allSchedulesDto, long integratorId,
        CancellationToken cancellationToken);
    Task<GetScheduleByIdRepositoryResult> GetByIdAsync(ScheduleIdDto scheduleIdDto, long integratorId, CancellationToken cancellationToken);
    Task<GetSchedulesForUserRepositoryResult> GetForUserAsync(SchedulesForUserDto schedulesForUserDto, long integratorId,
        CancellationToken cancellationToken);

    #endregion

    #region Post

    Task<AddScheduleRepositoryResult> AddAsync(ScheduleDbo newScheduleDbo, CancellationToken cancellationToken);

    #endregion

    #region Patch

    
    Task<SetScheduleCompletedRepositoryResult> SetScheduleCompleted(ScheduleIdDto scheduleIdDto, long integratorId,
        CancellationToken cancellationToken);
    
    #endregion
}