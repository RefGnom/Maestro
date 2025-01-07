using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Reminders;

public class ReminderByIdRepositoryResult(bool isSuccessful, ReminderDbo? data) : BaseRepositoryResultWithData<ReminderDbo?>(isSuccessful, data)
{
    public bool? IsReminderFound { get; init; }
}