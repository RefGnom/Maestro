using Maestro.Client.Models;
using Maestro.Core.Factories;

namespace Maestro.TelegramIntegrator.Factories;

public class EventFactory(IGuidFactory guidFactory) : IEventFactory
{
    private readonly IGuidFactory _guidFactory = guidFactory;

    public EventDto Create(long userId, string description, DateTime reminderTime, TimeSpan reminderTimeDuration)
    {
        return new EventDto(_guidFactory.Create(), userId, 1, description, reminderTime, reminderTimeDuration);
    }
}