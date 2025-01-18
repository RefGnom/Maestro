using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class CreateReminderCommandParser(IDateTimeProvider dateTimeProvider) : CommandParserBase
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public override string CommandName => TelegramCommandNames.CreateReminder;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        if (parts.Length < 3)
        {
            return ParseResult.CreateFailure<ICommandModel>(TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateReminderCommandPattern));
        }

        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();
        var dateTimeParseResult = ParserHelper.ParseDateTime(parts[1], currentDateTime);

        if (!dateTimeParseResult.IsSuccessful)
        {
            return ParseResult.CreateFailure<ICommandModel>(dateTimeParseResult.ParseFailureMessage);
        }

        var description = parts[2];
        var remindCount = 1;
        var remindInterval = TimeSpan.FromMinutes(5);

        if (parts.Length > 3)
        {
            var parserIntResult = ParserHelper.ParseInt(parts[3]);
            if (!parserIntResult.IsSuccessful)
            {
                return ParseResult.CreateFailure<ICommandModel>(parserIntResult.ParseFailureMessage);
            }

            remindCount = parserIntResult.Value;
        }

        if (parts.Length > 4)
        {
            var parserTimeSpanResult = ParserHelper.ParseTimeSpan(parts[4]);
            if (!parserTimeSpanResult.IsSuccessful)
            {
                return ParseResult.CreateFailure<ICommandModel>(parserTimeSpanResult.ParseFailureMessage);
            }

            remindInterval = parserTimeSpanResult.Value;
        }

        return ParseResult.CreateSuccess<ICommandModel>(
            new CreateReminderCommandModel(
                dateTimeParseResult.Value!.Value,
                description,
                remindCount,
                remindInterval
            )
        );
    }
}