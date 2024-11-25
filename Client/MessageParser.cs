using Client.Models;
using Core;
using Core.Providers;

namespace Client;

public class MessagesParser(IDateTimeProvider dateTimeProvider) : IMessageParser
{
    public Result<Message> ParseMessage(string message)
    {
        if (!message.StartsWith("/create"))
        {
            return Result.CreateFailure<Message>("Incorrect message entered");
        }

        var parts = message.Split(" ");
        var hasDate = parts[2].All(c => !char.IsLetter(c));
        if (hasDate)
        {
            var parseDateTimeResult = dateTimeProvider.TryParse(parts[2], parts[1]);
            if (parseDateTimeResult.IsSuccessful)
            {
                return Result.CreateSuccess(new Message(parts[0], parseDateTimeResult.Value,
                    string.Join(" ", parts.Skip(3))));
            }
        }
        else
        {
            var parseTimeResult = dateTimeProvider.TryParse(parts[1]);
            if (parseTimeResult.IsSuccessful)
            {
                return Result.CreateSuccess(new Message(parts[0], parseTimeResult.Value,
                    string.Join(" ", parts.Skip(2))));
            }
        }

        return Result.CreateFailure<Message>("Incorrect message entered");
    }
}