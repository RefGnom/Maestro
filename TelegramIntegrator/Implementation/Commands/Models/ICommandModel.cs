namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public interface ICommandModel
{
    string TelegramCommand { get; }
    string HelpDescription { get; }
}