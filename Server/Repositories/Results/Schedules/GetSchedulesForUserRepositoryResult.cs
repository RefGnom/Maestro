using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Schedules;

public class GetSchedulesForUserRepositoryResult(bool isSuccessful, List<ScheduleDbo> schedules)
    : BaseRepositoryResultWithData<List<ScheduleDbo>>(isSuccessful, schedules)
{
    
}