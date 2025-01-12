using Maestro.TelegramIntegrator.Implementation.Commands.Handlers;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public record CommandBundle(
    ICommandParser CommandParser,
    ICommandHandler CommandHandler
);