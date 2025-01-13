namespace Maestro.Server.Repositories.Results.Schedules;

public class SetScheduleCompletedRepositoryResult(bool isSuccessful) : BaseRepositoryResult(isSuccessful)
{
    public bool? IsScheduleFound { get; init; }
    public bool? IsCompletedAlreadySet { get; init; }
}