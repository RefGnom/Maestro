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

            var remindersDto = await response.Content.ReadFromJsonAsync<List<ReminderDtoWithId>>();

            _log.Info($"Received response from {requestEndpoint}. StatusCode: {response.StatusCode}. ItemsCount: {remindersDto!.Count}");

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