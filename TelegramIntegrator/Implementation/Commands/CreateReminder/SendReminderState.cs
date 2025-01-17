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

    public async override Task Initialize(long userId)
    {
        var reminderDto = _reminderBuilder.Build(userId);
        await _telegramBotClient.SendMessage(userId, "Типо отправил напоминание в сервер");
        await _stateSwitcher.SetStateAsync<MainState>(userId);
        await _maestroApiClient.CreateReminderAsync(reminderDto);
    }
}