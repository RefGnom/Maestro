using Maestro.Data.Models;

namespace Maestro.Data.Factories;

public class EventFactory(IGuidFactory guidFactory) : IEventFactory
{
    private readonly IGuidFactory _guidFactory = guidFactory;

    public Event Create(long userId, string description, DateTime reminderTime)
    {
        return new Event(_guidFactory.Create(), userId, description, reminderTime);
    }
}