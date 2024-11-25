using Data.Models;

namespace Data.Factories;

public interface IEventFactory
{
    Event Create(long userId, string description, DateTime reminderTime);
}