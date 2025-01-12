using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public interface ITelegramCommandMapper
{
    CommandBundle? MapCommandBundle(string userMessage);
}