namespace Maestro.Server.Repositories.Results.Reminders;

public class SetReminderCompletedRepositoryResult(bool isSuccessful) : BaseRepositoryResult(isSuccessful)
{
    public bool? IsReminderFound { get; init; }
    public bool? IsCompletedAlreadySet { get; init; }
}