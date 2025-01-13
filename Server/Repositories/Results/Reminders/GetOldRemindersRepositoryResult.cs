using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Reminders;

public class GetOldRemindersRepositoryResult(bool isSuccessful, List<ReminderDbo> reminders)
    : BaseRepositoryResultWithData<List<ReminderDbo>>(isSuccessful, reminders)
{
    
}