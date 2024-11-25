using Data.Models;

namespace Client;

public interface IEventsApiService
{
    public Task CreateEventAsync(Event eventToCreate);
}