using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class EnterRemindCountState(
    ILog<EnterRemindCountState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    ITelegramBotClient telegramBotClient,
    IReminderBuilder reminderBuilder
) : BaseState<EnterRemindCountState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IReminderBuilder _reminderBuilder = reminderBuilder;

    public override Task Initialize(long userId)
    {
        var inlineKeyboardMarkup = new InlineKeyboardMarkup()
            .AddButton(InlineKeyboardButton.WithCallbackData("1", "1"))
            .AddButton(InlineKeyboardButton.WithCallbackData("2", "2"))
            .AddButton(InlineKeyboardButton.WithCallbackData("3", "3"))
            .AddButton(InlineKeyboardButton.WithCallbackData("4", "4"))
            .AddButton(InlineKeyboardButton.WithCallbackData("5", "5"))
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithCallbackData("Бесконечно", int.MaxValue.ToString()));

        return _telegramBotClient.SendMessage(
            userId,
            "Сколько раз напомнить? Выберите или введите вручную",
            replyMarkup: inlineKeyboardMarkup
        );
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected override Task ReceiveMessageAsync(Message message)
    {
        return HandleCount(message.From!.Id, message.Text!);
    }

    protected override Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        return HandleCount(callbackQuery.From.Id, callbackQuery.Data!);
    }

    private Task HandleCount(long userId, string countText)
    {
        if (!int.TryParse(countText, out var count))
        {
            return _telegramBotClient.SendMessage(userId, "Не смогли определить введёное вами количество, попробуйте ещё раз");
        }

        _reminderBuilder.WithRemindCount(userId, count);
        return StateSwitcher.SetStateAsync<AnswerChangeIntervalState>(userId);

    }
}