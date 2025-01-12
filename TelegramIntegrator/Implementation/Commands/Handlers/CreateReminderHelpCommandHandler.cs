using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers;

public class CreateReminderHelpCommandHandler(ITelegramBotClient telegramBotClient)
    : CommandHandlerBase<CreateReminderHelpCommandModel>
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override string Name => TelegramCommandNames.CreateReminderHelp;

    protected override Task ExecuteAsync(ChatContext context, CreateReminderHelpCommandModel helpCommand)
    {
        return _telegramBotClient.SendMessage(context.ChatId, helpCommand.HelpDescription);
    }
}