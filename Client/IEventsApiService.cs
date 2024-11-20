using Data.Models;

namespace Client;

public interface IEventsApiService
{
    public void CreateEvent(Event eventToCreate);
}