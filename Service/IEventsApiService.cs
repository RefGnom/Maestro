using Data.Models;

namespace Service;

public interface IEventsApiService
{
    public Task CreateEventAsync(Event eventToCreate);
}