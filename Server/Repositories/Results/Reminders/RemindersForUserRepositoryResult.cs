using Maestro.Data.Models;

namespace Maestro.Server.Repositories.Results.Reminders;

public class RemindersForUserRepositoryResult(bool isSuccessful, List<ReminderDbo> data) : BaseRepositoryResultWithData<List<ReminderDbo>>(isSuccessful, data)
{
    
}