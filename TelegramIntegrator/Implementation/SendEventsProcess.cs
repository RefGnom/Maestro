using Maestro.Client;
using Maestro.Core.Extensions;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Operational.ProcessesCore;

namespace Maestro.TelegramIntegrator.Implementation;

public class SendEventsProcess(
    ILog<SendEventsProcess> log,
    IEventsApiClient eventsApiClient,
    IDateTimeProvider dateTimeProvider,
    ITelegramBotWrapper telegramBotWrapper
) : RegularProcessBase<SendEventsProcess>(log)
{
    public override string ProcessName => "Чтение пользовательских событий";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Timeout => TimeSpan.FromSeconds(5);
    protected async override Task TryRunAsync()
    {
        var currentDate = dateTimeProvider.GetCurrentDateTime();
        var inclusiveStartDate = currentDate.AddMinutes(-2); // почему 2? Хз. Нужно умно определять это число
        var exclusiveEndDate = currentDate.AddMinutes(2);

        int eventsReadCount;
        do
        {
            var events = await eventsApiClient.SelectEvents(inclusiveStartDate, exclusiveEndDate);
            var eventsToSend = events
                .Where(x => !x.IsCompleted)
                .Where(x => x.ReminderTime < currentDate)
                .ToArray();

            await eventsToSend.ForEachAsync(async x => await telegramBotWrapper.SendMessageAsync(
                x.UserId, $"Напоминание: {x.Description}")
            );

            var sentUserEventIds = eventsToSend
                .Where(x => !x.IsRepeatable)
                .Select(x => x.Id)
                .ToArray();

            await eventsApiClient.MarkEventsAsCompletedAsync(sentUserEventIds);
            eventsReadCount = events.Length;
        } while (eventsReadCount == 100);
    }
}