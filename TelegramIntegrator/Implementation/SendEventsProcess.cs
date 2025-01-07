using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Operational.ProcessesCore;
using Maestro.Operational.TimestampProvider;

namespace Maestro.TelegramIntegrator.Implementation;

public class SendEventsProcess(
    ILog<SendEventsProcess> log,
    IMaestroApiClient maestroApiClient,
    IDateTimeProvider dateTimeProvider,
    ITelegramBotWrapper telegramBotWrapper,
    ITimestampProviderFactory timestampProviderFactory
) : RegularProcessBase<SendEventsProcess>(log)
{
    private const string TimestampKey = "SendEventsProcessTimestamp";
    private static readonly TimeSpan RemindSendingEpsilon = TimeSpan.FromSeconds(30);

    private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly ITimestampProvider _timestampProvider = timestampProviderFactory.Create();

    public override string ProcessName => "Чтение пользовательских событий";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Interval => RemindSendingEpsilon;

    protected async override Task TryRunAsync()
    {
        var currentDate = dateTimeProvider.GetCurrentDateTime();
        var exclusiveStartDate = _timestampProvider.Get(TimestampKey);
        var maxReminderTime = exclusiveStartDate;

        var reminders = _maestroApiClient.GetRemindersForUserAsync(exclusiveStartDate);
        await foreach (var reminder in reminders)
        {
            if (reminder.RemindCount == 0 || reminder.ReminderTime - currentDate > RemindSendingEpsilon)
            {
                continue;
            }

            await telegramBotWrapper.SendMessageAsync(reminder.UserId, $"Напоминание: {reminder.Description}");
            if (reminder.RemindCount == 1)
            {
                await _maestroApiClient.MarkRemindersAsCompletedAsync(reminder.UserId);
            }
            else
            {
                await _maestroApiClient.SetReminderTimeAsync(reminder.Id, reminder.ReminderTime + reminder.RemindInterval);
                await _maestroApiClient.DecrementRemindCountAsync(reminder.Id);
            }

            if (reminder.ReminderTime > maxReminderTime)
            {
                maxReminderTime = reminder.ReminderTime;
            }
        }

        _timestampProvider.Set(TimestampKey, maxReminderTime);
    }
}