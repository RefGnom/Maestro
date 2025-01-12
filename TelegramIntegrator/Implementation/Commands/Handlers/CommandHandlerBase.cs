using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers;

public abstract class CommandHandlerBase<TCommand> : ICommandHandler
    where TCommand : class, ICommandModel
{
    public abstract string Name { get; }
    public bool CanExecute(ICommandModel commandModel) => commandModel is TCommand;
    protected abstract Task ExecuteAsync(ChatContext context, TCommand command);

    public Task ExecuteAsync(ChatContext context, ICommandModel commandModel)
    {
        if (commandModel is not TCommand genericCommand)
        {
            throw new ArgumentException($"Ожидалась команда типа {typeof(TCommand)}, но передали {commandModel.GetType()}");
        }

        return ExecuteAsync(context, genericCommand);
    }
}