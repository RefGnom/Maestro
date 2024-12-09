namespace Maestro.Core.Result;

public class Result<T>
{
    public T? Value { get; init; }
    public bool IsSuccessful { get; init; }
    public bool IsFailure => !IsSuccessful;
    public string Message { get; init; } = string.Empty;
}

public static class Result
{
    public static Result<T> CreateSuccess<T>(T value)
    {
        return new Result<T> { IsSuccessful = true, Value = value };
    }

    public static Result<T> CreateFailure<T>(string message)
    {
        return new Result<T> { Message = message };
    }
}