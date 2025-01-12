namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandsModels;

public interface ICommandModel
{
    string TelegramCommand { get; }
    string HelpDescription { get; }
}