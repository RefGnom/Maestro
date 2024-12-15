using Maestro.Core.Factories;
using Maestro.Core.Models;

namespace Maestro.TelegramIntegrator.Factories;

public class EventFactory(IGuidFactory guidFactory) : IEventFactory
{
    private readonly IGuidFactory _guidFactory = guidFactory;

    public EventDto Create(long userId, string description, DateTime reminderTime, TimeSpan reminderTimeDuration)
    {
        return new EventDto(userId, description, reminderTime, reminderTimeDuration);
    }
}