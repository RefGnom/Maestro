namespace Maestro.Server.Repositories.Results.Reminders;

public class DecrementRemindCountRepositoryResult(bool isSuccessful, int? data) : BaseRepositoryResultWithData<int?>(isSuccessful, data)
{
    public bool? IsReminderFound { get; init; }
}