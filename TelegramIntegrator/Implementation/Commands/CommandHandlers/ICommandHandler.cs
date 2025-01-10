using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandHandlers;

public interface ICommandHandler
{
    string TelegramCommandName { get; }
    bool CanExecute(ICommand command);
    Task ExecuteAsync(long chatId, ICommand command, CancellationToken cancellationToken);
}