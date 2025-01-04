using System.Net;
using System.Net.Http.Json;
using Maestro.Core.Logging;
using Maestro.Server.Core.Models;

namespace Maestro.Client;

public class MaestroApiClient : IMaestroApiClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILog<MaestroApiClient> _log;

    public MaestroApiClient(string uri, string apiKey, ILogFactory logFactory)
    {
        if (string.IsNullOrEmpty(uri))
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentNullException(nameof(apiKey));
        }

        ArgumentNullException.ThrowIfNull(logFactory, nameof(logFactory));

        _httpClient = new HttpClient { BaseAddress = new Uri(uri), DefaultRequestHeaders = { { "Authorization", apiKey } } };
        _log = logFactory.CreateLog<MaestroApiClient>();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }

    # region Get

    public async Task<ReminderDto?> GetReminderAsync(long reminderId)
    {
        const string requestEndpoint = "reminders/byId";

        var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
        {
            Content = JsonContent.Create(new ReminderIdDto
            {
                Id = reminderId
            })
        };

        _log.Info($"Sending request to {requestEndpoint}. Reminder Id: {reminderId}");

        var response = await _httpClient.SendAsync(request);

        _log.Info($"Received response from {requestEndpoint}. StatusCode: {response.StatusCode}");

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var reminder = await response.Content.ReadFromJsonAsync<ReminderDto>();

        return reminder;
    }

    public async IAsyncEnumerable<ReminderWithIdDto> GetRemindersForUserAsync(long userId)
    {
        const string requestEndpoint = "reminders/forUser";

        var offset = 0;

        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
            {
                Content = JsonContent.Create(new RemindersForUserDto
                {
                    UserId = userId,
                    Offset = offset,
                    Limit = RemindersForUserDto.LimitMaxValue
                })
            };

            _log.Info($"Sending request to {requestEndpoint}. Offset: {offset}, Limit: {RemindersForUserDto.LimitMaxValue}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var reminders = await response.Content.ReadFromJsonAsync<List<ReminderWithIdDto>>();

            _log.Info($"Received response from {requestEndpoint}. StatusCode: {response.StatusCode}. ItemsCount: {reminders!.Count}");

            foreach (var reminder in reminders)
            {
                yield return reminder;
            }

            if (reminders.Count < RemindersForUserDto.LimitMaxValue)
            {
                yield break;
            }

            offset += RemindersForUserDto.LimitMaxValue;
        } while (true);
    }

    public async IAsyncEnumerable<ReminderWithIdDto> GetRemindersForUserAsync(long userId, DateTime exclusiveStartDateTime)
    {
        const string requestEndpoint = "reminders/forUser";

        var offset = 0;

        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
            {
                Content = JsonContent.Create(new RemindersForUserDto
                {
                    UserId = userId,
                    Offset = offset,
                    Limit = RemindersForUserDto.LimitMaxValue,
                    ExclusiveStartDateTime = exclusiveStartDateTime
                })
            };

            _log.Info($"Sending request to {requestEndpoint}. Offset: {offset}, Limit: {RemindersForUserDto.LimitMaxValue}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var reminders = await response.Content.ReadFromJsonAsync<List<ReminderWithIdDto>>();

            _log.Info($"Received response from {requestEndpoint}. StatusCode: {response.StatusCode}. ItemsCount: {reminders!.Count}");

            foreach (var reminder in reminders)
            {
                yield return reminder;
            }

            if (reminders.Count < RemindersForUserDto.LimitMaxValue)
            {
                yield break;
            }

            offset += RemindersForUserDto.LimitMaxValue;
        } while (true);
    }

    #endregion

    #region Post

    public async Task<long> CreateReminderAsync(ReminderDto reminder)
    {
        const string requestEndpoint = "reminders/create";

        var request = new HttpRequestMessage(HttpMethod.Post, requestEndpoint)
        {
            Content = JsonContent.Create(reminder)
        };

        _log.Info($"Sending request to {requestEndpoint}");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        _log.Info($"Received response from {requestEndpoint}. StatusCode: {response.StatusCode}");

        var createdReminderId = (await response.Content.ReadFromJsonAsync<ReminderIdDto>())!.Id;

        _log.Info($"Created reminder Id: {createdReminderId}");

        return createdReminderId;
    }

    public async Task MarkRemindersAsCompletedAsync(params long[] remindersId)
    {
        const string requestEndpoint = "reminders/markAsCompleted";
        const int chunkSize = 50;

        for (int i = 0; i < remindersId.Length; i += chunkSize)
        {
            var chunk = remindersId.Skip(i).Take(chunkSize).ToList();
            var request = new HttpRequestMessage(HttpMethod.Post, requestEndpoint)
            {
                Content = JsonContent.Create(chunk)
            };

            _log.Info($"Sending request to {requestEndpoint}. Chunk size: {chunk.Count}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            _log.Info($"Received response from {requestEndpoint}. StatusCode: {response.StatusCode}");
        }
    }

    #endregion
}