using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class AnswerIsRepeatableReminderState(
    ILog<AnswerIsRepeatableReminderState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    ITelegramBotClient telegramBotClient
) : BaseState<AnswerIsRepeatableReminderState>(log, stateSwitcher, replyMarkupFactory)
{
    private const string AcceptCommand = "/accept";
    private const string DenyCommand = "/deny";

    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override Task InitializeAsync(long userId)
    {
        var inlineKeyboardMarkup = new InlineKeyboardMarkup()
            .AddButton(InlineKeyboardButton.WithCallbackData("Да", AcceptCommand))
            .AddButton(InlineKeyboardButton.WithCallbackData("Нет", DenyCommand));
        return _telegramBotClient.SendMessage(userId, "Повторять напоминание?", replyMarkup: inlineKeyboardMarkup);
    }

    protected override Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        var userId = callbackQuery.From.Id;
        return callbackQuery.Data switch
        {
            AcceptCommand => StateSwitcher.SetStateAsync<EnterRemindCountState>(userId),
            DenyCommand => StateSwitcher.SetStateAsync<SendReminderState>(userId),
            _ => Task.CompletedTask
        };
    }
}