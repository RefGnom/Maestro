namespace Maestro.Server.Repositories.Results.Reminders;

public class AddReminderRepositoryResult(bool isSuccessful, long reminderId) : BaseRepositoryResultWithData<long>(isSuccessful, reminderId)
{
    
}