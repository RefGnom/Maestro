using Maestro.Server.Public.Models.Schedules;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public interface IScheduleBuilder
{
    IScheduleBuilder CreateSchedule(long userId);
    IScheduleBuilder WithDescription(long userId, string description);
    IScheduleBuilder WithScheduleDateTime(long userId, DateTime dateTime);
    IScheduleBuilder WithScheduleDuration(long userId, TimeSpan duration);
    IScheduleBuilder WithCanOverlap(long userId, bool canOverlap);
    ScheduleDto Build(long userId);
}