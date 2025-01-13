namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class CreateScheduleCommandModel(
    DateTime startDateTime,
    TimeSpan duration,
    string description,
    bool canOverlap
) : ICommandModel
{
    public DateTime StartDateTime { get; } = startDateTime;
    public TimeSpan Duration { get; } = duration;
    public string ScheduleDescription { get; } = description;
    public bool CanOverlap { get; } = canOverlap;
    public string TelegramCommand => TelegramCommandNames.CreateSchedule;

    public string HelpDescription => "Введите детали для создания расписания через запятую:\n" +
                                     "- команда /schedule,\n- дата и время начала расписания через пробел в формате \"день.месяц.год часы:минуты\",\n" +
                                     "- продолжительность расписания в формате \"дни:часы:минуты\",\n- текст расписания,\n" +
                                     "- параметр \"overlap\" - если это расписание может пересекаться с другими";
}