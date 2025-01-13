using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Schedules;

public class GetAllSchedulesRepositoryResult(bool isSuccessful, List<ScheduleDbo> schedules)
    : BaseRepositoryResultWithData<List<ScheduleDbo>>(isSuccessful, schedules)
{
}