using Maestro.TelegramIntegrator.View;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public class ParserHelper
{
    private static readonly DateTimeParser DateTimeParser = new();

    public static ParseResult<DateTime?> ParseDateTime(string inputDateTime, DateTime messageDateTime)
    {
        if (DateTimeParser.TryParse(inputDateTime, messageDateTime, out var parsedDateTime))
        {
            return ParseResult.CreateSuccess(parsedDateTime);
        }

        var dateParts = inputDateTime.Split(' ');
        if (dateParts.Length == 2)
        {
            var date = dateParts[0];
            var time = dateParts[1];

            return DateTimeParser.TryParse($"{date}.{messageDateTime.Year} {time}", messageDateTime, out var dateTime)
                ? ParseResult.CreateSuccess(dateTime)
                : ParseResult.CreateFailure<DateTime?>(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseDateTimeFailureMessage));
        }

        if (dateParts.Length == 1)
        {
            return new DateTimeParser().TryParse($"{messageDateTime.Date:dd.MM.yyyy} {dateParts[0]}", messageDateTime, out var dateTime)
                ? ParseResult.CreateSuccess(dateTime)
                : ParseResult.CreateFailure<DateTime?>(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseDateTimeFailureMessage));
        }

        return ParseResult.CreateFailure<DateTime?>(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseDateTimeFailureMessage));
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