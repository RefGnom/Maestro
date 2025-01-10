using Maestro.TelegramIntegrator.Implementation.Commands.CommandHandlers;
using Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;
using Maestro.TelegramIntegrator.Implementation.Commands.TelegramCommandDescriptions;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public class TelegramCommandMapper : ITelegramCommandMapper
{
    private readonly CommandBundle[] _commandBundles;

    public TelegramCommandMapper(
        IEnumerable<ICommandParser> commandParsers,
        IEnumerable<ICommandHandler> commandHandlers,
        IEnumerable<ITelegramCommandDescription> telegramCommandDescriptions
    )
    {
        var parsers = commandParsers.ToArray();
        var handlers = commandHandlers.ToArray();
        var commandDescriptions = telegramCommandDescriptions.ToArray();

        _commandBundles = commandDescriptions.Select(
            commandDescription =>
            {
                var telegramCommandName = commandDescription.TelegramCommandName;
                var commandParser = parsers.FirstOrDefault(x => x.TelegramCommandName == telegramCommandName);
                if (commandParser is null)
                {
                    throw new InvalidTelegramCommandBundleException($"Неправильно определена связка для команды '{telegramCommandName}'. Не смогли найти парсера");
                }

                var commandHandler = handlers.FirstOrDefault(x => x.TelegramCommandName == telegramCommandName);
                if (commandHandler is null)
                {
                    throw new InvalidTelegramCommandBundleException($"Неправильно определена связка для команды '{telegramCommandName}'. Не смогли найти хэндлера");
                }

                return new CommandBundle(commandDescription, commandParser, commandHandler);
            }
        ).ToArray();
    }

    public CommandBundle? MapCommandBundle(string userMessage)
    {
        return _commandBundles.FirstOrDefault(
            x => userMessage.StartsWith(x.TelegramCommandDescription.TelegramCommandName)
        );
    }
}