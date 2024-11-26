using Client.Models;
using Core.Result;

namespace Client;

public interface IMessageParser
{
    Result<Message> ParseMessage(string input);
}