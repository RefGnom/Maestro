using Maestro.Data.Models;

namespace Maestro.Core;

public interface IEventsApiService
{
    public Task CreateAsync(Event eventToCreate);
}