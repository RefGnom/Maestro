using Maestro.Server.Public.Models.Schedules;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class ScheduleBuilder : IScheduleBuilder
{
    private readonly Dictionary<long, ScheduleDto> _schedules = new();

    public IScheduleBuilder CreateSchedule(long userId)
    {
        _schedules[userId] = new ScheduleDto();
        return this;
    }

    public IScheduleBuilder WithDescription(long userId, string description)
    {
        if (!_schedules.TryGetValue(userId, out var schedule))
        {
            throw NotCreatedModelException(userId);
        }

        schedule.Description = description;
        return this;
    }

    public IScheduleBuilder WithScheduleDateTime(long userId, DateTime dateTime)
    {
        if (!_schedules.TryGetValue(userId, out var schedule))
        {
            throw NotCreatedModelException(userId);
        }

        schedule.StartDateTime = dateTime;
        return this;
    }

    public IScheduleBuilder WithScheduleDuration(long userId, TimeSpan duration)
    {
        if (!_schedules.TryGetValue(userId, out var schedule))
        {
            throw NotCreatedModelException(userId);
        }

        schedule.Duration = duration;
        return this;
    }

    public IScheduleBuilder WithCanOverlap(long userId, bool canOverlap)
    {
        if (!_schedules.TryGetValue(userId, out var schedule))
        {
            throw NotCreatedModelException(userId);
        }

        schedule.CanOverlap = canOverlap;
        return this;
    }

    public ScheduleDto Build(long userId)
    {
        if (!_schedules.TryGetValue(userId, out var schedule))
        {
            throw NotCreatedModelException(userId);
        }

        return schedule;
    }

    private static ArgumentException NotCreatedModelException(long userId)
    {
        return new ArgumentException($"Для юзера {userId} не создана модель расписания");
    }
}