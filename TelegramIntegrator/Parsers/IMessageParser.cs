using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers;

public interface IMessageParser
{
    bool TryParse(string input, out Message? message);
}