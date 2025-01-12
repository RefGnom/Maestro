using Maestro.TelegramIntegrator.Implementation.Commands.Handlers;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public class TelegramCommandMapper : ITelegramCommandMapper
{
    private readonly CommandBundle[] _commandBundles;

    public TelegramCommandMapper(
        IEnumerable<ICommandParser> commandParsers,
        IEnumerable<ICommandHandler> commandHandlers
    )
    {
        var parsers = commandParsers.ToArray();
        var handlers = commandHandlers.ToArray();

        _commandBundles = parsers.Select(
            commandParser =>
            {
                var telegramCommandName = commandParser.Name;

                var commandHandler = handlers.FirstOrDefault(x => x.Name == telegramCommandName);
                if (commandHandler is null)
                {
                    throw new InvalidTelegramCommandBundleException($"Неправильно определена связка для команды '{telegramCommandName}'. Не смогли найти хэндлера");
                }

                return new CommandBundle(commandParser, commandHandler);
            }
        ).ToArray();
    }

    public CommandBundle? MapCommandBundle(string userMessage)
    {
        return _commandBundles.FirstOrDefault(
            x => userMessage.StartsWith(x.CommandParser.Name)
        );
    }
}