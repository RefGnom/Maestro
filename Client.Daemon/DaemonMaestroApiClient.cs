using System.Net;
using System.Net.Http.Json;
using Maestro.Core.Logging;
using Maestro.Server.Private.Models;
using Maestro.Server.Public.Models.Reminders;

namespace Maestro.Client.Daemon;

public class DaemonMaestroApiClient : IDaemonMaestroApiClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILog<DaemonMaestroApiClient> _log;

    private bool _isDisposed;

    public DaemonMaestroApiClient(string uri, string apiKey, ILogFactory logFactory)
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
        _log = logFactory.CreateLog<DaemonMaestroApiClient>();
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

    public async IAsyncEnumerable<ReminderWithIdDto> GetCompletedRemindersAsync()
    {
        const string requestEndpoint = "completedReminders";

        var offset = 0;
        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
            {
                Content = JsonContent.Create(new CompletedRemindersDto
                {
                    Offset = offset,
                    Limit = CompletedRemindersDto.LimitMaxValue
                })
            };

            _log.Info($"Sending request {request.Method} {requestEndpoint}. Offset: {offset}. Limit: {AllRemindersDto.LimitMaxValue}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var reminders = await response.Content.ReadFromJsonAsync<List<ReminderWithIdDto>>();

            _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}. ItemsCount: {reminders!.Count}");

            foreach (var reminder in reminders)
            {
                yield return reminder;
            }

            if (reminders.Count < CompletedRemindersDto.LimitMaxValue)
            {
                yield break;
            }

            offset += CompletedRemindersDto.LimitMaxValue;
        } while (true);
    }

    public async IAsyncEnumerable<ReminderWithIdDto> GetOldRemindersAsync(DateTime inclusiveBeforeDateTime)
    {
        const string requestEndpoint = "oldReminders";

        var offset = 0;
        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
            {
                Content = JsonContent.Create(new OldRemindersDto
                {
                    Offset = offset,
                    Limit = OldRemindersDto.LimitMaxValue,
                    InclusiveBeforeDateTime = inclusiveBeforeDateTime
                })
            };

            _log.Info($"Sending request {request.Method} {requestEndpoint}. Offset: {offset}. Limit: {OldRemindersDto.LimitMaxValue}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var reminders = await response.Content.ReadFromJsonAsync<List<ReminderWithIdDto>>();

            _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}. ItemsCount: {reminders!.Count}");

            foreach (var reminder in reminders)
            {
                yield return reminder;
            }

            if (reminders.Count < OldRemindersDto.LimitMaxValue)
            {
                yield break;
            }

            offset += OldRemindersDto.LimitMaxValue;
        } while (true);
    }

    public async Task DeleteReminderAsync(long reminderId)
    {
        const string requestEndpoint = "reminder";
        
        var request = new HttpRequestMessage(HttpMethod.Delete, requestEndpoint)
        {
            Content = JsonContent.Create(new ReminderIdDto
            {
                ReminderId = reminderId
            })
        };

        _log.Info($"Sending request {request.Method} {requestEndpoint}.");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
        
        _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}.");
    }
}