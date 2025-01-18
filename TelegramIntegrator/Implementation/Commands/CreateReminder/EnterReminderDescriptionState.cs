using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class EnterReminderDescriptionState(
    ILog<EnterReminderDescriptionState> log,
    IReminderBuilder reminderBuilder,
    IStateSwitcher stateSwitcher,
    ITelegramBotClient telegramBotClient,
    IReplyMarkupFactory replyMarkupFactory
) : BaseState<EnterReminderDescriptionState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly IReminderBuilder _reminderBuilder = reminderBuilder;
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override Task InitializeAsync(long userId)
    {
        _reminderBuilder.CreateReminder(userId);
        return _telegramBotClient.SendMessage(userId, "Введите текст напоминания", replyMarkup: ExitReplyMarkup);
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected override Task ReceiveMessageAsync(Message message)
    {
        var userId = message.From!.Id;
        var reminderDescription = message.Text!;
        _reminderBuilder.WithDescription(userId, reminderDescription);
        return _stateSwitcher.SetStateAsync<EnterReminderDateTimeState>(userId);
    }
}