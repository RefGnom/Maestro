using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation;

public class TelegramBotWrapper(ITelegramBotClient telegramBotClient) : ITelegramBotWrapper
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public async Task SendMessageAsync(long userId, string message)
    {
        await _telegramBotClient.SendMessage(userId, message);
    }

    public async Task SendMainMenu(long chatId, string message, CancellationToken cancellationToken)
    {
        var inlineKeyboardMarkup = new InlineKeyboardMarkup(
            [
                [
                    InlineKeyboardButton.WithCallbackData("Создать напоминание", "create_reminder")
                ],
                [
                    InlineKeyboardButton.WithCallbackData("Создать расписание", "create_schedule")
                ]
                //[
                //    InlineKeyboardButton.WithCallbackData("Посмотреть мои напоминания", "get_reminders")
                //],
                //[

                //    InlineKeyboardButton.WithCallbackData("Посмотреть мои расписания", "get_schedules")
                //]
            ]
        );

        await _telegramBotClient.SendMessage(chatId, message + " Выберите действие:", replyMarkup: inlineKeyboardMarkup, cancellationToken: cancellationToken);
    }
}
