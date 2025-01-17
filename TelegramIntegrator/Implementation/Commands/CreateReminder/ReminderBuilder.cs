using Maestro.Server.Public.Models.Reminders;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class ReminderBuilder : IReminderBuilder
{
    private readonly Dictionary<long, ReminderDto> _reminders = new();

    public IReminderBuilder CreateReminder(long userId)
    {
        _reminders[userId] = new ReminderDto();
        return this;
    }

    public IReminderBuilder WithDescription(long userId, string description)
    {
        if (!_reminders.TryGetValue(userId, out var reminder))
        {
            throw NotCreatedModelException(userId);
        }

        reminder.Description = description;
        return this;

    }

    public IReminderBuilder WithReminderDateTime(long userId, DateTime dateTime)
    {
        if (!_reminders.TryGetValue(userId, out var reminder))
        {
            throw NotCreatedModelException(userId);
        }

        reminder.RemindDateTime = dateTime;
        return this;
    }

    public IReminderBuilder WithRemindInterval(long userId, TimeSpan remindInterval)
    {
        if (!_reminders.TryGetValue(userId, out var reminder))
        {
            throw NotCreatedModelException(userId);
        }

        reminder.RemindInterval = remindInterval;
        return this;
    }

    public IReminderBuilder WithRemindCount(long userId, int remindCount)
    {
        if (!_reminders.TryGetValue(userId, out var reminder))
        {
            throw NotCreatedModelException(userId);
        }

        reminder.RemindCount = remindCount;
        return this;
    }

    public ReminderDto Build(long userId)
    {
        if (!_reminders.TryGetValue(userId, out var reminder))
        {
            throw NotCreatedModelException(userId);
        }

        return reminder;
    }

    private static ArgumentException NotCreatedModelException(long userId)
    {
         return new ArgumentException($"Для юзера {userId} не создана модель напоминания");
    }
}