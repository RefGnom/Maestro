using Maestro.Core.Models;

namespace Maestro.Client;

public class EventsApiClient : IEventsApiClient
{
    public Task CreateAsync(EventDto @event)
    {
        throw new NotImplementedException();
    }
    public Task<EventDto?> FindAsync(long id)
    {
        throw new NotImplementedException();
    }
    public Task<EventDto[]> SelectEventsForUserAsync(long userId)
    {
        throw new NotImplementedException();
    }
    public Task<EventDto[]> SelectEventsForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate)
    {
        throw new NotImplementedException();
    }
    public Task DeleteCompletedEvents()
    {
        throw new NotImplementedException();
    }
    public Task DeleteExpiredEvents(DateTime expirationTime)
    {
        throw new NotImplementedException();
    }
    public Task MarkEventsAsCompleted(long[] eventIds)
    {
        throw new NotImplementedException();
    }
}