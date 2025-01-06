using System.Net;
using System.Net.Http.Json;
using Maestro.Core.Logging;
using Maestro.Server.Core.Models;

namespace Maestro.Client;

public class MaestroApiClient : IMaestroApiClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILog<MaestroApiClient> _log;

    private bool _isDisposed;

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
        if (_isDisposed)
        {
            return;
        }

        _httpClient.Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
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

    public async IAsyncEnumerable<ReminderWithIdDto> GetAllRemindersAsync(DateTime exclusiveStartDateTime)
    {
        const string requestEndpoint = "reminders/all";

        var offset = 0;
        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
            {
                Content = JsonContent.Create(new AllRemindersDto
                {
                    Offset = offset,
                    Limit = RemindersForUserDto.LimitMaxValue,
                    ExclusiveStartDateTime = exclusiveStartDateTime
                })
            };

            _log.Info($"Sending request to {requestEndpoint}. Offset: {offset}. Limit: {AllRemindersDto.LimitMaxValue}");

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

    public async IAsyncEnumerable<ReminderWithIdDto> GetRemindersForUserAsync(long userId, DateTime? exclusiveStartDateTime = null)
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

            _log.Info($"Sending request to {requestEndpoint}. Offset: {offset}. Limit: {RemindersForUserDto.LimitMaxValue}");

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

        _log.Info($"Created ReminderId: {createdReminderId}");

        return createdReminderId;
    }

    public async Task MarkRemindersAsCompletedAsync(params long[] remindersIds)
    {
        const string requestEndpoint = "reminders/markAsCompleted";

        var iteration = 0;
        var remindersIdsChunks = remindersIds.Chunk(RemindersIdsDto.LimitMaxValue);
        foreach (var remindersIdsChunk in remindersIdsChunks)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, requestEndpoint)
            {
                Content = JsonContent.Create(new RemindersIdsDto
                {
                    RemindersIds = remindersIdsChunk
                })
            };

            _log.Info(
                $"Sending request to {requestEndpoint}. Offset: {iteration * RemindersIdsDto.LimitMaxValue}. ItemsCount: {remindersIds.Length}");

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode is HttpStatusCode.NotFound)
            {
                var notFoundRemindersIds = await response.Content.ReadFromJsonAsync<RemindersIdsDto>();
                throw new ArgumentException($"Reminder(s) with Id(s) was not found: [{string.Join(',', notFoundRemindersIds!.RemindersIds)}]",
                    nameof(remindersIds));
            }

            response.EnsureSuccessStatusCode();

            _log.Info($"Received response from {requestEndpoint}. StatusCode: {response.StatusCode}");

            iteration++;
        }
    }

    #endregion
}