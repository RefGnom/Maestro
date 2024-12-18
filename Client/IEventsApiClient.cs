using Maestro.Core.Models;

namespace Maestro.Client;

public interface IEventsApiClient
{
    public Task CreateAsync(ReminderDto reminder);
    public Task<ReminderDto?> FindAsync(long id);
    public Task<ReminderDto[]> SelectEventsForUserAsync(long userId);
    public Task<ReminderDto[]> SelectEventsForUserAsync(long userId, DateTime inclusiveStartDate, DateTime exclusiveEndDate);

    /// <summary>
    /// Метод возвращает пачку событий размером 100 в переданном временном промежутке
    /// </summary>
    /// <param name="inclusiveStartDate">Дата с которой начинаем брать события</param>
    /// <param name="exclusiveEndDate">Дата до которой берём события</param>
    /// <returns></returns>
    public Task<ReminderDto[]> SelectEvents(DateTime inclusiveStartDate, DateTime exclusiveEndDate);

    public Task MarkEventsAsCompletedAsync(params long[] eventIds);
}