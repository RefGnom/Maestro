using Data.Models;

namespace Service;

public interface IEventsApiService
{
    public Task CreateAsync(Event eventToCreate);
}