using Maestro.Core.Result;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation;

public interface IMessageParser
{
    Result<Message> ParseMessage(string input);
}