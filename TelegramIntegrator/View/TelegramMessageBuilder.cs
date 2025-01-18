using Maestro.TelegramIntegrator.Implementation.Commands;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;

namespace Maestro.TelegramIntegrator.View;

public static class TelegramMessageBuilder
{
    public static string BuildByCommandPattern(string telegramCommandPattern) => $"Пожалуйста, введите через запятую детали для создания {telegramCommandPattern}";

    public static string BuildParseFailureMessage(string parseFailureMessage) => $"Не удалось распарсить: {parseFailureMessage}";

    public static string BuildUnknownCommand()
    {
        return $"Не смогли распознать введёную вами команду. Список возможных команд:" +
               $"\n{string.Join('\n', TelegramCommandNames.GetCommandNames())}";
    }

    public static string BuildReminderCreatedMessage(CreateReminderCommandModel reminderCommandModel)
    {
        var reminderRepeatMessage = "";
        if (reminderCommandModel.RemindCount > 1)
        {
            reminderRepeatMessage = $", повторная отправка напоминания: {reminderCommandModel.RemindCount} раз(а) " +
                                    $"через {reminderCommandModel.RemindInterval.Days} д {reminderCommandModel.RemindInterval.Hours} ч {reminderCommandModel.RemindInterval.Minutes} м.";
        }


        return $"Напоминание \"{reminderCommandModel.ReminderDescription}\" создано на время {reminderCommandModel.ReminderTime:dd.MM.yyyy HH:mm}{reminderRepeatMessage}";
    }
}