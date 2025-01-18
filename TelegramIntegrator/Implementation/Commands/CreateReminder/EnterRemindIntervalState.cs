using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.View;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class EnterRemindIntervalState(
    ILog<EnterRemindIntervalState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    ITelegramBotClient telegramBotClient,
    IReminderBuilder reminderBuilder
) : BaseState<EnterRemindIntervalState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IReminderBuilder _reminderBuilder = reminderBuilder;

    public override Task Initialize(long userId)
    {
        return _telegramBotClient.SendMessage(userId, $"Введите интервал по шаблону\n\n{EnterDataPatterns.TimeSpanPattern}");
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected override Task ReceiveMessageAsync(Message message)
    {
        var userId = message.From!.Id;
        var text = message.Text!;

        var parseResult = ParserHelper.ParseTimeSpan(text);
        if (!parseResult.IsSuccessful)
        {
            return _telegramBotClient.SendMessage(userId, parseResult.ParseFailureMessage);
        }

        _reminderBuilder.WithRemindInterval(userId, parseResult.Value);
        return StateSwitcher.SetStateAsync<SendReminderState>(userId);

    }
}