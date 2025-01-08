using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Core.Models;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.Parsers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    //IMaestroApiClient maestroApiClient,
    ICommandParser commandParser,
    IDateTimeProvider dateTimeProvider,
    ICommandHandler[] commandHandlers
)
    : IMaestroCommandHandler
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ILog<MaestroCommandHandler> _log = log;
    //private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly ICommandParser _commandParser = commandParser;
    private readonly ICommandHandler[] _commandHandlers = commandHandlers;

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message is null)
        {
            return;
        }

        if (_commandParser.TryParseCommand(update.Message.Text!, out var parsedCommand))
        {
            var executed = false; 
            foreach (var handler in _commandHandlers)
            {
                if (handler.CanExecute(parsedCommand))
                {
                    await handler.ExecuteAsync(bot, update.Message.Chat.Id, parsedCommand, cancellationToken);
                    executed = true;
                    break;
                }
            }
            if (!executed)
            {
                await bot.SendMessage(update.Message.Chat.Id,
                        "Не удалось распознать команду.",
                        cancellationToken: cancellationToken);                
            }
                }
        }
        else
        {
            _log.Warn("Failed to parse message");
            await bot.SendMessage(
                update.Message.Chat.Id,
                "Чтобы создать напоминание или расписание используйте команду /reminder {время напоминания} {описание}" +
                "или /schedule {время напоминания} {описание}.",
                cancellationToken: cancellationToken
            );
        }
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}