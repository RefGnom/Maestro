using Maestro.TelegramIntegrator.Implementation.Commands.CommandHandlers;
using Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;
using Maestro.TelegramIntegrator.Implementation.Commands.TelegramCommandDescriptions;

namespace Maestro.TelegramIntegrator.Models;

public record CommandBundle(
    ITelegramCommandDescription TelegramCommandDescription,
    ICommandParser CommandParser,
    ICommandHandler CommandHandler
);