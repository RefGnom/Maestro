using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Implementation.View;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

public class CreateScheduleCommandParser : ICommandParser
{
    public string TelegramCommandName => TelegramCommandNames.CreateScheduleTelegramCommand;

    public bool CanParse(string command)
    {
        return command.StartsWith(TelegramCommandName);
    }

    public ParseResult<ICommand> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        if (parts.Length < 4)
        {
            return ParseResult<ICommand>.CreateFailure(TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateScheduleCommandPattern));
        }

        var telegramCommand = parts[0];
        var startDateTime = ParserHelper.ParseDateTime(parts[1]);

        if (!startDateTime.IsSuccessful)
        {
            return ParseResult<ICommand>.CreateFailure(startDateTime.ParseFailureMessage);
        }

        var endDateTime = ParserHelper.ParseDateTime(parts[2]);

        if (!endDateTime.IsSuccessful)
        {
            return ParseResult<ICommand>.CreateFailure(endDateTime.ParseFailureMessage);
        }

        var description = parts[3];
        var canOverlap = parts.Length == 5 && parts[4] == "overlap";

        return ParseResult<ICommand>.CreateSuccess(
            new CreateScheduleCommand(
                telegramCommand,
                description,
                startDateTime.Value!.Value,
                endDateTime.Value!.Value,
                canOverlap
            )
        );
    }
}