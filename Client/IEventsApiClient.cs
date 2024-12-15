using Maestro.Core.Models;

namespace Maestro.Client;

public interface IEventsApiClient
{
    public Task CreateAsync(EventDto @event);
    public Task<EventDto?> FindAsync(long id);
    public Task<EventDto[]> SelectEventsForUserAsync(long userId);
    public Task<EventDto[]> SelectEventsForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate);
    public Task DeleteCompletedEvents();
    public Task DeleteExpiredEvents(DateTime expirationTime);
    public Task MarkEventsAsCompleted(long[] eventIds);
}