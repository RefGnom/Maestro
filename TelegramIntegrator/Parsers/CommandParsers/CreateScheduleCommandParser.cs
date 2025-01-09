using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers.CommandParsers;

public class CreateScheduleCommandParser(IDateTimeParser dateTimeParser) : ICommandParser
{
    private readonly IDateTimeParser _dateTimeProvider = dateTimeParser;

    public bool CanParse(string command)
    {
        return command.StartsWith("/schedule");
    }

    public ParseResult<ICommand> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        var telegremCommand = parts[0];
        var startDateTime = ParserHelper.ParseDateTime(parts[1]);

        if (!startDateTime.IsSuccessful)
            return ParseResult<ICommand>.CreateFailure(startDateTime.ParseFailureMessage);

        var endDateTime = ParserHelper.ParseDateTime(parts[2]);

        if (!endDateTime.IsSuccessful)
            return ParseResult<ICommand>.CreateFailure(endDateTime.ParseFailureMessage);

        var description = parts[3];
        bool canOverlap = false;

        if (parts.Length == 5 && parts[4] == "overlap")
            canOverlap = true;

        return ParseResult<ICommand>.CreateSuccess(
                 new CreateScheduleCommand(
                     telegremCommand,
                     description,
                     startDateTime.Value!.Value,
                     endDateTime.Value!.Value,
                     canOverlap
                 )
             );
    }
}