using System.Net.Http.Json;
using Maestro.Core.Logging;
using Maestro.Server.Core.ApiModels;

namespace Maestro.Client;

public class ApiClient : IApiClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILog<ApiClient> _log;

    public ApiClient(string uri, string apiKey, ILogFactory logFactory)
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
        _log = logFactory.CreateLog<ApiClient>();
    }

    # region Get

    public Task<ReminderDto?> GetReminderAsync(long reminderId)
    {
        throw new NotImplementedException();
    }

    public async IAsyncEnumerable<ReminderDtoWithId> GetRemindersForUserAsync(long userId)
    {
        var offset = 0;
        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "reminders/forUser")
            {
                Content = JsonContent.Create(new RemindersForUserDto
                {
                    UserId = userId,
                    Offset = offset,
                    Limit = RemindersForUserDto.LimitMaxValue
                })
            };

            _log.Info($"Sending request to {request.RequestUri}. Offset: {offset}, Limit: {RemindersForUserDto.LimitMaxValue}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var remindersDto = await response.Content.ReadFromJsonAsync<List<ReminderDtoWithId>>();

            _log.Info($"Received response from {request.RequestUri}. StatusCode: {response.StatusCode}. ItemsCount: {remindersDto!.Count}");

            foreach (var reminderDto in remindersDto)
            {
                yield return reminderDto;
            }

            if (remindersDto.Count < RemindersForUserDto.LimitMaxValue)
            {
                yield break;
            }

            offset += RemindersForUserDto.LimitMaxValue;
        } while (true);
    }

    public Task<ReminderDto[]> GetRemindersForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Post

    public async Task<long> CreateReminderAsync(ReminderDto reminder)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "reminders/create")
        {
            Content = JsonContent.Create(reminder)
        };

        _log.Info($"Sending request to {request.RequestUri}");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        _log.Info($"Received response from {request.RequestUri}. StatusCode: {response.StatusCode}");

        var createdReminderId = (await response.Content.ReadFromJsonAsync<ReminderIdDto>())!.Id;

        _log.Info($"Created reminder Id: {createdReminderId}");

        return createdReminderId;
    }

    public Task MarkRemindersAsCompletedAsync(params long[] remindersId)
    {
        throw new NotImplementedException();
    }

    #endregion

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}