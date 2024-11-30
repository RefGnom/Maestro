using Maestro.Client.Models;
using Maestro.Core.Result;

namespace Maestro.Client;

public interface IMessageParser
{
    Result<Message> ParseMessage(string input);
}