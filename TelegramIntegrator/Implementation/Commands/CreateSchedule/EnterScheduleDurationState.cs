using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.View;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class EnterScheduleDurationState(
    ILog<EnterScheduleDurationState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    ITelegramBotClient telegramBotClient,
    IScheduleBuilder scheduleBuilder
) : BaseState<EnterScheduleDurationState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IScheduleBuilder _scheduleBuilder = scheduleBuilder;

    public override Task InitializeAsync(long userId)
    {
        return _telegramBotClient.SendMessage(userId, $"Введите продолжительность по шаблону\n\n{EnterDataPatterns.TimeSpanPattern}");
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

        _scheduleBuilder.WithScheduleDuration(userId, parseResult.Value);
        return StateSwitcher.SetStateAsync<AnswerCanOverlapScheduleState>(userId);
    }
}