namespace Data.Models;

public record Event(Guid Id, long UserId, string Description, DateTime ReminderTime);