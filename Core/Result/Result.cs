namespace Core;

public class Result<T>
{
    public T? Value;
    public bool IsSuccess { get; init; }
    public string Message { get; init;  }
}

public static class Result
{
    public static Result<T> CreateSuccess<T>(T value)
    {
        return new Result<T> { IsSuccess = true, Value = value };
    }

    public static Result<T> CreateFailure<T>(string message)
    {
        return new Result<T> { Message = message };
    }
}