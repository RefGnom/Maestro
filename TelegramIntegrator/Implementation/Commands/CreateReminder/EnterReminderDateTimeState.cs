using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class EnterReminderDateTimeState(
    ILog<EnterReminderDateTimeState> log,
    IReminderBuilder reminderBuilder,
    IStateSwitcher stateSwitcher,
    ITelegramBotClient telegramBotClient,
    IReplyMarkupFactory replyMarkupFactory
) : BaseState<EnterReminderDateTimeState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly IReminderBuilder _reminderBuilder = reminderBuilder;
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override Task Initialize(long userId)
    {
        return _telegramBotClient.SendMessage(userId, "Введите дату напоминания", replyMarkup: ExitReplyMarkup);
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected async override Task ReceiveMessageAsync(Message message)
    {
        var userId = message.From!.Id;
        var dateTimeMessage = message.Text!;
        var parseDateTimeResult = ParserHelper.ParseDateTime(dateTimeMessage);
        if (!parseDateTimeResult.IsSuccessful)
        {
            await _telegramBotClient.SendMessage(userId, parseDateTimeResult.ParseFailureMessage, replyMarkup: ExitReplyMarkup);
            return;
        }

        _reminderBuilder.WithReminderDateTime(userId, parseDateTimeResult.Value!.Value);
        await _stateSwitcher.SetStateAsync<SendReminderState>(userId);
    }
}