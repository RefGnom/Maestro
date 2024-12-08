namespace Maestro.TelegramIntegrator.Models;

public record Message(string Command, DateTime ReminderTime, string Description);