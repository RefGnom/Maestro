namespace Maestro.TelegramIntegrator.Models;

public interface ICommand
{
    string Command { get; }
    string Description { get; }
}