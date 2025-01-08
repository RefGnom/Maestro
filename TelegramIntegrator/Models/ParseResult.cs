namespace Maestro.TelegramIntegrator.Models;

public class ParseResult(bool isSuccessful, string parseFailureMessage, ICommand command)
{
    public bool IsSuccessful { get; } = isSuccessful;
    public string ParseFailureMessage { get; } = parseFailureMessage;
    public ICommand Command { get; } = command;

    public static ParseResult CreateSuccess(ICommand command) => new(true, null!, command);
    public static ParseResult CreateFailure(string parseFailureMessage) => new(false, parseFailureMessage, null!);
}