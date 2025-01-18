using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.View;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class EnterReminderDateTimeState(
    ILog<EnterReminderDateTimeState> log,
    IReminderBuilder reminderBuilder,
    IStateSwitcher stateSwitcher,
    ITelegramBotClient telegramBotClient,
    IReplyMarkupFactory replyMarkupFactory,
    IDateTimeProvider dateTimeProvider
) : BaseState<EnterReminderDateTimeState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly IReminderBuilder _reminderBuilder = reminderBuilder;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public override Task Initialize(long userId)
    {
        return _telegramBotClient.SendMessage(
            userId,
            $"Введите дату и время напоминания по шаблону\n\n{EnterDataPatterns.DateTimePattern}",
            replyMarkup: ExitReplyMarkup
        );
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected override Task ReceiveMessageAsync(Message message)
    {
        var userId = message.From!.Id;
        var dateTimeMessage = message.Text!;
        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();
        var parseDateTimeResult = ParserHelper.ParseDateTime(dateTimeMessage, currentDateTime);
        if (!parseDateTimeResult.IsSuccessful)
        {
            return _telegramBotClient.SendMessage(
                userId,
                $"Ошибка ввода, пожалуйста, используйте шаблон\n\n{EnterDataPatterns.DateTimePattern}",
                replyMarkup: ExitReplyMarkup
            );
        }

        _reminderBuilder.WithReminderDateTime(userId, parseDateTimeResult.Value!.Value);
        return StateSwitcher.SetStateAsync<AnswerIsRepeatableReminderState>(userId);
    }
}