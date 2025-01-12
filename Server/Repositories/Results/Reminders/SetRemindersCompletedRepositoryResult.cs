namespace Maestro.Server.Repositories.Results.Reminders;

public class SetRemindersCompletedRepositoryResult(bool isSuccessful) : BaseRepositoryResult(isSuccessful)
{
    public bool? IsReminderFound { get; init; }
    public bool? IsCompletedAlreadySet { get; init; }
}