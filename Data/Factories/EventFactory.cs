using Maestro.Data.Models;

namespace Maestro.Data.Factories;

public class EventFactory(IGuidFactory guidFactory) : IEventFactory
{
    public Event Create(long userId, string description, DateTime reminderTime)
    {
        return new Event(guidFactory.Create(), userId, description, reminderTime);
    }
}