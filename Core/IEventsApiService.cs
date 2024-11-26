using Data.Models;

namespace Core;

public interface IEventsApiService
{
    public Task CreateAsync(Event eventToCreate);
}