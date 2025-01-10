using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Reminders;

public class AllRemindersRepositoryResult(bool isSuccessful, List<ReminderDbo> reminders)
    : BaseRepositoryResultWithData<List<ReminderDbo>>(isSuccessful, reminders)
{
}