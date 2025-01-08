using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.CommandHandlers
{
    public interface ICommandHandler
    {
        bool CanExecute(ICommand command);
        Task ExecuteAsync(long chatId, ICommand command, CancellationToken cancellationToken);
    }
}