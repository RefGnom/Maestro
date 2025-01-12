using Maestro.TelegramIntegrator.Implementation.View;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public class ParserHelper
{
    public static ParseResult<DateTime?> ParseDateTime(string dateTime)
    {
        return new DateTimeParser().TryParse(dateTime, out var parsedDateTime)
            ? ParseResult<DateTime?>.CreateSuccess(parsedDateTime)
            : ParseResult<DateTime?>.CreateFailure(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseDateTimeFailureMessage));
    }

    public static ParseResult<int> ParseInt(string integer)
    {
        return (int.TryParse(integer, out var parsedInteger) && parsedInteger > 0)
            ? ParseResult<int>.CreateSuccess(parsedInteger)
            : ParseResult<int>.CreateFailure(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseIntFailureMessage));
    }

    public static ParseResult<TimeSpan> ParseTimeSpan(string timeSpan)
    {
        return TimeSpan.TryParse(timeSpan, out var parsedTimeSpan)
            ? ParseResult<TimeSpan>.CreateSuccess(parsedTimeSpan)
            : ParseResult<TimeSpan>.CreateFailure(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseTimeSpanFailureMessage));
    }
}