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
            .AddButton("Ввести свой часовой пояс", TelegramCommandNames.SetTimeZone)
            .AddNewRow()
            .AddButton("Создать напоминание", TelegramCommandNames.CreateReminderStepByStep)
            .AddButton("Создать расписание", TelegramCommandNames.CreateScheduleStepByStep)
            .AddNewRow()
            .AddButton("Мои напоминания", TelegramCommandNames.ViewReminders)
            .AddButton("Моё расписание", TelegramCommandNames.ViewSchedules);
    }
}