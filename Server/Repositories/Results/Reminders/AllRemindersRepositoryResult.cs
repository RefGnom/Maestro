using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Reminders;

public class AllRemindersRepositoryResult(bool isSuccessful, List<ReminderDbo> data) : BaseRepositoryResultWithData<List<ReminderDbo>>(isSuccessful, data)
{
}