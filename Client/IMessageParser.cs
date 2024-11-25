using Client.Models;
using Core;

namespace Client;

public interface IMessageParser
{
    Result<Message> ParseMessage(string input);
}