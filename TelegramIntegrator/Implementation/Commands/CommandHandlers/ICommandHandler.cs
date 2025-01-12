using Maestro.TelegramIntegrator.Implementation.Commands.CommandsModels;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandHandlers;

public interface ICommandHandler
{
    string Name { get; }
    bool CanExecute(ICommandModel commandModel);
    Task ExecuteAsync(ChatContext context, ICommandModel commandModel);
}