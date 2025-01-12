using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers;

public interface ICommandHandler
{
    string CommandName { get; }
    bool CanExecute(ICommandModel commandModel);
    Task ExecuteAsync(ChatContext context, ICommandModel commandModel);
}