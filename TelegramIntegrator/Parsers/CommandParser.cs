using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers;

public class CommandParser(IDateTimeParser dateTimeParser) : ICommandParser
{
    private readonly IDateTimeParser _dateTimeProvider = dateTimeParser;

    public bool TryParseCommand(string command, out ICommand? commandResult)
    {
        if(command.StartsWith("/reminder"))
        {
            return TryParseReminderCommand(command, out commandResult);
        }
        else if (command.StartsWith("/schedule"))
        {
            return TryParseScheduleCommand(command, out commandResult);
        }

        commandResult = null;

        return false;
    }

    public bool TryParseReminderCommand(string command, out ICommand? commandResult)
    {
        var parts = command.Split(" ");
        var telegramCommand = parts[0];
        var date = parts[1];
        var time = parts[2];
        var hasDate = time.All(c => !char.IsLetter(c));
        if (hasDate)
        {
            if (!_dateTimeProvider.TryParse(time, date, out var dateTimeParseResult))
            {
                commandResult = null;
                return false;
            }

            commandResult = new ReminderCommand(telegramCommand, parts.Last(), dateTimeParseResult!.Value);
            return true;
        }
        else
        {
            if (_dateTimeProvider.TryParse(date, null, out var dateTimeParseResult))
            {
                commandResult = new ReminderCommand(telegramCommand, parts.Last(), dateTimeParseResult!.Value);
                return true;
            }
        }

        commandResult = null;

        return false;
    }

    public bool TryParseScheduleCommand(string command, out ICommand? commandResult)
    {
        var parts = command.Split(" ");
        var telegremCommand = parts[0];
        var startDate = parts[1];
        var startTime = parts[2];
        var endDate = parts[3];
        var endTime = parts[4];
        var description = string.Join(" ", parts.Skip(5));
        bool canOverlap = false;

        if (description.EndsWith("overlap", StringComparison.OrdinalIgnoreCase))
        {
            canOverlap = true;
            description = description.Substring(0, description.Length - "overlap".Length).Trim();
        }

        if (_dateTimeProvider.TryParse(startTime, startDate, out var startDateTime) &&
            _dateTimeProvider.TryParse(endTime, endDate, out var endDateTime))
        {
            commandResult = new ScheduleCommand(telegremCommand, description,
                startDateTime!.Value, endDateTime!.Value, canOverlap);
            return true;
        }

        commandResult = null;
        return false;
    }
}