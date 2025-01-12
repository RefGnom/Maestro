namespace Maestro.TelegramIntegrator.Models;

public class ParseResult<T>(bool isSuccessful, T value, string parseFailureMessage)
{
    public bool IsSuccessful { get; } = isSuccessful;
    public T Value { get; } = value;
    public string ParseFailureMessage { get; } = parseFailureMessage;

    public static ParseResult<T> CreateSuccess(T value)
    {
        return new ParseResult<T>(isSuccessful: true, value, null!);
    }

    public static ParseResult<T> CreateFailure(string parseFailureMessage)
    {
        return new ParseResult<T>(isSuccessful: false, default!, parseFailureMessage);
    }
}