using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Operational.ProcessesCore;
using Maestro.Operational.TimestampProvider;

namespace Maestro.TelegramIntegrator.Implementation.Processes;

public class SendSchedulesProcess(
    ILog<SendSchedulesProcess> log,
    IDateTimeProvider dateTimeProvider,
    ITimestampProviderFactory timestampProviderFactory,
    IMaestroApiClient maestroApiClient,
    ITelegramBotWrapper telegramBotWrapper)
    : RegularProcessBase<SendSchedulesProcess>(log)
{
    private const string TimestampKey = "SendchedulesProcessTimestamp";
    private static readonly TimeSpan ScheduleSendingEpsilon = TimeSpan.FromSeconds(30);

    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ITimestampProvider _timestampProvider = timestampProviderFactory.Create();
    private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly ITelegramBotWrapper _telegramBotWrapper = telegramBotWrapper;
    private readonly ILog<SendSchedulesProcess> _log = log;

    public override string ProcessName => "Чтение пользовательских расписаний";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Interval => ScheduleSendingEpsilon;

    protected async override Task UnsafeRunAsync()
    {
        var currentDate = _dateTimeProvider.GetCurrentDateTime();
        var exclusiveStartDate = _timestampProvider.Find(TimestampKey) ?? DateTime.Today;
        var maxReminderTime = exclusiveStartDate;

        var schedules = _maestroApiClient.GetAllSchedulesAsync(exclusiveStartDate);
        await foreach (var schedule in schedules)
        {
            if (schedule.StartDateTime - currentDate > ScheduleSendingEpsilon || schedule.IsCompleted)
            {
                continue;
            }

            try
            {
                await _telegramBotWrapper.SendMessageAsync(schedule.UserId, $"Напоминание о расписании \"{schedule.Description}\"");
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
            }

            await _maestroApiClient.SetScheduleCompletedAsync(schedule.ScheduleId);

            if (schedule.StartDateTime > maxReminderTime)
            {
                maxReminderTime = schedule.StartDateTime;
            }
        }

        _timestampProvider.Set(TimestampKey, maxReminderTime);
    }
}