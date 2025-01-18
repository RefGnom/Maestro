using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class SendReminderState(
    ILog<SendReminderState> log,
    IMaestroApiClient maestroApiClient,
    IReminderBuilder reminderBuilder,
    IStateSwitcher stateSwitcher,
    ITelegramBotClient telegramBotClient,
    IReplyMarkupFactory replyMarkupFactory
) : BaseState<SendReminderState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly IReminderBuilder _reminderBuilder = reminderBuilder;
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public async override Task InitializeAsync(long userId)
    {
        var reminderDto = _reminderBuilder.Build(userId);
        try
        {
            await _maestroApiClient.CreateReminderAsync(reminderDto);
            await _telegramBotClient.SendMessage(userId, "Напоминание создано");
        }
        catch (Exception e)
        {
            Log.Error($"Ошибка при создании напоминания {e}");
            await _telegramBotClient.SendMessage(userId, "Что-то пошло не так, приносим свои извинения");
        }

        await _stateSwitcher.SetStateAsync<MainState>(userId);
    }
}