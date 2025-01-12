using Maestro.TelegramIntegrator.Implementation.Commands.CommandHandlers;
using Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public record CommandBundle(
    ICommandParser CommandParser,
    ICommandHandler CommandHandler
);