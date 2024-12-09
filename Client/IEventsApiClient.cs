using Maestro.Data.Models;

namespace Maestro.Client;

public interface IEventsApiClient
{
    public Task CreateAsync(Event @event);
}