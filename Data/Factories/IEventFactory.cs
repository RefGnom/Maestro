using Maestro.Data.Models;

namespace Maestro.Data.Factories;

public interface IEventFactory
{
    Event Create(long userId, string description, DateTime reminderTime);
}