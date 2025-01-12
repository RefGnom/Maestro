namespace Maestro.TelegramIntegrator.Implementation.Commands.TelegramCommandDescriptions;

public class CreateReminderTelegramCommandDescription : ITelegramCommandDescription
{
    public string TelegramCommandName => TelegramCommandNames.CreateReminderTelegramCommand;

    public string TelegramCommandHelpDescription => "Введите детали для создания напоминания через запятую:\nОбязательные детали:\n" +
                                                    "- команда /reminder,\n- дата и время через пробел в формате \"день.месяц.год часы:минуты\",\n- текст напоминания,\n" +
                                                    "Необязательные параметры:\n" +
                                                    "- количество повторной отправки напоминания - если хотите получить напоминание несколько раз, чтобы точно не забыть,\n" +
                                                    "- интервал между повторной отправкой в формате \"часы:минуты:секунды\" (по умолчанию 5 минут) " +
                                                    "- если хотите поменять значение интервала по умолчанию";
}