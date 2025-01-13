namespace Maestro.Server.Repositories.Results.Reminders;

public class DecrementRemindCountRepositoryResult(bool isSuccessful, int? remainRemindCount)
    : BaseRepositoryResultWithData<int?>(isSuccessful, remainRemindCount)
{
    public bool? IsReminderFound { get; init; }

    public bool? IsRemindCountEqualZero { get; init; }
}