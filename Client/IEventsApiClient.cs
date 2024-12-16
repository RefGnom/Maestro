using Maestro.Core.Models;

namespace Maestro.Client;

public interface IEventsApiClient
{
    public Task CreateAsync(NotificationDto notification);
    public Task<NotificationDto?> FindAsync(long id);
    public Task<NotificationDto[]> SelectEventsForUserAsync(long userId);
    public Task<NotificationDto[]> SelectEventsForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate);
    public Task MarkEventsAsCompletedAsync(params long[] eventIds);
    public Task DeleteEventAsync(long eventId);
}