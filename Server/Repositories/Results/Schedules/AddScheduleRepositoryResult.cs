namespace Maestro.Server.Repositories.Results.Schedules;

public class AddScheduleRepositoryResult(bool isSuccessful, long? scheduleId) : BaseRepositoryResultWithData<long?>(isSuccessful, scheduleId)
{
    public bool? IsScheduleOverlap { get; init; }
    
    public List<long>? OverlapScheduleIds { get; init; }
}