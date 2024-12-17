using Maestro.Core.Models;

namespace Maestro.Client;

public class EventsApiClient : IEventsApiClient
{
    public Task CreateAsync(ReminderDto reminder)
    {
        throw new NotImplementedException();
    }

    public Task<ReminderDto?> FindAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ReminderDto[]> SelectEventsForUserAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<ReminderDto[]> SelectEventsForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate)
    {
        throw new NotImplementedException();
    }

    public Task MarkEventsAsCompletedAsync(params long[] eventIds)
    {
        throw new NotImplementedException();
    }
}