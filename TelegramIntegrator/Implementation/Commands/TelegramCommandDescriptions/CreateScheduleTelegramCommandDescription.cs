namespace Maestro.TelegramIntegrator.Implementation.Commands.TelegramCommandDescriptions;

public class CreateScheduleTelegramCommandDescription : ITelegramCommandDescription
{
    public string TelegramCommandName => TelegramCommandNames.CreateScheduleTelegramCommand;

    public string TelegramCommandHelpDescription => "Введите детали для создания расписания через запятую:\n" +
                                                    "- команда /schedule,\n- дата и время начала расписания через пробел в формате \"день.месяц.год часы:минуты\",\n" +
                                                    "- дата и время конца расписания через пробел в формате \"день.месяц.год часы:минуты\",\n- текст расписания,\n" +
                                                    "- параметр \"overlap\" - если это расписание может пересекаться с другими";
}