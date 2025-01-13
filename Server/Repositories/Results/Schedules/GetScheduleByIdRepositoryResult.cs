using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Schedules;

public class GetScheduleByIdRepositoryResult(bool isSuccessful, ScheduleDbo? schedule)
    : BaseRepositoryResultWithData<ScheduleDbo?>(isSuccessful, schedule)
{
    public bool? IsScheduleFound { get; init; }
}