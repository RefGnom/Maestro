using Maestro.Core.Models;

namespace Maestro.Client;

public class EventsApiClient : IEventsApiClient
{
    public Task CreateAsync(NotificationDto notification)
    {
        throw new NotImplementedException();
    }
    public Task<NotificationDto?> FindAsync(long id)
    {
        throw new NotImplementedException();
    }
    public Task<NotificationDto[]> SelectEventsForUserAsync(long userId)
    {
        throw new NotImplementedException();
    }
    public Task<NotificationDto[]> SelectEventsForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate)
    {
        throw new NotImplementedException();
    }
    public Task MarkEventsAsCompletedAsync(params long[] eventIds)
    {
        throw new NotImplementedException();
    }
    public Task DeleteEventAsync(long eventId)
    {
        throw new NotImplementedException();
    }
}