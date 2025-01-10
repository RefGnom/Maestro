using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Reminders;

public class GetReminderByIdRepositoryResult(bool isSuccessful, ReminderDbo? reminder)
    : BaseRepositoryResultWithData<ReminderDbo?>(isSuccessful, reminder)
{
    public bool? IsReminderFound { get; init; }
}