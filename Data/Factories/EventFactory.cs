using Data.Models;

namespace Data.Factories;

public class EventFactory : IEventFactory
{
    public Event Create(long userId, string description, DateTime reminderTime)
    {
        return new Event(Guid.NewGuid(), userId, description, reminderTime);
    }
}