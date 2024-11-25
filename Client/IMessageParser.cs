using Core;

namespace Client;

public interface IMessageParser<T>
{
    Result<T> ParseMessage(string input);
}