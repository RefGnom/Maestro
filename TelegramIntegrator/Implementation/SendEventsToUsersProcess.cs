using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Operational.ProcessesCore;

namespace Maestro.TelegramIntegrator.Implementation;

public class SendEventsToUsersProcess(
    ILog<SendEventsToUsersProcess> log,
    IEventsApiClient eventsApiClient,
    IDateTimeProvider dateTimeProvider,
    ITelegramBotWrapper telegramBotWrapper
) : RegularProcessBase<SendEventsToUsersProcess>(log)
{
    private readonly IEventsApiClient _eventsApiClient = eventsApiClient;
    public override string ProcessName => "Чтение пользовательских событий";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Timeout => TimeSpan.FromSeconds(5);

    protected async override Task TryRunAsync()
    {
        var currentDate = dateTimeProvider.GetCurrentDateTime();
        var inclusiveStartDate = currentDate.AddMinutes(-2); // почему 2? Хз. Нужно умно определять это число
        var exclusiveEndDate = currentDate.AddMinutes(2);

        var events = await _eventsApiClient.SelectEvents(inclusiveStartDate, exclusiveEndDate);
        var eventsToSend = events
            .Where(x => !x.IsCompleted)
            .Where(x => x.ReminderTime < currentDate)
            .ToArray();

        foreach (var eventToSend in eventsToSend)
        {
            await telegramBotWrapper.SendMessageAsync(eventToSend.UserId, eventToSend.Description);
        }

        var sentUserEventIds = eventsToSend.Where(x => !x.IsRepeatable)
            .Select(x => x.Id)
            .ToArray();
        await _eventsApiClient.MarkEventsAsCompletedAsync(sentUserEventIds);
    }
}