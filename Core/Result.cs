namespace Core;

public class Result<T>
{
    public bool IsSuccess;
    public T? Value;
    public string Message;
    
    public static Result<T> CreateSuccess(T value)
    {
        return new Result<T> { IsSuccess = true, Value = value };
    }

    public static Result<T> CreateFailure(string message)
    {
        return new Result<T> { IsSuccess = false, Message = message };
    }
}