using System.Net.Http.Json;
using Maestro.Core.Logging;
using Maestro.Core.Models;

namespace Maestro.Client;

public class EventsApiClient : IEventsApiClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILog<EventsApiClient> _log;

    public EventsApiClient(string uri, string apiKey, ILogFactory logFactory)
    {
        if (string.IsNullOrEmpty(uri))
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentNullException(nameof(apiKey));
        }

        _httpClient = new HttpClient { BaseAddress = new Uri(uri), DefaultRequestHeaders = { { "Authorization", apiKey } } };
        _log = logFactory.CreateLog<EventsApiClient>();
    }

    public async Task CreateAsync(ReminderDto reminder)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "reminders/create")
        {
            //            Content = new StringContent(json, Encoding.UTF8, "application/json")
            Content = JsonContent.Create(reminder),
        };

        _log.Info($"Sending request to {request.RequestUri}");

        var response = await _httpClient.SendAsync(request);

        _log.Info($"Received response from {request.RequestUri}");

        response.EnsureSuccessStatusCode();
    }

    public Task<ReminderDto?> FindAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ReminderDto[]> SelectRemindersForUserAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<ReminderDto[]> SelectRemindersForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate)
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
    
    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}