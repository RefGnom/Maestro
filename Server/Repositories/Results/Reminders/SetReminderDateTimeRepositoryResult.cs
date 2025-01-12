namespace Maestro.Server.Repositories.Results.Reminders;

public class SetReminderDateTimeRepositoryResult(bool isSuccessful) : BaseRepositoryResult(isSuccessful)
{
    public bool? IsReminderFound { get; init; }
}