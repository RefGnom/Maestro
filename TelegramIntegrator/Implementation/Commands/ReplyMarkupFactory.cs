 using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public class ReplyMarkupFactory : IReplyMarkupFactory
{
    public IReplyMarkup CreateExitToMainMenuReplyMarkup()
    {
        return new InlineKeyboardMarkup()
            .AddButton(InlineKeyboardButton.WithCallbackData("В главное меню", "/exit"));
    }

    public IReplyMarkup CreateOptionsReplyMarkup()
    {
        return new InlineKeyboardMarkup()
            .AddButton(InlineKeyboardButton.WithCallbackData("Ввести свой часовой пояс", "/timezone"))
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithCallbackData("Создать напоминание", "/reminder"))
            .AddButton(InlineKeyboardButton.WithCallbackData("Создать расписание", "/bele"))
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithCallbackData("Мои напоминания", "/my1"))
            .AddButton(InlineKeyboardButton.WithCallbackData("Моё расписание", "/my2"));
    }
}