namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class CreateScheduleHelpCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.CreateSchedule;
    public string HelpDescription => "Введите детали для создания расписания через запятую:\n" +
                                     "- команда /schedule,\n- дата и время начала расписания через пробел в формате \"день.месяц.год часы:минуты\",\n" +
                                     "- дата и время конца расписания через пробел в формате \"день.месяц.год часы:минуты\",\n- текст расписания,\n" +
                                     "- параметр \"overlap\" - если это расписание может пересекаться с другими";
}