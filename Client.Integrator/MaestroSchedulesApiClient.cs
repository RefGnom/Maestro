using System.Net;
using System.Net.Http.Json;
using Maestro.Server.Public.Models.Schedules;

namespace Maestro.Client.Integrator;

public partial class MaestroApiClient
{
    #region Get

    public async Task<ScheduleDto?> GetScheduleByIdAsync(long scheduleId)
    {
        const string requestEndpoint = "schedules/byId";

        var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
        {
            Content = JsonContent.Create(new ScheduleIdDto
            {
                ScheduleId = scheduleId
            })
        };

        _log.Info($"Sending request {request.Method} {requestEndpoint}. ScheduleId: {scheduleId}");

        var response = await _httpClient.SendAsync(request);

        _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}");

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var schedule = await response.Content.ReadFromJsonAsync<ScheduleDto>();

        return schedule;
    }

    public async IAsyncEnumerable<ScheduleWithIdDto> GetAllSchedulesAsync(DateTime exclusiveStartDateTime)
    {
        const string requestEndpoint = "schedules/all";

        var offset = 0;
        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
            {
                Content = JsonContent.Create(new AllSchedulesDto
                {
                    Offset = offset,
                    Limit = AllSchedulesDto.LimitMaxValue,
                    ExclusiveStartDateTime = exclusiveStartDateTime
                })
            };

            _log.Info($"Sending request {request.Method} {requestEndpoint}. Offset: {offset}. Limit: {AllSchedulesDto.LimitMaxValue}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var schedules = await response.Content.ReadFromJsonAsync<List<ScheduleWithIdDto>>();

            _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}. ItemsCount: {schedules!.Count}");

            foreach (var schedule in schedules)
            {
                yield return schedule;
            }

            if (schedules.Count < AllSchedulesDto.LimitMaxValue)
            {
                yield break;
            }

            offset += AllSchedulesDto.LimitMaxValue;
        } while (true);
    }

    public async IAsyncEnumerable<ScheduleWithIdDto> GetSchedulesForUserAsync(long userId, DateTime? exclusiveStartDateTime = null)
    {
        const string requestEndpoint = "schedules/forUser";

        var offset = 0;
        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestEndpoint)
            {
                Content = JsonContent.Create(new SchedulesForUserDto
                {
                    UserId = userId,
                    Offset = offset,
                    Limit = SchedulesForUserDto.LimitMaxValue,
                    ExclusiveStartDateTime = exclusiveStartDateTime
                })
            };

            _log.Info($"Sending request {request.Method} {requestEndpoint}. Offset: {offset}. Limit: {SchedulesForUserDto.LimitMaxValue}");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var schedules = await response.Content.ReadFromJsonAsync<List<ScheduleWithIdDto>>();

            _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}. ItemsCount: {schedules!.Count}");

            foreach (var schedule in schedules)
            {
                yield return schedule;
            }

            if (schedules.Count < SchedulesForUserDto.LimitMaxValue)
            {
                yield break;
            }

            offset += SchedulesForUserDto.LimitMaxValue;
        } while (true);
    }

    #endregion

    #region Post

    public async Task<long?> CreateScheduleAsync(ScheduleDto schedule)
    {
        const string requestEndpoint = "schedules/create";

        var request = new HttpRequestMessage(HttpMethod.Post, requestEndpoint)
        {
            Content = JsonContent.Create(schedule)
        };

        _log.Info($"Sending request {request.Method} {requestEndpoint}");

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode is HttpStatusCode.Conflict)
        {
            _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}");
            _log.Info("Schedule not created due overlap");
            return null;
        }

        response.EnsureSuccessStatusCode();

        _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}");

        var createdScheduleId = (await response.Content.ReadFromJsonAsync<ScheduleIdDto>())!.ScheduleId;

        _log.Info($"Created ScheduleId: {createdScheduleId}");

        return createdScheduleId;
    }

    #endregion

    #region Patch

    public async Task SetScheduleCompletedAsync(long scheduleId)
    {
        const string requestEndpoint = "schedules/setCompleted";

        var request = new HttpRequestMessage(HttpMethod.Patch, requestEndpoint)
        {
            Content = JsonContent.Create(new ScheduleIdDto
            {
                ScheduleId = scheduleId
            })
        };

        _log.Info($"Sending request {request.Method} {requestEndpoint}. ScheduleId: {scheduleId}");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        _log.Info($"Received response {request.Method} {requestEndpoint}. StatusCode: {response.StatusCode}");
    }

    #endregion
}