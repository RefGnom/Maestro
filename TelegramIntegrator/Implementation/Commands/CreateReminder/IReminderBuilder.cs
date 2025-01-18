using Maestro.Server.Public.Models.Reminders;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public interface IReminderBuilder
{
    IReminderBuilder CreateReminder(long userId);
    IReminderBuilder WithDescription(long userId, string description);
    IReminderBuilder WithReminderDateTime(long userId, DateTime dateTime);
    IReminderBuilder WithRemindInterval(long userId, TimeSpan remindInterval);
    IReminderBuilder WithRemindCount(long userId, int remindCount);
    ReminderDto Build(long userId);
}