namespace Maestro.Server.Repositories.Results.Reminders;

public class DeleteReminderByIdRepositoryResult(bool isSuccessful) : BaseRepositoryResult(isSuccessful)
{
    public bool? IsReminderFound { get; set; }
}