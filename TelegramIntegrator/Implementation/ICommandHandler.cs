using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation
{
    public interface ICommandHandler
    {
        bool CanExecute(ICommand command);
        Task ExecuteAsync(ITelegramBotClient bot, long chatId, ICommand command, CancellationToken cancellationToken);
    }
}
