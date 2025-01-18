using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class SendScheduleState(
    ILog<SendScheduleState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    IMaestroApiClient maestroApiClient,
    ITelegramBotClient telegramBotClient,
    IScheduleBuilder scheduleBuilder
) : BaseState<SendScheduleState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IScheduleBuilder _scheduleBuilder = scheduleBuilder;

    public async override Task InitializeAsync(long userId)
    {
        var schedule = _scheduleBuilder.Build(userId);
        try
        {
            var result = await _maestroApiClient.CreateScheduleAsync(schedule);
            var message = result is null
                ? "Есть конфликты с другим расписанием. В грядущим обновлении будем показывать с какими"
                : "Расписание создано";
            await _telegramBotClient.SendMessage(userId, message);
        }
        catch (Exception e)
        {
            Log.Error($"Ошибка при создании расписания {e}");
            await _telegramBotClient.SendMessage(userId, "Что-то пошло не так, приносим свои извинения");
        }

        await StateSwitcher.SetStateAsync<MainState>(userId);
    }
}