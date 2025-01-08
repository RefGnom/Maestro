using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers;

public class CommandParser(IDateTimeParser dateTimeParser) : ICommandParser
{
    private readonly IDateTimeParser _dateTimeProvider = dateTimeParser;

    public ParseResult ParseCommand(string command)
    {
        if (command.StartsWith("/reminder"))
        {
            return TryParseReminderCommand(command);
        }

        if (command.StartsWith("/schedule"))
        {
            return TryParseScheduleCommand(command);
        }

        return ParseResult.CreateFailure("Неизвестная комманда");
    }

    public ParseResult TryParseReminderCommand(string command)
    {
        var parts = command.Split(" ");
        var telegramCommand = parts[0];
        var date = parts[1];
        var time = parts[2];
        var hasDate = time.All(c => !char.IsLetter(c));

        if (hasDate)
        {
            return !_dateTimeProvider.TryParse(time, date, out var dateTimeParseResult)
                ? ParseResult.CreateFailure("Не удалось распарсить дату напоминания") // Напиши сюда формат
                : ParseResult.CreateSuccess(new CreateReminderCommand(telegramCommand, parts.Last(), dateTimeParseResult!.Value));
        }
        else
        {
            return !_dateTimeProvider.TryParse(date, null, out var dateTimeParseResult)
                ? ParseResult.CreateFailure("Не удалось распарсить дату напоминания") // Напиши сюда формат
                : ParseResult.CreateSuccess(new CreateReminderCommand(telegramCommand, parts.Last(), dateTimeParseResult!.Value));
        }
    }

    public ParseResult TryParseScheduleCommand(string command)
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
            return ParseResult.CreateSuccess(
                new CreateScheduleCommand(
                    telegremCommand,
                    description,
                    startDateTime!.Value,
                    endDateTime!.Value,
                    canOverlap
                )
            );
        }

        return ParseResult.CreateFailure("Не удалось распарсить дату расписания"); // Напиши сюда формат
    }
}