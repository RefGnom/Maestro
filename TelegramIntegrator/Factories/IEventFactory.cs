using Maestro.Core.Models;

namespace Maestro.TelegramIntegrator.Factories;

public interface IEventFactory
{
    EventDto Create(long userId, string description, DateTime reminderTime, TimeSpan reminderTimeDuration);
}