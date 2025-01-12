namespace Maestro.TelegramIntegrator.Implementation.Commands.TelegramCommandDescriptions;

public interface ITelegramCommandDescription
{
    string TelegramCommandName { get; }
    string TelegramCommandHelpDescription { get; }
}