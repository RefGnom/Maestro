using Maestro.Client.Models;
using Maestro.Core.Result;

namespace Maestro.Client.Implementation;

public interface IMessageParser
{
    Result<Message> ParseMessage(string input);
}