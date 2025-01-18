using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class EnterScheduleDescriptionState(
    ILog<EnterScheduleDescriptionState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    ITelegramBotClient telegramBotClient,
    IScheduleBuilder scheduleBuilder
) : BaseState<EnterScheduleDescriptionState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IScheduleBuilder _scheduleBuilder = scheduleBuilder;

    public override Task InitializeAsync(long userId)
    {
        _scheduleBuilder.CreateSchedule(userId);
        return _telegramBotClient.SendMessage(userId, "Введите текст расписания", replyMarkup: ExitReplyMarkup);
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected override Task ReceiveMessageAsync(Message message)
    {
        var userId = message.From!.Id;
        var text = message.Text!;
        _scheduleBuilder.WithDescription(userId, text);
        return StateSwitcher.SetStateAsync<EnterScheduleDateTimeState>(userId);
    }
}