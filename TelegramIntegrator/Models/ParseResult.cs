namespace Maestro.TelegramIntegrator.Models;

public class ParseResult<T>(bool isSuccessful, T Value, string parseFailureMessage)
{
    public bool IsSuccessful { get; } = isSuccessful;
    public T Value { get; } = Value;
    public string ParseFailureMessage { get; } = parseFailureMessage;

    public static ParseResult<T> CreateSuccess(T value) => new(true, value, null!);
    public static ParseResult<T> CreateFailure(string parseFailureMessage) => new(false, default!, parseFailureMessage);
}