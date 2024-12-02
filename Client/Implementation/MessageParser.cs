using Maestro.Client.Models;
using Maestro.Core.Providers;
using Maestro.Core.Result;

namespace Maestro.Client.Implementation;

public class MessagesParser(IDateTimeProvider dateTimeProvider) : IMessageParser
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public Result<Message> ParseMessage(string message)
    {
        if (!message.StartsWith("/create"))
        {
            return Result.CreateFailure<Message>("Incorrect message entered");
        }

        var parts = message.Split(" ");
        var command = parts[0];
        var date = parts[1];
        var time = parts[2];
        var hasDate = time.All(c => !char.IsLetter(c));
        if (hasDate)
        {
            var dateTimeParseResult = _dateTimeProvider.TryParse(time, date);
            if (dateTimeParseResult.IsSuccessful)
            {
                return Result.CreateSuccess(
                    new Message(
                        command,
                        dateTimeParseResult.Value,
                        string.Join(" ", parts.Skip(3))
                    )
                );
            }
        }
        else
        {
            var timeParseResult = _dateTimeProvider.TryParse(date);
            if (timeParseResult.IsSuccessful)
            {
                return Result.CreateSuccess(
                    new Message(
                        command,
                        timeParseResult.Value,
                        string.Join(" ", parts.Skip(2))
                    )
                );
            }
        }

        return Result.CreateFailure<Message>("Incorrect message entered");
    }
}