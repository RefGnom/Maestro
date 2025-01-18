using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class AnswerCanOverlapScheduleState(
    ILog<AnswerCanOverlapScheduleState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    ITelegramBotClient telegramBotClient,
    IScheduleBuilder scheduleBuilder
) : BaseState<AnswerCanOverlapScheduleState>(log, stateSwitcher, replyMarkupFactory)
{
    private const string AcceptCommand = "/accept";
    private const string DenyCommand = "/deny";

    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IScheduleBuilder _scheduleBuilder = scheduleBuilder;

    public override Task InitializeAsync(long userId)
    {
        var inlineKeyboardMarkup = new InlineKeyboardMarkup()
            .AddButton(InlineKeyboardButton.WithCallbackData("Да", AcceptCommand))
            .AddButton(InlineKeyboardButton.WithCallbackData("Нет", DenyCommand));
        return _telegramBotClient.SendMessage(
            userId,
            "Может ли это расписание пересекаться с другим?",
            replyMarkup: inlineKeyboardMarkup
        );
    }

    protected override Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        var userId = callbackQuery.From.Id;
        if (callbackQuery.Data == AcceptCommand)
        {
            _scheduleBuilder.WithCanOverlap(userId, true);
        }

        return StateSwitcher.SetStateAsync<SendScheduleState>(userId);
    }
}