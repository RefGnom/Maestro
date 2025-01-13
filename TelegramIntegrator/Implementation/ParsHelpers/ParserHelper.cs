using Maestro.TelegramIntegrator.View;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public class ParserHelper
{
    public static ParseResult<DateTime?> ParseDateTime(string dateTime)
    {
        return new DateTimeParser().TryParse(dateTime, out var parsedDateTime)
            ? ParseResult.CreateSuccess(parsedDateTime)
            : ParseResult.CreateFailure<DateTime?>(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseDateTimeFailureMessage));
    }

    public static ParseResult<int> ParseInt(string integer)
    {
        return (int.TryParse(integer, out var parsedInteger) && parsedInteger > 0)
            ? ParseResult.CreateSuccess(parsedInteger)
            : ParseResult.CreateFailure<int>(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseIntFailureMessage));
    }

    public static ParseResult<TimeSpan> ParseTimeSpan(string timeSpan)
    {
        return TimeSpan.TryParse(timeSpan, out var parsedTimeSpan)
            ? ParseResult.CreateSuccess(parsedTimeSpan)
            : ParseResult.CreateFailure<TimeSpan>(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseTimeSpanFailureMessage));
    }
}